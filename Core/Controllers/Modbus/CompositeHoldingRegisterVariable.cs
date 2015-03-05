using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutomationLibrary.Concurrency;

namespace AutomationLibrary.Controllers.Modbus
{
    internal sealed class CompositeHoldingRegisterVariable
        : IMutableVariable<ushort[]>
    {
        private readonly ModbusControllerConnection _connection;
        private readonly HoldingRegisterAddress _address;
        private readonly ushort _sizeInWords;

        public CompositeHoldingRegisterVariable(ModbusControllerConnection connection, HoldingRegisterAddress address, ushort sizeInWords)
        {
            _connection = connection;
            _address = address;
            _sizeInWords = sizeInWords;
        }

        public void SynchronousWrite(ushort[] value)
        {
            if (value == null) throw new ArgumentNullException();
            if (value.Length != _sizeInWords) throw new ArgumentException("Value is of incorrect size.");

            _connection.WriteHoldingRegisters(_address, value);
        }

        public ILatch DeferredWrite(ushort[] value)
        {
            SynchronousWrite(value);
            return Latch.AlreadyCompleted;
        }

        public ushort[] Read()
        {
            return _connection.ReadHoldingRegisters(_address, _sizeInWords);
        }
    }
}
