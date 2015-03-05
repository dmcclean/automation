using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutomationLibrary.Controllers.Modbus
{
    public struct HoldingRegisterAddress
        : IEquatable<HoldingRegisterAddress>
    {
        private const UInt16 FirstFormattedAddress = 40001;
        private const UInt16 LastFormattedAddress  = 49999;
        private const int FirstExtendedAddress = 400001;
        private const int LastExtendedAddress  = 465536;

        public static HoldingRegisterAddress FromInteger(int value)
        {
            if (1 <= value && value <= 65536) return new HoldingRegisterAddress((ushort)(value - 1));
            else throw new ArgumentOutOfRangeException();
        }

        public static HoldingRegisterAddress FromRawValue(ushort value)
        {
            return new HoldingRegisterAddress(value);
        }

        public static HoldingRegisterAddress FromModbus984(string value)
        {
            int numericValue;
            if(!int.TryParse(value, out numericValue)) throw new ArgumentException();
            else if (value.Length == 6)
            {
                // extended addressing
                if(FirstExtendedAddress <= numericValue && numericValue <= LastExtendedAddress) return new HoldingRegisterAddress((ushort)(numericValue - FirstExtendedAddress));
                else throw new ArgumentOutOfRangeException();
            }
            else
            {
                // ordinary addressing
                if (FirstFormattedAddress <= numericValue && numericValue <= LastFormattedAddress) return new HoldingRegisterAddress((ushort)(numericValue - FirstFormattedAddress));
                else throw new ArgumentOutOfRangeException();
            }
        }

        private readonly UInt16 _wireValue;

        internal HoldingRegisterAddress(UInt16 wireValue)
        {
            _wireValue = wireValue;
        }

        internal ushort WireValue { get { return _wireValue; } }

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
