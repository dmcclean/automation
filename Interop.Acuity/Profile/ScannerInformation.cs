using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Interop.Acuity.Profile
{
    [StructLayout(LayoutKind.Sequential)]
    public struct ScannerInformation
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst=16)]
        public string SerialNumber;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst=128)]
        public string FirmwareVersion;
        public UInt32 BeginningOfMeasuringRange;
        public UInt32 MeasuringRange;
        public UInt32 ScanRangeBeginning;
        public UInt32 ScanRangeEnd;
        public UInt32 MaxZLinearized;
        public UInt32 MaxXLinearized;
        public UInt32 MaxZNonLinearized;
        public UInt32 MaxXNonLinearized;
        public UInt32 NumZPixels;
        public UInt32 NumXPixels;
        public UInt32 ScannerTemperature;
        public UInt32 OperatingSecondsCounter;
        public UInt32 PowerCycleCounter;
        public UInt32 Fifo;
        public UInt32 PositionEncoder;
        public UInt32 PositionEncoderDirection;
        public UInt32 Protocol;
        public UInt32 Linearization;
        public UInt32 CameraMode;
        public UInt32 ProfileMode;
        public UInt32 ScannerMode;
        public Int32  ShutterTimeManual;
        public Int32  ShutterTimeAuto;
        public UInt32 PixelReadOutStart;
        public UInt32 PixelReadOutEnd;
        public UInt32 VideoGain;
        public UInt32 LaserIntensityThreshold;
        public UInt32 LaserTargetValue;
        public UInt32 PeakWidthLimit;
        public UInt32 PeakThreshold;
        public UInt32 Synchronization;
        public UInt32 ProtocolVersion;
        public UInt32 ShutterControl;
        public Int32  Linearization2;
        public UInt32 Speed;
        public UInt32 FPGAVersion;
        public UInt32 DigitalInput;
        public UInt32 LaserValueOfProfile;

        public double XScaleFactor
        {
            get
            {
                // see section 7.1 of manual
               return (1.0 / MaxXLinearized) * (ScanRangeEnd / 10.0); // this is the width of the scan region at the end of the scan range, in 0.1 mm
            }
        }

        public double ZScaleFactor
        {
            get
            {
                // see section 7.1 of manual
                return (1.0 / MaxZLinearized) * (MeasuringRange / 10.0);
            }
        }
    }
    /*
     * From Acuity Demo Software:
     * 
        <StructLayout(LayoutKind.Sequential)> Public Structure ScanInfoStruct
        <MarshalAs(UnmanagedType.ByValTStr, sizeconst:=16)> Public SerialNumber As String
        <MarshalAs(UnmanagedType.ByValTStr, sizeconst:=128)> Public FirmwareVersion As String
        Public BeginningOfMeasuringRange As UInteger
        Public MeasuringRange As UInteger
        Public ScanRangeBeginning As UInteger
        Public ScanRangeEnd As UInteger
        Public MaxZLinearized As UInteger
        Public MaxXLinearized As UInteger
        Public MaxZNonLinearized As UInteger
        Public MaxXNonLinearized As UInteger
        Public NumZPixels As UInteger
        Public NumXPixels As UInteger
        Public ScannerTemperature As UInteger
        Public OperatingSecondsCounter As UInteger
        Public PowerCycleCounter As UInteger
        Public Fifo As UInteger
        Public PositionEncoder As UInteger
        Public PositionEncoderDirection As UInteger
        Public Protocol As UInteger
        Public Linearization As UInteger
        Public CameraMode As UInteger
        Public ProfileMode As UInteger
        Public ScannerMode As UInteger
        Public ShutterTimeManual As Integer
        Public ShutterTimeAuto As Integer
        Public PixelReadOutStart As UInteger
        Public PixelReadOutEnd As UInteger
        Public VideoGain As UInteger
        Public LaserIntensityThreshold As UInteger
        Public LaserTargetValue As UInteger
        Public PeakWidthLimit As UInteger
        Public PeakThreshold As UInteger
        Public Synchronization As UInteger
        Public ProtocolVersion As UInteger
        Public ShutterControl As UInteger
        Public Linearization2 As Integer
        Public Speed As UInteger
        Public FPGAVersion As UInteger
        Public DigitalInput As UInteger
        Public LaserValueOfProfile As UInteger
    End Structure
    */
}
