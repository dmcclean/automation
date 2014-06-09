using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutomationLibrary.Controllers.Modbus
{
    public struct HoldingRegisterAddress
        : IEquatable<HoldingRegisterAddress>
    {
        private const UInt16 FirstFormattedAddress = 30001;

        private readonly UInt16 _wireValue;

        internal HoldingRegisterAddress(UInt16 wireValue)
        {
            _wireValue = wireValue;
        }

        public bool Equals(HoldingRegisterAddress other)
        {
            return _wireValue == other._wireValue;
        }

        public override bool Equals(object obj)
        {
            if (obj is HoldingRegisterAddress) return Equals((HoldingRegisterAddress)obj);
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

        public static implicit operator DatumAddress(HoldingRegisterAddress address)
        {
            return new DatumAddress(DatumAddressKind.HoldingRegister, address._wireValue);
        }
    }
}
