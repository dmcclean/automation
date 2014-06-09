using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutomationLibrary.Controllers.Modbus
{
    internal sealed class InputVariable
        : IVariable<bool>
    {
        // TODO: connection handle
        private readonly InputAddress _address;

        public InputVariable(InputAddress address)
        {
            _address = address;
        }

        public bool Read()
        {
            throw new NotImplementedException();
        }
    }
}
