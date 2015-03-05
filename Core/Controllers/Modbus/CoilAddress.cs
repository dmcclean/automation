using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutomationLibrary.Controllers.Modbus
{
    public struct CoilAddress
        : IEquatable<CoilAddress>
    {
        private const UInt16 FirstFormattedAddress = 00001;
        private const UInt16 LastFormattedAddress  = 09999;
        private const int FirstExtendedAddress = 000001;
        private const int LastExtendedAddress  = 065536;

        private readonly UInt16 _wireValue;

        public static CoilAddress FromInteger(int value)
        {
            if (1 <= value && value <= 65536) return new CoilAddress((ushort)(value - 1));
            else throw new ArgumentOutOfRangeException();
        }

        public static CoilAddress FromRawValue(ushort value)
        {
            return new CoilAddress(value);
        }

        public static CoilAddress FromModbus984(string value)
        {
            int numericValue;
            if(!int.TryParse(value, out numericValue)) throw new ArgumentException();
            else if (value.Length == 6)
            {
                // extended addressing
                if(FirstExtendedAddress <= numericValue && numericValue <= LastExtendedAddress) return new CoilAddress((ushort)(numericValue - FirstExtendedAddress));
                else throw new ArgumentOutOfRangeException();
            }
            else
            {
                // ordinary addressing
                if (FirstFormattedAddress <= numericValue && numericValue <= LastFormattedAddress) return new CoilAddress((ushort)(numericValue - FirstFormattedAddress));
                else throw new ArgumentOutOfRangeException();
            }
        }

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
