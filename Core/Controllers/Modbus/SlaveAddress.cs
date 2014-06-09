using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutomationLibrary.Controllers.Modbus
{
    public struct SlaveAddress
        : IEquatable<SlaveAddress>
    {
        private const byte BroadcastAddress = 0;
        private const byte MaximumValidAddress = 247;

        private readonly byte _value;

        public SlaveAddress(byte value)
        {
            if (value == BroadcastAddress) throw new ArgumentException();
            else if (value > MaximumValidAddress) throw new ArgumentException();

            _value = value;
        }

        public static readonly SlaveAddress Broadcast = new SlaveAddress();

        public bool Equals(SlaveAddress other)
        {
            return _value == other._value;
        }

        public override bool Equals(object obj)
        {
            if (obj is SlaveAddress) return Equals((SlaveAddress)obj);
            else return false;
        }

        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }

        public override string ToString()
        {
            return _value.ToString("D3");
        }
    }
}
