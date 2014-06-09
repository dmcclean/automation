using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutomationLibrary.Controllers.Modbus
{
    public enum FunctionCode
        : byte
    {
        ReadCoilStatus = 1,
        ReadInputStatus = 2,
        ReadHoldingRegisters = 3,
        ReadInputRegisters = 4,
        ForceSingleCoil = 5,
        PresetSingleRegister = 6,
        ReadExceptionStatus = 7,      
        Diagnostics = 8,
        ForceMultipleCoils = 15,
        PresetMultipleRegisters = 16,
        ReportSlaveID = 17,
        ReadWriteMultipleRegisters = 23,
    }
}
