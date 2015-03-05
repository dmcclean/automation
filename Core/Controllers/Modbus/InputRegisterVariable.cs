using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutomationLibrary.Controllers.Modbus
{
    internal sealed class InputRegisterVariable
        : IVariable<UInt16>
    {
        private readonly ModbusControllerConnection _connection;
        private readonly InputRegisterAddress _address;

        public InputRegisterVariable(ModbusControllerConnection connection, InputRegisterAddress address)
        {
            _connection = connection;
            _address = address;
        }

        public UInt16 Read()
        {
            var result = _connection.ReadInputRegisters(_address, 1);
            return result[0];
        }
    }
}
