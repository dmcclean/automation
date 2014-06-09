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
        // TODO: connection handle
        private readonly HoldingRegisterAddress _address;

        public HoldingRegisterVariable(HoldingRegisterAddress address)
        {
            _address = address;
        }

        public void SynchronousWrite(UInt16 value)
        {
            throw new NotImplementedException();
        }

        public ILatch DeferredWrite(UInt16 value)
        {
            throw new NotImplementedException();
        }

        public UInt16 Read()
        {
            throw new NotImplementedException();
        }
    }
}
