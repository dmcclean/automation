using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace AutomationLibrary.Concurrency
{
    public interface IWaitHandle
    {
        // TODO: change result type
        void TryAcquire(TimeSpan timeout);
        void Release();
    }
}
