using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwinCAT.Ads;

namespace MassBayEngineering.Interop.Beckhoff
{
    public abstract class AdsMarshallingInfo
    {
        internal AdsMarshallingInfo()
        {
        }

        public abstract bool IsSimple { get; }
        public abstract Type AdsType { get; }
        public abstract AdsDatatypeId AdsTypeCode { get; }
        public abstract string AdsTypeName { get; }
        public abstract bool IsCompatible(TcAdsSymbolInfo symbol);
    }
}
