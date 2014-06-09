using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutomationLibrary.Controllers.Modbus
{
    public struct DatumAddress
        : IEquatable<DatumAddress>
    {
        private readonly DatumAddressKind _kind;
        private readonly UInt16 _wireValue;

        internal DatumAddress(DatumAddressKind kind, UInt16 wireValue)
        {
            _kind = kind;
            _wireValue = wireValue;
        }

        public bool Equals(DatumAddress other)
        {
            return _kind == (other._kind) && (_wireValue == other._wireValue);
        }

        public static explicit operator CoilAddress(DatumAddress address)
        {
            if (address._kind != DatumAddressKind.Coil) throw new InvalidCastException();

            return new CoilAddress(address._wireValue);
        }

        public static explicit operator InputAddress(DatumAddress address)
        {
            if (address._kind != DatumAddressKind.Input) throw new InvalidCastException();

            return new InputAddress(address._wireValue);
        }

        public static explicit operator InputRegisterAddress(DatumAddress address)
        {
            if (address._kind != DatumAddressKind.InputRegister) throw new InvalidCastException();

            return new InputRegisterAddress(address._wireValue);
        }

        public static explicit operator HoldingRegisterAddress(DatumAddress address)
        {
            if (address._kind != DatumAddressKind.HoldingRegister) throw new InvalidCastException();

            return new HoldingRegisterAddress(address._wireValue);
        }
    }
}
