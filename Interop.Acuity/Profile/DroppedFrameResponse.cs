using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Interop.Acuity.Profile
{
    public enum DroppedFrameResponse
    {
        Ignore,
        IgnoreOnce,
        ThrowException,
        ReturnNonInterleaved
    }
}
