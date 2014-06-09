using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Interop.Acuity.Confocal
{
    [Flags]
    public enum DistanceModeDataItems : short
    {
        DistanceMsb         = 0x001,
        DistanceLsb         = 0x002,
        AdaptiveModeData    = 0x004,
        Intensity           = 0x008,
        // skip 0x010 and 0x020
        Barycenter          = 0x040,
        // skip 0x80
        State               = 0x100,
        Counter             = 0x200,
    }
}
