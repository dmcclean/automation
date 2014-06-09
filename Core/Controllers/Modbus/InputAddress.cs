﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutomationLibrary.Controllers.Modbus
{
    public struct InputAddress
        : IEquatable<InputAddress>
    {
        private const UInt16 FirstFormattedAddress = 10001;

        private readonly UInt16 _wireValue;

        public InputAddress(UInt16 wireValue)
        {
            _wireValue = wireValue;
        }

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