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
        // TODO: connection handle
        private readonly CoilAddress _address;

        public CoilVariable(CoilAddress address)
        {
            _address = address;
        }

        public void SynchronousWrite(bool value)
        {
            throw new NotImplementedException();
        }

        public ILatch DeferredWrite(bool value)
        {
            throw new NotImplementedException();
        }

        public bool Read()
        {
            throw new NotImplementedException();
        }
    }
}
