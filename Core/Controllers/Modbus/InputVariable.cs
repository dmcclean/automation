using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutomationLibrary.Controllers.Modbus
{
    internal sealed class InputVariable
        : IVariable<bool>
    {
        private readonly ModbusControllerConnection _connection;
        private readonly InputAddress _address;

        public InputVariable(ModbusControllerConnection connection, InputAddress address)
        {
            _connection = connection;
            _address = address;
        }

        public bool Read()
        {
            var result = _connection.ReadInputs(_address, 1);
            return result[0];
        }
    }
}
