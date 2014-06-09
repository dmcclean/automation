using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutomationLibrary.Controllers.Modbus
{
    public enum DatumAddressKind
        : byte
    {
        Coil = 0,
        Input = 1,
        InputRegister = 3,
        HoldingRegister = 4
    }
}
