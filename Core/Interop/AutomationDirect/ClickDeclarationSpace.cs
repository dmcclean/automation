using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using AutomationLibrary.Controllers;
using AutomationLibrary.Controllers.Modbus;

namespace MassBayEngineering.Interop.AutomationDirect
{
    public sealed class ClickDeclarationSpace
        : IDeclarationSpace<string>
    {
        private readonly IDeclarationSpace<DatumAddress> _underlying;
        private readonly Dictionary<ClickVariableType, AddressSpace> _addressMap;

        private static readonly Regex AddressPattern = new Regex(@"^(?<prefix>X|Y|C|T|CT|SC|DS|DD|DH|DF|XD|YD|TD|CTD|SD|TXT)(?<num>\d+)$");

        public ClickDeclarationSpace(IDeclarationSpace<DatumAddress> underlying)
        {
            _underlying = underlying;
            _addressMap = new Dictionary<ClickVariableType, AddressSpace>();

            RegisterAddressSpace(ClickVariableType.X,   DatumAddressKind.Input,           0x0000, typeof(bool),  1, 816);
            RegisterAddressSpace(ClickVariableType.Y,   DatumAddressKind.Coil,            0x2000, typeof(bool),  1, 816);
            RegisterAddressSpace(ClickVariableType.C,   DatumAddressKind.Coil,            0x4000, typeof(bool),  1, 2000);
            RegisterAddressSpace(ClickVariableType.T,   DatumAddressKind.Input,           0xB000, typeof(bool),  1, 500);
            RegisterAddressSpace(ClickVariableType.CT,  DatumAddressKind.Input,           0xC000, typeof(bool),  1, 250);
            RegisterAddressSpace(ClickVariableType.SC,  DatumAddressKind.Input,           0xF000, typeof(bool),  1, 1000);
            RegisterAddressSpace(ClickVariableType.DS,  DatumAddressKind.HoldingRegister, 0x0000, typeof(short), 1, 4500);
            RegisterAddressSpace(ClickVariableType.DD,  DatumAddressKind.HoldingRegister, 0x4000, typeof(int),   2, 1000);
            RegisterAddressSpace(ClickVariableType.DH,  DatumAddressKind.HoldingRegister, 0x6000, typeof(short), 1, 500);
            RegisterAddressSpace(ClickVariableType.DF,  DatumAddressKind.HoldingRegister, 0x7000, typeof(float), 2, 500);
            RegisterAddressSpace(ClickVariableType.XD,  DatumAddressKind.InputRegister,   0xE000, typeof(uint),  2, 0, 8);
            RegisterAddressSpace(ClickVariableType.YD,  DatumAddressKind.HoldingRegister, 0xE200, typeof(uint),  2, 0, 8);
            RegisterAddressSpace(ClickVariableType.TD,  DatumAddressKind.InputRegister,   0xB000, typeof(short), 1, 500);
            RegisterAddressSpace(ClickVariableType.CTD, DatumAddressKind.InputRegister,   0xC000, typeof(int),   2, 250);
            RegisterAddressSpace(ClickVariableType.SD,  DatumAddressKind.InputRegister,   0xF000, typeof(short), 1, 1000);
            RegisterAddressSpace(ClickVariableType.TXT, DatumAddressKind.HoldingRegister, 0x9000, typeof(char),  1, 1000);
        }

        #region Address Space

        private void RegisterAddressSpace(ClickVariableType variableType, DatumAddressKind addressKind, int baseAddress, Type datumType, int datumSize, int minLogicalAddress, int maxLogicalAddress)
        {
            var space = new AddressSpace(variableType, addressKind, baseAddress, datumType, datumSize, minLogicalAddress, maxLogicalAddress);
            _addressMap.Add(variableType, space);
        }

        private void RegisterAddressSpace(ClickVariableType variableType, DatumAddressKind addressKind, int baseAddress, Type datumType, int datumSize, int maxLogicalAddress)
        {
            RegisterAddressSpace(variableType, addressKind, baseAddress, datumType, datumSize, 1, maxLogicalAddress);
        }

        private sealed class AddressSpace
        {
            public ClickVariableType Type { get; set; }
            public DatumAddressKind Kind { get; set; }
            public int BaseAddress { get; set; }
            public Type DatumType { get; set; }
            public int DatumSize { get; set; }
            public int MinLogicalAddress { get; set; }
            public int MaxLogicalAddress { get; set; }

            public AddressSpace(ClickVariableType type, DatumAddressKind kind, int baseAddress, Type datumType, int datumSize, int minLogicalAddress, int maxLogicalAddress)
            {
                this.Type = type;
                this.Kind = kind;
                this.BaseAddress = baseAddress;
                this.DatumType = datumType;
                this.DatumSize = datumSize;
                this.MinLogicalAddress = minLogicalAddress;
                this.MaxLogicalAddress = maxLogicalAddress;
            }

            public DatumAddress CreateAddress(int numericValue)
            {
                if (!IsValid(numericValue)) throw new ArgumentOutOfRangeException();

                var raw = ComputeAddress(numericValue);

                switch (Kind)
                {
                    case DatumAddressKind.Coil:
                        return CoilAddress.FromRawValue(raw);
                    case DatumAddressKind.Input:
                        return InputAddress.FromRawValue(raw);
                    case DatumAddressKind.HoldingRegister:
                        return HoldingRegisterAddress.FromRawValue(raw);
                    case DatumAddressKind.InputRegister:
                        return InputRegisterAddress.FromRawValue(raw);
                    default:
                        throw new ApplicationException();

                }
            }

            private ushort ComputeAddress(int numericValue)
            {
                switch (Type)
                {
                    case ClickVariableType.X:
                    case ClickVariableType.Y:
                        // deal with the fact that they are in chunks of 16
                        var hundreds = numericValue / 100;
                        var remainder = numericValue % 100;

                        return (ushort)(BaseAddress + (0x20 * hundreds) + remainder - 1);
                    default:
                        return (ushort)(BaseAddress + DatumSize * (numericValue - MinLogicalAddress));
                }
            }

            private bool IsValid(int numericValue)
            {
                if (numericValue < MinLogicalAddress) return false;
                else if (numericValue > MaxLogicalAddress) return false;
                else
                {
                    switch (Type)
                    {
                        case ClickVariableType.X:
                        case ClickVariableType.Y:
                            // check if they are 1-16 for some hundred
                            var lastTwoDigits = numericValue % 100;
                            if (lastTwoDigits == 0 || lastTwoDigits > 16) return false;
                            else return true;
                        default:
                            return true;
                    }
                }
            }
        }

        #endregion

        #region Parsing

        private Tuple<DatumAddress, Type, int> ParseClickAddress(string clickAddress)
        {
            var match = AddressPattern.Match(clickAddress);
            if (!match.Success) throw new ArgumentException();
            else
            {
                var prefix = (ClickVariableType)Enum.Parse(typeof(ClickVariableType), match.Groups["prefix"].Value);
                int numericValue = int.Parse(match.Groups["num"].Value);

                var addressSpace = _addressMap[prefix];
                var address = addressSpace.CreateAddress(numericValue);

                return Tuple.Create(address, addressSpace.DatumType, addressSpace.DatumSize);
            }
        }

        private static Tuple<DatumAddress, Type, int> CreateCoil(int numericValue, int baseValue)
        {
            return Tuple.Create((DatumAddress)CoilAddress.FromInteger(numericValue), typeof(bool), 1);
        }

        private static Tuple<DatumAddress, Type, int> CreateInput(int numericValue, int baseValue)
        {
            return Tuple.Create((DatumAddress)InputAddress.FromInteger(numericValue), typeof(bool), 1);
        }

        private static Tuple<DatumAddress, Type, int> CreateHoldingRegister(int numericValue, int baseValue, Type type, int size)
        {
            var raw = (ushort)(baseValue + size * (numericValue - 1));

            return Tuple.Create((DatumAddress)HoldingRegisterAddress.FromRawValue(raw), type, size);
        }

        private static Tuple<DatumAddress, Type, int> CreateInputRegister(int numericValue, int baseValue, Type type, int size)
        {
            var raw = (ushort)(baseValue + size * (numericValue - 1));

            return Tuple.Create((DatumAddress)InputRegisterAddress.FromRawValue(raw), type, size);
        }

        #endregion

        #region Generic Interface

        public IVariable<TValue> GetReadOnlyVariable<TValue>(string name)
        {
            var parse = ParseClickAddress(name);

            if (typeof(TValue) != parse.Item2) throw new ArgumentException("The requested variable does not have the requested type.");

            switch (parse.Item1.Kind)
            {
                case DatumAddressKind.Coil:
                case DatumAddressKind.Input:
                    var rawBool = _underlying.GetReadOnlyVariable<bool>(parse.Item1);
                    return rawBool as IVariable<TValue>;
                case DatumAddressKind.HoldingRegister:
                case DatumAddressKind.InputRegister:
                    switch (Type.GetTypeCode(parse.Item2))
                    {
                        case TypeCode.UInt16:
                            return _underlying.GetReadOnlyVariable<ushort>(parse.Item1) as IVariable<TValue>;
                        case TypeCode.Int16:
                            return _underlying.GetReadOnlyVariable<short>(parse.Item1) as IVariable<TValue>;
                        case TypeCode.Char:
                            return _underlying.GetReadOnlyVariable<char>(parse.Item1) as IVariable<TValue>;
                        case TypeCode.Single:
                            return _underlying.GetReadOnlyVariable<float>(parse.Item1) as IVariable<TValue>;
                        case TypeCode.Int32:
                            return _underlying.GetReadOnlyVariable<int>(parse.Item1) as IVariable<TValue>;
                        case TypeCode.UInt32:
                            return _underlying.GetReadOnlyVariable<uint>(parse.Item1) as IVariable<TValue>;
                        default:
                            throw new ApplicationException();
                    }
                default:
                    throw new ApplicationException();
            }
        }

        public IMutableVariable<TValue> GetVariable<TValue>(string name)
        {
            var parse = ParseClickAddress(name);

            if (typeof(TValue) != parse.Item2) throw new ArgumentException("The requested variable does not have the requested type.");

            switch (parse.Item1.Kind)
            {
                case DatumAddressKind.Input:
                case DatumAddressKind.InputRegister:
                    throw new ArgumentException("The specified variable is read-only.");
                case DatumAddressKind.Coil:
                    var rawBool = _underlying.GetReadOnlyVariable<bool>(parse.Item1);
                    return rawBool as IMutableVariable<TValue>;
                case DatumAddressKind.HoldingRegister:
                    switch (Type.GetTypeCode(parse.Item2))
                    {
                        case TypeCode.UInt16:
                            return _underlying.GetVariable<ushort>(parse.Item1) as IMutableVariable<TValue>;
                        case TypeCode.Int16:
                            return _underlying.GetVariable<short>(parse.Item1) as IMutableVariable<TValue>;
                        case TypeCode.Char:
                            return _underlying.GetVariable<char>(parse.Item1) as IMutableVariable<TValue>;
                        case TypeCode.Single:
                            return _underlying.GetVariable<float>(parse.Item1) as IMutableVariable<TValue>;
                        case TypeCode.Int32:
                            return _underlying.GetVariable<int>(parse.Item1) as IMutableVariable<TValue>;
                        case TypeCode.UInt32:
                            return _underlying.GetVariable<uint>(parse.Item1) as IMutableVariable<TValue>;
                        default:
                            throw new ApplicationException();
                    }
                default:
                    throw new ApplicationException();
            }
        }

        #endregion

        #region Specifically Typed Click Interface

        public IVariable<bool> X(int value)
        {
            var address = _addressMap[ClickVariableType.X].CreateAddress(value);
            return _underlying.GetReadOnlyVariable<bool>(address);
        }

        public IMutableVariable<bool> Y(int value)
        {
            var address = _addressMap[ClickVariableType.Y].CreateAddress(value);
            return _underlying.GetVariable<bool>(address);
        }

        public IMutableVariable<bool> C(int value)
        {
            var address = _addressMap[ClickVariableType.C].CreateAddress(value);
            return _underlying.GetVariable<bool>(address);
        }

        public IVariable<bool> T(int value)
        {
            var address = _addressMap[ClickVariableType.T].CreateAddress(value);
            return _underlying.GetReadOnlyVariable<bool>(address);
        }

        public IVariable<bool> CT(int value)
        {
            var address = _addressMap[ClickVariableType.C].CreateAddress(value);
            return _underlying.GetReadOnlyVariable<bool>(address);
        }

        public IVariable<bool> SC(int value)
        {
            var address = _addressMap[ClickVariableType.SC].CreateAddress(value);
            return _underlying.GetReadOnlyVariable<bool>(address);
        }

        public IMutableVariable<short> DS(int value)
        {
            var address = _addressMap[ClickVariableType.DS].CreateAddress(value);
            return _underlying.GetVariable<short>(address);
        }

        public IMutableVariable<int> DD(int value)
        {
            var address = _addressMap[ClickVariableType.DD].CreateAddress(value);
            return _underlying.GetVariable<int>(address);
        }

        public IMutableVariable<short> DH(int value)
        {
            var address = _addressMap[ClickVariableType.DH].CreateAddress(value);
            return _underlying.GetVariable<short>(address);
        }

        public IMutableVariable<float> DF(int value)
        {
            var address = _addressMap[ClickVariableType.DF].CreateAddress(value);
            return _underlying.GetVariable<float>(address);
        }

        public IVariable<uint> XD(int value)
        {
            var address = _addressMap[ClickVariableType.XD].CreateAddress(value);
            return _underlying.GetReadOnlyVariable<uint>(address);
        }

        public IMutableVariable<uint> YD(int value)
        {
            var address = _addressMap[ClickVariableType.YD].CreateAddress(value);
            return _underlying.GetVariable<uint>(address);
        }

        public IVariable<short> TD(int value)
        {
            var address = _addressMap[ClickVariableType.TD].CreateAddress(value);
            return _underlying.GetReadOnlyVariable<short>(address);
        }

        public IVariable<int> CTD(int value)
        {
            var address = _addressMap[ClickVariableType.CTD].CreateAddress(value);
            return _underlying.GetReadOnlyVariable<int>(address);
        }

        public IVariable<short> SD(int value)
        {
            var address = _addressMap[ClickVariableType.SD].CreateAddress(value);
            return _underlying.GetReadOnlyVariable<short>(address);
        }

        public IMutableVariable<char> TXT(int value)
        {
            var address = _addressMap[ClickVariableType.TXT].CreateAddress(value);
            return _underlying.GetVariable<char>(address);
        }

        #endregion

        public AutomationLibrary.Concurrency.IWaitHandle GetWaitHandle(string name)
        {
            throw new NotImplementedException();
        }

        public IProducerChannel<TValue> GetProducerChannel<TValue>(string name)
        {
            throw new NotImplementedException();
        }

        public IConsumerChannel<TValue> GetConsumerChannel<TValue>(string name)
        {
            throw new NotImplementedException();
        }

        public ISignal<TValue> GetSignal<TValue>(string name)
        {
            throw new NotImplementedException();
        }
    }
}
