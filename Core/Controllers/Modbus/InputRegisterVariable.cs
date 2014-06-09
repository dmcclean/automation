using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutomationLibrary.Controllers.Modbus
{
    internal sealed class InputRegisterVariable
        : IVariable<UInt16>
    {
        // TODO: connection handle
        private readonly InputRegisterAddress _address;

        public InputRegisterVariable(InputRegisterAddress address)
        {
            _address = address;
        }

        public UInt16 Read()
        {
            throw new NotImplementedException();
        }
    }
}
