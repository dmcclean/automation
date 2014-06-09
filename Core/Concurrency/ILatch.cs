using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace AutomationLibrary.Concurrency
{
    public interface ILatch
    {
        // throws TimeoutException
        void Await(int millisecondsTimeout);
        void Await(int millisecondsTimeout, CancellationToken cancellationToken);
        WaitHandle ToWaitHandle();
    }
}
