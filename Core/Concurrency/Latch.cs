using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace AutomationLibrary.Concurrency
{
    public static class Latch
    {
        private sealed class SetLatch
            : ILatch
        {
            public void Await(int millisecondsTimeout)
            {
                return;
            }

            public void Await(int millisecondsTimeout, CancellationToken cancellationToken)
            {
                return;
            }

            public WaitHandle ToWaitHandle()
            {
                var @event = new ManualResetEvent(initialState: true); // initially signaled
                return @event;
            }
        }

        public static readonly ILatch AlreadyCompleted = new SetLatch();

        public static ILatch CreateLatch(out Action signal)
        {
            var @event = new ManualResetEventSlim();
            var latch = new ManualResetEventLatch(@event);

            signal = delegate() { @event.Set(); };

            return latch;
        }
    }
}
