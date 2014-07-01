using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Interop.Acuity.Profile
{
    public enum SynchronizationMode
    {
        Simultaneous = 0,
        Alternating = 0x01 // stored in bit 1
    }
}
