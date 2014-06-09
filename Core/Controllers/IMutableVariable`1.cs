using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutomationLibrary.Concurrency;

namespace AutomationLibrary.Controllers
{
    public interface IMutableVariable<TValue>
        : IVariable<TValue>
    {
        void SynchronousWrite(TValue value);
        ILatch DeferredWrite(TValue value);
    }
}
