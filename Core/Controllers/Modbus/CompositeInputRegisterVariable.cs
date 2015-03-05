using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutomationLibrary.Controllers.Modbus
{
    internal sealed class CompositeInputRegisterVariable
        : IVariable<ushort[]>
    {
        private readonly ModbusControllerConnection _connection;
        private readonly InputRegisterAddress _address;
        private readonly ushort _sizeInWords;

        public CompositeInputRegisterVariable(ModbusControllerConnection connection, InputRegisterAddress address, ushort sizeInWords)
        {
            _connection = connection;
            _address = address;
            _sizeInWords = sizeInWords;
        }

        public ushort[] Read()
        {
            return _connection.ReadInputRegisters(_address, _sizeInWords);
        }
    }
}
