using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutomationLibrary.Controllers.Modbus
{
    public struct InputAddress
        : IEquatable<InputAddress>
    {
        private const UInt16 FirstFormattedAddress = 10001;
        private const UInt16 LastFormattedAddress  = 19999;
        private const int FirstExtendedAddress = 100001;
        private const int LastExtendedAddress  = 165536;

        private readonly UInt16 _wireValue;

        public static InputAddress FromModbus984(string value)
        {
            int numericValue;
            if (!int.TryParse(value, out numericValue)) throw new ArgumentException();
            else if (value.Length == 6)
            {
                // extended addressing
                if (FirstExtendedAddress <= numericValue && numericValue <= LastExtendedAddress) return new InputAddress((ushort)(numericValue - FirstExtendedAddress));
                else throw new ArgumentOutOfRangeException();
            }
            else
            {
                // ordinary addressing
                if (FirstFormattedAddress <= numericValue && numericValue <= LastFormattedAddress) return new InputAddress((ushort)(numericValue - FirstFormattedAddress));
                else throw new ArgumentOutOfRangeException();
            }
        }

        internal InputAddress(UInt16 wireValue)
        {
            _wireValue = wireValue;
        }

        internal ushort WireValue { get { return _wireValue; } }

        public bool Equals(InputAddress other)
        {
            return _wireValue == other._wireValue;
        }

        public override bool Equals(object obj)
        {
            if (obj is InputAddress) return Equals((InputAddress)obj);
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

        public static implicit operator DatumAddress(InputAddress address)
        {
            return new DatumAddress(DatumAddressKind.Input, address._wireValue);
        }
    }
}
