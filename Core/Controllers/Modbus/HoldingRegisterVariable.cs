using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutomationLibrary.Concurrency;

namespace AutomationLibrary.Controllers.Modbus
{
    internal sealed class HoldingRegisterVariable
        : IMutableVariable<UInt16>
    {
        private readonly ModbusControllerConnection _connection;
        private readonly HoldingRegisterAddress _address;

        public HoldingRegisterVariable(ModbusControllerConnection connection, HoldingRegisterAddress address)
        {
            _connection = connection;
            _address = address;
        }

        public void SynchronousWrite(UInt16 value)
        {
            _connection.WriteHoldingRegister(_address, value);
        }

        public ILatch DeferredWrite(UInt16 value)
        {
            SynchronousWrite(value);
            return Latch.AlreadyCompleted;
        }

        public UInt16 Read()
        {
            var result = _connection.ReadHoldingRegisters(_address, 1);
            return result[0];
        }
    }
}
