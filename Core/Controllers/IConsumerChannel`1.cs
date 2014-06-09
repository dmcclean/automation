using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutomationLibrary.Controllers
{
    public interface IConsumerChannel<TValue>
    {
        void SendValue(TValue value);
    }
}
