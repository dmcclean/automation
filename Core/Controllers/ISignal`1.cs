using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutomationLibrary.Collections;

namespace AutomationLibrary.Controllers
{
    public interface ISignal<TValue>
    {
        bool ReadAvailable(RingBuffer<TValue> buffer);
    }
}
