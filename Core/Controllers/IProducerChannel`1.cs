﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutomationLibrary.Controllers
{
    public interface IProducerChannel<TValue>
    {
        bool TryReceiveValue(out TValue value);
    }
}
