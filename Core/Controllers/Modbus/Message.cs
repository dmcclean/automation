using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AutomationLibrary.Controllers.Modbus
{
    public sealed class Message
    {
        private readonly byte[] _data;

        private Message(byte[] data)
        {
            _data = data;
        }

        private static Message CreateFromRaw(List<byte> data)
        {
            var fullData = new byte[data.Count + 2];
            data.CopyTo(fullData);

            var crc = CalculateCRC(fullData, data.Count);

            var high = (byte)(crc >> 8);
            var low = (byte)(crc & 0xff);

            fullData[fullData.Length - 2] = high;
            fullData[fullData.Length - 1] = low;

            return new Message(fullData);
        }

        public static Message CreateReadCoilStatusCommand(SlaveAddress slaveAddress, CoilAddress firstCoil, ushort numberOfCoils)
        {
            var data = new List<byte>();
            WriteSlaveAddress(data, slaveAddress);
            WriteFunctionCode(data, FunctionCode.ReadCoilStatus);
            WriteCoilAddress(data, firstCoil);
            WriteBigEndianUInt16(data, numberOfCoils);

            return CreateFromRaw(data);
        }

        public static Message CreateReadInputStatusCommand(SlaveAddress slaveAddress, InputAddress firstInput, ushort numberOfInputs)
        {
            var data = new List<byte>();
            WriteSlaveAddress(data, slaveAddress);
            WriteFunctionCode(data, FunctionCode.ReadInputStatus);
            WriteInputAddress(data, firstInput);
            WriteBigEndianUInt16(data, numberOfInputs);

            return CreateFromRaw(data);
        }

        public static Message CreateReadHoldingRegistersCommand(SlaveAddress slaveAddress, HoldingRegisterAddress firstRegister, ushort numberOfRegisters)
        {
            var data = new List<byte>();
            WriteSlaveAddress(data, slaveAddress);
            WriteFunctionCode(data, FunctionCode.ReadHoldingRegisters);
            WriteHoldingRegisterAddress(data, firstRegister);
            WriteBigEndianUInt16(data, numberOfRegisters);

            return CreateFromRaw(data);
        }

        public static Message CreateReadInputRegistersCommand(SlaveAddress slaveAddress, InputRegisterAddress firstRegister, ushort numberOfRegisters)
        {
            var data = new List<byte>();
            WriteSlaveAddress(data, slaveAddress);
            WriteFunctionCode(data, FunctionCode.ReadInputRegisters);
            WriteInputRegisterAddress(data, firstRegister);
            WriteBigEndianUInt16(data, numberOfRegisters);

            return CreateFromRaw(data);
        }

        public static Message CreateForceSingleCoilCommand(SlaveAddress slaveAddress, CoilAddress coil, bool desiredState)
        {
            var data = new List<byte>();
            WriteSlaveAddress(data, slaveAddress);
            WriteFunctionCode(data, FunctionCode.ForceSingleCoil);
            WriteCoilAddress(data, coil);

            const byte On = 0xff;
            const byte Off = 0x00;

            data.Add(desiredState ? On : Off);
            data.Add(0x00);

            return CreateFromRaw(data);
        }

        public static Message CreatePresetSingleRegisterCommand(SlaveAddress slaveAddress, HoldingRegisterAddress register, ushort desiredValue)
        {
            var data = new List<byte>();
            WriteSlaveAddress(data, slaveAddress);
            WriteFunctionCode(data, FunctionCode.PresetSingleRegister);
            WriteHoldingRegisterAddress(data, register);
            WriteBigEndianUInt16(data, desiredValue);

            return CreateFromRaw(data);
        }

        public static Message CreateForceMultipleCoilsCommand(SlaveAddress slaveAddress, CoilAddress firstCoil, bool[] desiredStates)
        {
            if (desiredStates == null) throw new ArgumentNullException();
            if (desiredStates.Length == 0) throw new ArgumentException("Must write at least one coil.");
            if (desiredStates.Length > MaxBooleanArrayLength) throw new ArgumentException("Too many coils.");

            var data = new List<byte>();
            WriteSlaveAddress(data, slaveAddress);
            WriteFunctionCode(data, FunctionCode.ForceSingleCoil);
            WriteCoilAddress(data, firstCoil);
            WriteBigEndianUInt16(data, (ushort)desiredStates.Length);
            WriteLengthPrefixedPackedBooleanArray(data, desiredStates);

            return CreateFromRaw(data);
        }

        public static Message CreatePresetMultipleRegistersCommand(SlaveAddress slaveAddress, HoldingRegisterAddress firstRegister, ushort[] desiredValues)
        {
            if (desiredValues == null) throw new ArgumentNullException();
            if (desiredValues.Length == 0) throw new ArgumentException("Must write at least one register.");
            if (desiredValues.Length > MaxUShortArrayLength) throw new ArgumentException("Too many registers.");

            var data = new List<byte>();
            WriteSlaveAddress(data, slaveAddress);
            WriteFunctionCode(data, FunctionCode.PresetMultipleRegisters);
            WriteHoldingRegisterAddress(data, firstRegister);
            WriteBigEndianUInt16(data, (ushort)desiredValues.Length);
            WriteLengthPrefixedUShortArray(data, desiredValues);

            return CreateFromRaw(data);
        }

        private static void WriteSlaveAddress(List<byte> buffer, SlaveAddress address)
        {
            buffer.Add(address.WireValue);
        }

        private static void WriteFunctionCode(List<byte> buffer, FunctionCode functionCode)
        {
            buffer.Add((byte)functionCode);
        }

        private static void WriteBigEndianUInt16(List<byte> buffer, ushort value)
        {
            var high = (byte)(value >> 8);
            var low = (byte)(value & 0xff);

            buffer.Add(high);
            buffer.Add(low);
        }

        private static void WriteCoilAddress(List<byte> buffer, CoilAddress address)
        {
            WriteBigEndianUInt16(buffer, address.WireValue);
        }

        private static void WriteInputAddress(List<byte> buffer, InputAddress address)
        {
            WriteBigEndianUInt16(buffer, address.WireValue);
        }

        private static void WriteHoldingRegisterAddress(List<byte> buffer, HoldingRegisterAddress address)
        {
            WriteBigEndianUInt16(buffer, address.WireValue);
        }

        private static void WriteInputRegisterAddress(List<byte> buffer, InputRegisterAddress address)
        {
            WriteBigEndianUInt16(buffer, address.WireValue);
        }

        private const int MaxBooleanArrayLength = 255 * 8;

        private static void WriteLengthPrefixedPackedBooleanArray(List<byte> buffer, bool[] values)
        {
            if (values == null) throw new ArgumentNullException();
            if (values.Length > MaxBooleanArrayLength) throw new ArgumentException();

            var length = values.Length;
            var lengthInBytes = (byte)(length / 8);

            buffer.Add(lengthInBytes);
            for(int i = 0; i < lengthInBytes; i++)
            {
                byte valueByte = 0;
                for(int j = 0; j < 8; j++)
                {
                    var idx = 8 * i + j;
                    if (idx >= length) continue;
                    if (!values[idx]) continue;

                    valueByte |= (byte)(1 << j);
                }
                buffer.Add(valueByte);
            }
        }

        private const int MaxUShortArrayLength = 254 / 2;

        private static void WriteLengthPrefixedUShortArray(List<byte> buffer, ushort[] values)
        {

        }

        public SlaveAddress SlaveAddress
        {
            get
            {
                return new SlaveAddress(_data[0]);
            }
        }

        public FunctionCode FunctionCode
        {
            get
            {
                return (FunctionCode)_data[1];
            }
        }

        public bool IsCRCValid
        {
            get
            {
                return EncodedCRC == CalculatedCRC;
            }
        }

        public ushort EncodedCRC
        {
            get
            {
                return BitConverter.ToUInt16(_data, _data.Length - 2);
            }
        }

        private static ushort CalculateCRC(byte[] data, int length)
        {
            if (data == null) throw new ArgumentNullException();
            if (length < 0 || length > data.Length) throw new ArgumentException();

            ushort crc = 0xFFFF;

            for (int i = 0; i < length; i++)
            {
                crc ^= data[i];          // XOR byte into least sig. byte of crc

                for (int j = 8; j > 0; j--)
                {    // Loop over each bit
                    if ((crc & 0x0001) != 0)
                    {      // If the LSB is set
                        crc >>= 1;                    // Shift right and XOR 0xA001
                        crc ^= 0xA001;
                    }
                    else                            // Else LSB is not set
                        crc >>= 1;                    // Just shift right
                }
            }

            // At this point, result has low and high bytes swapped
            var low = crc >> 8;
            var high = crc & 0xff;

            ushort result = (ushort)((high << 8) | low);

            return result; 
        }

        public ushort CalculatedCRC
        {
            get
            {
                return CalculateCRC(_data, _data.Length - 2);
            }
        }

        public void WriteAscii(TextWriter writer)
        {
            for(int i = 0; i < _data.Length; i++)
            {
                var b = _data[i];
                writer.Write(_data[i].ToString("X2"));
            }
        }

        public void WriteAscii(Stream stream)
        {
            using (var writer = new StreamWriter(stream, Encoding.ASCII))
            {
                WriteAscii(writer);
            }
        }

        public void WriteRtu(Stream stream)
        {
            stream.Write(_data, 0, _data.Length);
        }
    }
}
