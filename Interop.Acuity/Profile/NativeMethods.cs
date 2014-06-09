using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Interop.Acuity.Profile
{
    internal static class NativeMethods
    {
        [DllImport("EthernetScanner.dll")]
        public static extern IntPtr EthernetScanner_Connect(string IPAddr, string port, UInt32 timeout);
        [DllImport("EthernetScanner.dll")]
        public static extern UInt32 EthernetScanner_Disconnect(IntPtr pScanner);
        [DllImport("EthernetScanner.dll")]
        public static extern void EthernetScanner_GetConnectStatus(IntPtr pScanner, ref UInt32 status);
        [DllImport("EthernetScanner.dll")]
        public static extern bool EthernetScanner_GetInfoRaw(IntPtr pScanner, byte[] buffer, UInt32 bufferSize, UInt32 timeout);
        [DllImport("EthernetScanner.dll")]
        public static extern UInt32 EthernetScanner_GetInfo(IntPtr pScanner, ref ScannerInformation scanInfo, UInt32 scanInfoSize, UInt32 timeout);
        [DllImport("EthernetScanner.dll")]
        public static extern UInt32 EthernetScanner_WriteData(IntPtr pScanner, byte[] buffer, UInt32 bufferSize);
        [DllImport("EthernetScanner.dll")]
        public static extern UInt32 EthernetScanner_GetVersion([MarshalAs(UnmanagedType.LPStr), Out]StringBuilder version, UInt32 length);
        [DllImport("EthernetScanner.dll")]
        public static extern UInt32 EthernetScanner_GetScanRawData(IntPtr pScanner, byte[] buffer, UInt32 bufferSize, UInt32 scanProfileMode, UInt32 timeout);
    }
}
