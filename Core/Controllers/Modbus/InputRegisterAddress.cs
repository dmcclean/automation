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
        private const UInt16 LastFormattedAddress  = 39999;
        private const int FirstExtendedAddress = 300001;
        private const int LastExtendedAddress  = 365536;

        private readonly UInt16 _wireValue;

        public static InputRegisterAddress FromInteger(int value)
        {
            if (1 <= value && value <= 65536) return new InputRegisterAddress((ushort)(value - 1));
            else throw new ArgumentOutOfRangeException();
        }

        public static InputRegisterAddress FromRawValue(ushort value)
        {
            return new InputRegisterAddress(value);
        }

        public static InputRegisterAddress FromModbus984(string value)
        {
            int numericValue;
            if (!int.TryParse(value, out numericValue)) throw new ArgumentException();
            else if (value.Length == 6)
            {
                // extended addressing
                if (FirstExtendedAddress <= numericValue && numericValue <= LastExtendedAddress) return new InputRegisterAddress((ushort)(numericValue - FirstExtendedAddress));
                else throw new ArgumentOutOfRangeException();
            }
            else
            {
                // ordinary addressing
                if (FirstFormattedAddress <= numericValue && numericValue <= LastFormattedAddress) return new InputRegisterAddress((ushort)(numericValue - FirstFormattedAddress));
                else throw new ArgumentOutOfRangeException();
            }
        }

        internal InputRegisterAddress(UInt16 wireValue)
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
