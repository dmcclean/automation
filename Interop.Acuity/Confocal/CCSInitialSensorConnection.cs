using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Diagnostics;

namespace Interop.Acuity.Confocal
{
    public class CCSInitialSensorConnection
    {
        private readonly SerialPort _port;

        public CCSInitialSensorConnection(string portName)
        {
            _port = new SerialPort(portName);
            _port.BaudRate = 115200;
            _port.DataBits = 8;
            _port.Parity = Parity.None;
            _port.StopBits = StopBits.One;
            _port.Handshake = Handshake.None;
            _port.ReceivedBytesThreshold = 2;
            _port.NewLine = "\r";
            _port.ReadTimeout = 300;
        }

        public void EnableAsciiProtocol() 
        {
            SendCommand("$ASC");
        }

        public void EnableStateBasedTriggering()
        {
            SendCommand("$TRN1");
        }

        public void EnableAltitudeMode()
        {
            SendCommand("$RVS01");
        }

        public void EnableAutomaticAdaptiveIntensityMode()
        {
            SendCommand("$AAL1");
        }

        public void SetTransmittedItems(DistanceModeDataItems items)
        {
            var temp = (ushort)items;

            var cmd = new StringBuilder();
            cmd.Append("$SOD");
            for (int i = 0; i < 10; i++)
            {
                if (i != 0) cmd.Append(",");
                bool includeItemAtThisIndex = (temp & 0x001) == 0x001;
                cmd.Append(includeItemAtThisIndex ? "1" : "0");
                temp >>= 1;
            }

            cmd.Append("\r");
            var command = cmd.ToString();

            SendCommand(command);
        }

        public void SetPresetSampleRate(PresetSampleRate rate)
        {
            if ((int)rate < 0 || (int)rate > 5) throw new ArgumentOutOfRangeException();

            var command = string.Format("$SRA{0}", ((int)rate).ToString("00"));

            SendCommand(command);
        }

        public int ReadMeasuringRangeInMicrons()
        {
            var command = "$SCA";
            var response = SendCommand(command).Trim().Substring(4);

            int result;

            if (int.TryParse(response, out result)) return result;
            else return 0;
        }

        public void ClearReadBuffer()
        {
            if (_port.IsOpen)
            {
                byte[] buffer = new byte[1024];
                _port.ReadExisting();
                _port.DiscardInBuffer();
                _port.ReadExisting();
                while (_port.BytesToRead > 0)
                {
                    int numRead = _port.Read(buffer, 0, 1024);
                }
            }
        }

        public Tuple<double, int> ParseDistanceWithLsbAndSequenceNumber(int measuringRangeInMicrons)
        {
            char[] separators = new char[] { ',' };

            var text = _port.ReadLine();
            var segments = text.Split(separators, StringSplitOptions.None);

            if (segments.Length == 2)
            {
                int sequenceNumber;
                int msb;

                if (!int.TryParse(segments[1], out sequenceNumber)) throw new Exception(string.Format("Unable to parse sequence number. Raw value was '{0}'.", segments[1]));
                if (!int.TryParse(segments[0], out msb)) throw new Exception(string.Format("Unable to parse MSB of reading. Raw value was '{0}'.", segments[0]));

                long fullValue = msb;
                double coefficient = (double)measuringRangeInMicrons / Math.Pow(2, 15);
                double reading = (double)fullValue * coefficient;

                return Tuple.Create(reading, sequenceNumber);
            }

            Trace.WriteLine(string.Format("Parsing failed for a garbled point. Raw text was: {0}", text));

            // parsing failed
            return null;
        }

        public void DiscardTrash()
        {
            var existing = _port.ReadExisting();
            Trace.WriteLine(string.Format("Discard existing input: {0}", existing));
            _port.DiscardInBuffer();
        }

        private string SendCommand(string command)
        {
            Trace.WriteLine(string.Format("Sending message to CCS Initial sensor: {0}", command));

            _port.Write(command);
            Trace.WriteLine("Sent.");

            var response = _port.ReadLine();
            Trace.WriteLine(string.Format("Received response from CCS Initial:    {0}", response));

            return response;
        }

        public void Open()
        {
            if (_port.IsOpen) throw new InvalidOperationException("Serial port was already open.");
            _port.Open();
        }

        public void Close()
        {
            if (_port.IsOpen) _port.Close();
        }
    }
}
