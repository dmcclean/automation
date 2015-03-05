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

        private static readonly Regex AddressPattern = new Regex(@"^(?<prefix>X|Y|C|T|CT|SC|DS|DD|DH|DF|XD|YD|TD|CTD|SD|TXT)(?<num>\d+)$");

        public ClickDeclarationSpace(IDeclarationSpace<DatumAddress> underlying)
        {
            _underlying = underlying;
        }

        #region Parsing

        private Tuple<DatumAddress, Type, int> ParseClickAddress(string clickAddress)
        {
            var match = AddressPattern.Match(clickAddress);
            if (!match.Success) throw new ArgumentException();
            else
            {
                var prefix = (ClickVariableType)Enum.Parse(typeof(ClickVariableType), match.Groups["prefix"].Value);
                int numericValue = int.Parse(match.Groups["num"].Value);

                switch (prefix)
                {
                    case ClickVariableType.X:
                        return CreateInput(numericValue, 0x0000);
                    case ClickVariableType.Y:
                        return CreateCoil(numericValue, 0x2000);
                    case ClickVariableType.C:
                        return CreateCoil(numericValue, 0x4000);
                    case ClickVariableType.T:
                        return CreateInput(numericValue, 0xB000);
                    case ClickVariableType.CT:
                        return CreateInput(numericValue, 0xC000);
                    case ClickVariableType.SC:
                        return CreateInput(numericValue, 0xF000);
                    case ClickVariableType.DS:
                        return CreateHoldingRegister(numericValue, 0x0000, typeof(short), 1);
                    case ClickVariableType.DD:
                        return CreateHoldingRegister(numericValue, 0x4000, typeof(int), 2);;
                    case ClickVariableType.DH:
                        return CreateHoldingRegister(numericValue, 0x6000, typeof(short), 1);
                    case ClickVariableType.DF:
                        return CreateHoldingRegister(numericValue, 0x7000, typeof(float), 2);
                    case ClickVariableType.XD:
                        return CreateInputRegister(numericValue, 0xE000, typeof(uint), 2);
                    case ClickVariableType.YD:
                        return CreateHoldingRegister(numericValue, 0xE200, typeof(uint), 2);
                    case ClickVariableType.TD:
                        return CreateInputRegister(numericValue, 0xB000, typeof(short), 1);
                    case ClickVariableType.CTD:
                        return CreateInputRegister(numericValue, 0xC000, typeof(int), 2);
                    case ClickVariableType.SD:
                        return CreateInputRegister(numericValue, 0xF000, typeof(short), 1);
                    case ClickVariableType.TXT:
                        return CreateHoldingRegister(numericValue, 0x9000, typeof(char), 1);
                    default:
                        throw new ArgumentException();
                }
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
