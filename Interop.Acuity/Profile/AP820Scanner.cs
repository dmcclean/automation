using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Interop.Acuity.Profile
{
    public sealed class AP820Scanner
    {
        private IntPtr _handle;
        private ScannerInformation _scannerInformation;

        private const uint DefaultTimeout = 500; // milliseconds
        private const int BufferLength = 3500;
        private const uint SuccessCode = 1;

        #region Constructors

        internal AP820Scanner(IntPtr handle)
        {
            _handle = handle;

            int attempts = 0;
            while (true)
            {
                switch (GetConnectionStatus())
                {
                    case ConnectionStatus.Connected:
                        goto Connected;
                    case ConnectionStatus.Connecting:
                        break;
                    default:
                        if (attempts > 3) throw new ApplicationException("Unable to connect to scanner.");
                        else break;
                }
                System.Threading.Thread.Sleep(500);
                attempts += 1;
            }

        Connected:
            GetScannerInformation();
        }

        #endregion

        #region Static Methods

        public static AP820Scanner Connect(System.Net.IPEndPoint endPoint)
        {
            var handle = NativeMethods.EthernetScanner_Connect(endPoint.Address.ToString(), endPoint.Port.ToString(), DefaultTimeout);

            if (handle == IntPtr.Zero) throw new ApplicationException("Unable to connect to scanner.");

            // TODO: verify connection

            return new AP820Scanner(handle);
        }

        public static AP820Scanner Connect(System.Net.IPAddress address, int port)
        {
            var endPoint = new System.Net.IPEndPoint(address, port);

            return Connect(endPoint);
        }

        public static AP820Scanner Connect(string ipAddress, int port)
        {
            var address = System.Net.IPAddress.Parse(ipAddress);

            return Connect(address, port);
        }

        public static string GetDllVersion()
        {
            // buffer must be at least 256 bytes
            const int BufferSize = 256;
            var resultBuilder = new StringBuilder(BufferSize);

            var resultCode = NativeMethods.EthernetScanner_GetVersion(resultBuilder, 256);

            if (resultCode <= 0 || resultCode > BufferSize) return resultBuilder.ToString();
            else throw new ApplicationException("An error occured while trying to get the scanner DLL version.");
        }

        #endregion

        public bool Disconnect()
        {
            AssertValidHandle();
            uint resultCode = NativeMethods.EthernetScanner_Disconnect(_handle);

            _handle = IntPtr.Zero;

            return (resultCode != 0);
        }

        public ConnectionStatus GetConnectionStatus()
        {
            AssertValidHandle();
            uint status = 0;
            NativeMethods.EthernetScanner_GetConnectStatus(_handle, ref status);

            return (ConnectionStatus)status;
        }

        public ScannerInformation GetScannerInformation()
        {
            AssertValidHandle();

            ScannerInformation result = new ScannerInformation();
            UInt32 resultCode = NativeMethods.EthernetScanner_GetInfo(_handle, ref result, (uint)Marshal.SizeOf(typeof(ScannerInformation)), DefaultTimeout);

            if (resultCode != 0)
            {
                _scannerInformation = result;
                return result;
            }
            else throw new ApplicationException("An error occured while attempting to retrieve sensor information.");
        }

        public ScanResult ReadScan()
        {
            return ReadScan(useSymmetricXCoordinates: true, timeoutMilliseconds: DefaultTimeout);
        }

        public ScanResult ReadScan(bool useSymmetricXCoordinates, uint timeoutMilliseconds)
        {
            AssertValidHandle();

            byte[] buffer = new byte[BufferLength];
            bool success = GetScanRawData(buffer, timeoutMilliseconds);

            if (!success) return null;
            else return ScanResult.FromSingleBuffer(_scannerInformation, buffer, useSymmetricXCoordinates);
        }

        public ScanResult ReadInterleavedScan(DroppedFrameResponse droppedFrameResponse)
        {
            return ReadInterleavedScan(droppedFrameResponse, useSymmetricXCoordinates: true, timeoutMilliseconds: DefaultTimeout);
        }

        public ScanResult ReadInterleavedScan(DroppedFrameResponse droppedFrameResponse, bool useSymmetricXCoordinates, uint timeoutMilliseconds)
        {
            AssertValidHandle();

            byte[] buffer1 = new byte[BufferLength];
            byte[] buffer2 = new byte[BufferLength];

            bool success = GetScanRawData(buffer1, timeoutMilliseconds);
            if (!success) return null;

            if (ScanResult.PeekImageNumber(buffer1) % 2 == 0)
            {
                // we want to start with an odd-numbered buffer, so repeat the process
                success = GetScanRawData(buffer1, timeoutMilliseconds);

                if (!success) return null;
            }

            // we now have a valid first image, get the second one
            success = GetScanRawData(buffer2, timeoutMilliseconds);
            if (!success) return null;

            return ScanResult.FromBufferPair(_scannerInformation, buffer1, buffer2, droppedFrameResponse, useSymmetricXCoordinates);
        }

        private bool GetScanRawData(byte[] buffer, uint timeout)
        {
            AssertValidHandle();

            var resultCode = NativeMethods.EthernetScanner_GetScanRawData(_handle, buffer, (uint)buffer.Length, 0, timeout);
            return resultCode == SuccessCode;
        }

        private UInt32 WriteData(byte[] message)
        {
            AssertValidHandle();

            return NativeMethods.EthernetScanner_WriteData(_handle, message, (uint)message.Length);
        }

        #region Configuration Methods

        public void ResetEncoderCount()
        {
            // This function simply sets the encoder counts to zero.
            var message = ConstructMessage(14, 0); // value is irrelevant

            WriteData(message);
        }

        public void SendSoftwareTrigger()
        {
            var message = ConstructMessage(29, 0); // value is irrelevant

            WriteData(message);
        }

        public void ResetFifoBuffer()
        {
            var message = ConstructMessage(28, 0); // value is irrelevant

            WriteData(message);
        }

        public void SetLaserPower(bool laserOn)
        {
            var message = ConstructMessage(12, laserOn ? 1 : 0);

            WriteData(message);
        }

        public void TurnOnLaser()
        {
            SetLaserPower(true);
        }

        public void TurnOffLaser()
        {
            SetLaserPower(false);
        }

        public void SetMeasurementControlMode(MeasurementControlMode mode)
        {
            var message = ConstructMessage(20, (int)mode);

            WriteData(message);
        }

        public void SetSynchronizationMode(SynchronizationMode mode)
        {
            var message = ConstructMessage(15, (int)mode);

            WriteData(message);
        }

        #endregion

        #region Configuration Helpers

        private static byte[] ConstructMessage(byte address, int value)
        {
            var message = new byte[2];

            message[0] = ConstructAddressByte(address);
            message[1] = ConstructDataByte(value);

            return message;
        }

        private static byte ConstructAddressByte(byte address)
        {
            if (address > 127) throw new ArgumentOutOfRangeException();
            return address;
        }

        private static byte ConstructDataByte(int value)
        {
            if (value > 127 || value < 0) throw new ArgumentOutOfRangeException();
            return (byte)((byte)0x80 | (byte)value);
        }

        #endregion

        private void AssertValidHandle()
        {
            if (_handle == IntPtr.Zero) throw new InvalidOperationException("Scanner connection has been closed.");
        }
    }
}
