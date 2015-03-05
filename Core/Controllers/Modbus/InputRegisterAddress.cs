using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutomationLibrary.Controllers.Modbus
{
    public struct InputRegisterAddress
        : IEquatable<InputRegisterAddress>
    {
        private const UInt16 FirstFormattedAddress = 30001;

        private readonly UInt16 _wireValue;

        public InputRegisterAddress(UInt16 wireValue)
        {
            _wireValue = wireValue;
        }

        internal ushort WireValue { get { return _wireValue; } }

        public bool Equals(InputRegisterAddress other)
        {
            return _wireValue == other._wireValue;
        }

        public override bool Equals(object obj)
        {
            if (obj is InputRegisterAddress) return Equals((InputRegisterAddress)obj);
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

        public static implicit operator DatumAddress(InputRegisterAddress address)
        {
            return new DatumAddress(DatumAddressKind.InputRegister, address._wireValue);
        }
    }
}
