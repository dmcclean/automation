using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Interop.Acuity.Profile
{
    public enum MeasurementControlMode
    {
        Continuous = 0,
        Triggered = 0x08 // stored in bit 3
    }
}
