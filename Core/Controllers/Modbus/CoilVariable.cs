using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutomationLibrary.Concurrency;

namespace AutomationLibrary.Controllers.Modbus
{
    internal sealed class CoilVariable
        : IMutableVariable<bool>
    {
        private readonly ModbusControllerConnection _connection;
        private readonly CoilAddress _address;

        public CoilVariable(ModbusControllerConnection connection, CoilAddress address)
        {
            _connection = connection;
            _address = address;
        }

        public void SynchronousWrite(bool value)
        {
            _connection.WriteCoil(_address, value);
        }

        public ILatch DeferredWrite(bool value)
        {
            SynchronousWrite(value);
            return Latch.AlreadyCompleted;
        }

        public bool Read()
        {
            var result = _connection.ReadCoils(_address, 1);
            return result[0];
        }
    }
}
