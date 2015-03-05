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

        private readonly byte _wireValue;

        public SlaveAddress(byte value)
        {
            if (value == BroadcastAddress) throw new ArgumentException();
            else if (value > MaximumValidAddress) throw new ArgumentException();

            _wireValue = value;
        }

        public static readonly SlaveAddress Broadcast = new SlaveAddress();

        internal byte WireValue { get { return _wireValue;  } }

        public bool Equals(SlaveAddress other)
        {
            return _wireValue == other._wireValue;
        }

        public override bool Equals(object obj)
        {
            if (obj is SlaveAddress) return Equals((SlaveAddress)obj);
            else return false;
        }

        public override int GetHashCode()
        {
            return _wireValue.GetHashCode();
        }

        public override string ToString()
        {
            return _wireValue.ToString("D3");
        }

        public static bool operator==(SlaveAddress a, SlaveAddress b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(SlaveAddress a, SlaveAddress b)
        {
            return !(a == b);
        }
    }
}
