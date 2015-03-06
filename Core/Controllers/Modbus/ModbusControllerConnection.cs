using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NModbus = global::Modbus;

namespace AutomationLibrary.Controllers.Modbus
{
    public sealed class ModbusControllerConnection
        : IControllerConnection<DatumAddress>,
          IDisposable
    {
        private sealed class DeclarationSpace
            : IDeclarationSpace<DatumAddress>
        {
            private readonly ModbusControllerConnection _connection;

            public DeclarationSpace(ModbusControllerConnection connection)
            {
                _connection = connection;
            }

            public IVariable<TValue> GetReadOnlyVariable<TValue>(DatumAddress name)
            {
                switch (name.Kind)
                {
                    case DatumAddressKind.Coil:
                        if (typeof(TValue) != typeof(bool)) throw new ArgumentException("Coil variables must be boolean.");
                        return new CoilVariable(_connection, (CoilAddress)name) as IVariable<TValue>;
                    case DatumAddressKind.Input:
                        if (typeof(TValue) != typeof(bool)) throw new ArgumentException("Input variables must be boolean.");
                        return new InputVariable(_connection, (InputAddress)name) as IVariable<TValue>;
                    case DatumAddressKind.InputRegister:
                        return WrapReadOnlyRegister<InputRegisterAddress, TValue>(
                            (InputRegisterAddress)name,
                            n => new InputRegisterVariable(_connection, n),
                            (n, size) => new CompositeInputRegisterVariable(_connection, n, size));
                    case DatumAddressKind.HoldingRegister:
                        return WrapReadOnlyRegister<HoldingRegisterAddress, TValue>(
                            (HoldingRegisterAddress)name,
                            n => new HoldingRegisterVariable(_connection, n),
                            (n, size) => new CompositeHoldingRegisterVariable(_connection, n, size));
                    default:
                        throw new ArgumentException();
                }
            }

            public IMutableVariable<TValue> GetVariable<TValue>(DatumAddress name)
            {
                switch (name.Kind)
                {
                    case DatumAddressKind.Coil:
                        if (typeof(TValue) != typeof(bool)) throw new ArgumentException("Coil variables must be boolean.");
                        return new CoilVariable(_connection, (CoilAddress)name) as IMutableVariable<TValue>;
                    case DatumAddressKind.HoldingRegister:
                        return WrapRegister<HoldingRegisterAddress, TValue>(
                            (HoldingRegisterAddress)name,
                            n => new HoldingRegisterVariable(_connection, n),
                            (n, size) => new CompositeHoldingRegisterVariable(_connection, n, size));
                    case DatumAddressKind.Input:
                    case DatumAddressKind.InputRegister:
                        throw new ArgumentException("Inputs and input registers are inherently read-only.");
                    default:
                        throw new ArgumentException();
                }
            }

            private static int IntFromArray(ushort[] value)
            {
                return unchecked((int)UIntFromArray(value));
            }

            private static ushort[] IntToArray(int value)
            {
                return UIntToArray(unchecked((uint)value));
            }

            private static uint UIntFromArray(ushort[] value)
            {
                var result = unchecked((uint)((value[1] << 16) | value[0]));

                return result;
            }

            private static ushort[] UIntToArray(uint value)
            {
                unchecked
                {
                    var high = (ushort)(value >> 16);
                    var low = (ushort)value;

                    var result = new ushort[] { low, high };

                    return result;
                }
            }

            private static float FloatFromArray(ushort[] value)
            {
                unchecked
                {
                    var bytes = new byte[4];
                    bytes[0] = (byte)(value[0]);
                    bytes[1] = (byte)(value[0] >> 8);
                    bytes[2] = (byte)(value[1]);
                    bytes[3] = (byte)(value[1] >> 8);

                    if (!BitConverter.IsLittleEndian) bytes.Reverse();
                    // bytes are now host order

                    var result = BitConverter.ToSingle(bytes, 0);

                    return result;
                }
            }

            private static ushort[] FloatToArray(float value)
            {
                var bytes = BitConverter.GetBytes(value);
                if (!BitConverter.IsLittleEndian) bytes.Reverse();

                // bytes are now big-endian

                var high = (ushort)(bytes[1] << 8 | bytes[0]);
                var low = (ushort)(bytes[3] << 8 | bytes[2]);

                var result = new ushort[] { high, low };

                return result;
            }

            private IVariable<TValue> WrapReadOnlyRegister<TAddress, TValue>(TAddress address, Func<TAddress, IVariable<ushort>> createSingle, Func<TAddress, ushort, IVariable<ushort[]>> createComposite)
            {
                if (typeof(TValue) == typeof(ushort)) return createSingle(address) as IVariable<TValue>;
                else if (typeof(TValue) == typeof(short)) return createSingle(address).Wrap(x => unchecked((short)x)) as IVariable<TValue>;
                else if (typeof(TValue) == typeof(byte)) return createSingle(address).Wrap(x => unchecked((byte)x & 0xff)) as IVariable<TValue>;
                else if (typeof(TValue) == typeof(char)) return createSingle(address).Wrap(x => unchecked((char)x & 0xff)) as IVariable<TValue>;
                else if (typeof(TValue) == typeof(int)) return createComposite(address, 2).Wrap(IntFromArray) as IVariable<TValue>;
                else if (typeof(TValue) == typeof(uint)) return createComposite(address, 2).Wrap(UIntFromArray) as IVariable<TValue>;
                else if (typeof(TValue) == typeof(float)) return createComposite(address, 2).Wrap(FloatFromArray) as IVariable<TValue>;
                else throw new Exception("Unsupported register type.");
            }

            private IMutableVariable<TValue> WrapRegister<TAddress, TValue>(TAddress address, Func<TAddress, IMutableVariable<ushort>> createSingle, Func<TAddress, ushort, IMutableVariable<ushort[]>> createComposite)
            {
                if (typeof(TValue) == typeof(ushort)) return createSingle(address) as IMutableVariable<TValue>;
                else if (typeof(TValue) == typeof(short)) return createSingle(address).Wrap(x => unchecked((short)x), x => unchecked((ushort)x)) as IMutableVariable<TValue>;
                else if (typeof(TValue) == typeof(byte)) return createSingle(address).Wrap(x => unchecked((byte)(x & 0xff)), x => unchecked((ushort)x)) as IMutableVariable<TValue>;
                else if (typeof(TValue) == typeof(char)) return createSingle(address).Wrap(x => unchecked((char)(x & 0xff)), x => unchecked((ushort)x)) as IMutableVariable<TValue>;
                else if (typeof(TValue) == typeof(int)) return createComposite(address, 2).Wrap(IntFromArray, IntToArray) as IMutableVariable<TValue>;
                else if (typeof(TValue) == typeof(uint)) return createComposite(address, 2).Wrap(UIntFromArray, UIntToArray) as IMutableVariable<TValue>;
                else if (typeof(TValue) == typeof(float)) return createComposite(address, 2).Wrap(FloatFromArray, FloatToArray) as IMutableVariable<TValue>;
                else throw new Exception("Unsupported register type.");
            }

            public Concurrency.IWaitHandle GetWaitHandle(DatumAddress name)
            {
                throw new NotImplementedException();
            }

            public IProducerChannel<TValue> GetProducerChannel<TValue>(DatumAddress name)
            {
                throw new NotImplementedException();
            }

            public IConsumerChannel<TValue> GetConsumerChannel<TValue>(DatumAddress name)
            {
                throw new NotImplementedException();
            }

            public ISignal<TValue> GetSignal<TValue>(DatumAddress name)
            {
                throw new NotImplementedException();
            }
        }

        private readonly NModbus.Device.IModbusMaster _master;
        private readonly SlaveAddress _slave;
        private readonly byte _slaveRaw;
        private readonly DeclarationSpace _declarationSpace;

        internal ModbusControllerConnection(NModbus.Device.IModbusMaster master, SlaveAddress slave)
        {
            _master = master;
            _slave = slave;
            _slaveRaw = slave.WireValue;
            _declarationSpace = new DeclarationSpace(this);
        }

        internal bool[] ReadCoils(CoilAddress firstCoil, ushort numberOfCoils)
        {
            return _master.ReadCoils(_slaveRaw, firstCoil.WireValue, numberOfCoils);
        }

        internal void WriteCoil(CoilAddress coil, bool value)
        {
            _master.WriteSingleCoil(_slaveRaw, coil.WireValue, value);
        }

        internal ushort[] ReadHoldingRegisters(HoldingRegisterAddress firstRegister, ushort numberOfRegisters)
        {
            return _master.ReadHoldingRegisters(_slaveRaw, firstRegister.WireValue, numberOfRegisters);
        }

        internal void WriteHoldingRegisters(HoldingRegisterAddress firstRegister, ushort[] values)
        {
            _master.WriteMultipleRegisters(_slaveRaw, firstRegister.WireValue, values);
        }

        internal void WriteHoldingRegister(HoldingRegisterAddress register, ushort value)
        {
            _master.WriteSingleRegister(_slaveRaw, register.WireValue, value);
        }

        internal bool[] ReadInputs(InputAddress firstInput, ushort numberOfInputs)
        {
            return _master.ReadInputs(_slaveRaw, firstInput.WireValue, numberOfInputs);
        }

        internal ushort[] ReadInputRegisters(InputRegisterAddress firstRegister, ushort numberOfRegisters)
        {
            return _master.ReadInputRegisters(_slaveRaw, firstRegister.WireValue, numberOfRegisters);
        }

        public IDeclarationSpace<DatumAddress> RootNamespace
        {
            get { return _declarationSpace; }
        }

        public void Dispose()
        {
            if (_master != null) _master.Dispose();
        }
    }
}
