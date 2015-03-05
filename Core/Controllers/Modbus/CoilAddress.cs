using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutomationLibrary.Controllers.Modbus
{
    public struct CoilAddress
        : IEquatable<CoilAddress>
    {
        private const UInt16 FirstFormattedAddress = 1;

        private readonly UInt16 _wireValue;

        internal CoilAddress(UInt16 wireValue)
        {
            _wireValue = wireValue;
        }

        internal ushort WireValue { get { return _wireValue; } }

        public bool Equals(CoilAddress other)
        {
            return _wireValue == other._wireValue;
        }

        public override bool Equals(object obj)
        {
            if (obj is CoilAddress) return Equals((CoilAddress)obj);
            else return false;
        }

        public override int GetHashCode()
        {
            return _wireValue.GetHashCode();
        }

        public override string ToString()
        {
            return (FirstFormattedAddress + _wireValue).ToString("D");
        }

        public static implicit operator DatumAddress(CoilAddress address)
        {
            return new DatumAddress(DatumAddressKind.Coil, address._wireValue);
        }
    }
}
