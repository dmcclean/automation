using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace AutomationLibrary.Concurrency
{
    internal sealed class ManualResetEventLatch
        : ILatch
    {
        private readonly ManualResetEventSlim _event;

        public ManualResetEventLatch(ManualResetEventSlim @event)
        {
            _event = @event;
        }

        public void Await(int millisecondsTimeout)
        {
            _event.Wait(millisecondsTimeout);
        }

        public void Await(int millisecondsTimeout, CancellationToken cancellationToken)
        {
            _event.Wait(millisecondsTimeout, cancellationToken);
        }

        public WaitHandle ToWaitHandle()
        {
            return _event.WaitHandle;
        }
    }
}
