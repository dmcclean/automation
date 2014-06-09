using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwinCAT.Ads;

namespace MassBayEngineering.Interop.Beckhoff
{
    public sealed class AdsMarshallingInfo<TValue>
        : AdsMarshallingInfo
    {
        private readonly AdsDatatypeId _adsTypeCode;
        private readonly string _adsTypeName;
        private readonly Type _managedType;
        private readonly Type _adsType;
        private readonly Delegate _readDelegate;
        private readonly Delegate _writeDelegate;

        private AdsMarshallingInfo(Type adsType, AdsDatatypeId adsTypeCode, string adsTypeName, Delegate readDelegate, Delegate writeDelegate)
        {
            _adsType = adsType;
            _managedType = typeof(TValue);
            _adsTypeCode = adsTypeCode;
            _adsTypeName = adsTypeName;
            _readDelegate = readDelegate;
            _writeDelegate = writeDelegate;
        }

        public override bool IsSimple
        {
            get
            {
                return _adsType == _managedType && _readDelegate == null && _writeDelegate == null;
            }
        }

        public override Type AdsType
        {
            get
            {
                return _adsType;
            }
        }

        public override AdsDatatypeId AdsTypeCode
        {
            get
            {
                return _adsTypeCode;
            }
        }


        public override string AdsTypeName
        {
            get
            {
                return _adsTypeName;
            }
        }

        public override bool IsCompatible(TcAdsSymbolInfo symbol)
        {
            // TODO: check much more thoroughly
            return (symbol.Datatype == AdsTypeCode);
        }

        internal Delegate ReadDelegate
        {
            get
            {
                return _readDelegate;
            }
        }

        internal Delegate WriteDelegate
        {
            get
            {
                return _writeDelegate;
            }
        }

        internal static AdsMarshallingInfo<TValue> CreateSimple(AdsDatatypeId adsTypeCode, string adsTypeName)
        {
            return new AdsMarshallingInfo<TValue>(typeof(TValue), adsTypeCode, adsTypeName, null, null);
        }

        internal static AdsMarshallingInfo<TValue> CreateWithCast<TAdsValue>(AdsDatatypeId adsTypeCode, string adsTypeName)
        {
            return new AdsMarshallingInfo<TValue>(typeof(TAdsValue), adsTypeCode, adsTypeName, (Func<TAdsValue, TValue>)(x => (TValue)(object)x), (Func<TValue,TAdsValue>)(x => (TAdsValue)(object)x));
        }

        internal static AdsMarshallingInfo<TValue> Create<TAdsValue>(AdsDatatypeId adsTypeCode, string adsTypeName, Func<TAdsValue, TValue> read, Func<TValue, TAdsValue> write)
        {
            return new AdsMarshallingInfo<TValue>(typeof(TAdsValue), adsTypeCode, adsTypeName, read, write);
        }
    }
}
