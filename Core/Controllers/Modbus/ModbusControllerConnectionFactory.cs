using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using NModbus = global::Modbus;

namespace AutomationLibrary.Controllers.Modbus
{
    public sealed class ModbusControllerConnectionFactory
        : IControllerConnectionFactory<DatumAddress>
    {
        private readonly ModbusSerialProtocol _protocol;
        private readonly SlaveAddress _slave;
        private readonly string _portName;
        private readonly int _baudRate;
        private readonly Parity _parity;
        private readonly int _dataBits;
        private readonly StopBits _stopBits;

        public ModbusControllerConnectionFactory(ModbusSerialProtocol protocol, SlaveAddress slave, string portName, int baudRate, Parity parity, int dataBits, StopBits stopBits)
        {
            _protocol = protocol;
            _slave = slave;
            _portName = portName;
            _baudRate = baudRate;
            _parity = parity;
            _dataBits = dataBits;
            _stopBits = stopBits;
        }


        public IControllerConnection<DatumAddress> Connect(TimeSpan timeout)
        {
            var port = new SerialPort(_portName, _baudRate, _parity, _dataBits, _stopBits);
            try
            {
                port.ReadTimeout = (int)timeout.TotalMilliseconds;
                port.WriteTimeout = (int)timeout.TotalMilliseconds;

                port.Open();

                NModbus.Device.ModbusMaster master;
                switch (_protocol)
                {
                    case ModbusSerialProtocol.ASCII:
                        master = NModbus.Device.ModbusSerialMaster.CreateAscii(port);
                        break;
                    case ModbusSerialProtocol.RTU:
                        master = NModbus.Device.ModbusSerialMaster.CreateRtu(port);
                        break;
                    default:
                        throw new ApplicationException("Invalid modbus protocol.");
                }

                return new ModbusControllerConnection(master, _slave);
            }
            catch (Exception)
            {
                port.Dispose();
                throw;
            }
        }
    }
}
