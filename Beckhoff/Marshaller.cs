using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwinCAT.Ads;
using AutomationLibrary.Controllers;

namespace MassBayEngineering.Interop.Beckhoff
{
    public static class Marshaller
    {
        public static AdsMarshallingInfo<TValue> GetMarshallingInfo<TValue>()
        {
            var result = GetMarshallingInfoRaw<TValue>();
            return (AdsMarshallingInfo<TValue>)result;
        }

        internal static AdsMarshallingInfo GetMarshallingInfoRaw<TValue>()
        {
            var type = typeof(TValue);
            var typeCode = Type.GetTypeCode(type);
            if (type.IsEnum)
            {
                if (typeCode != TypeCode.UInt16 && typeCode != TypeCode.Int16) throw new NotSupportedException("Exception types must be 16 bit types to be mapped.");
                var adsName = PlcTypeNameAttribute.ReadTypeName(type);
                if (adsName != null)
                {
                    return AdsMarshallingInfo<TValue>.CreateWithCast<Int16>(AdsDatatypeId.ADST_INT16, adsName);
                }
            }

            switch (typeCode)
            {
                case TypeCode.Boolean:
                    return AdsMarshallingInfo<Boolean>.CreateSimple(AdsDatatypeId.ADST_BIT, "BOOL");
                case TypeCode.Single:
                    return AdsMarshallingInfo<Single>.CreateSimple(AdsDatatypeId.ADST_REAL32, "REAL");
                case TypeCode.Double:
                    return AdsMarshallingInfo<Double>.CreateSimple(AdsDatatypeId.ADST_REAL64, "LREAL");
                case TypeCode.Byte:
                    return AdsMarshallingInfo<Byte>.CreateSimple(AdsDatatypeId.ADST_UINT8, "BYTE");
                case TypeCode.SByte:
                    return AdsMarshallingInfo<SByte>.CreateSimple(AdsDatatypeId.ADST_INT8, "SINT");
                case TypeCode.Int16:
                    return AdsMarshallingInfo<Int16>.CreateSimple(AdsDatatypeId.ADST_INT16, "INT");
                case TypeCode.UInt16:
                    return AdsMarshallingInfo<UInt16>.CreateSimple(AdsDatatypeId.ADST_UINT16, "UINT");
                case TypeCode.Int32:
                    return AdsMarshallingInfo<Int32>.CreateSimple(AdsDatatypeId.ADST_INT32, "DINT");
                case TypeCode.UInt32:
                    return AdsMarshallingInfo<UInt32>.CreateSimple(AdsDatatypeId.ADST_UINT32, "UDINT");
                case TypeCode.Int64:
                    return AdsMarshallingInfo<Int64>.CreateSimple(AdsDatatypeId.ADST_INT64, "LINT");
                case TypeCode.UInt64:
                    return AdsMarshallingInfo<UInt64>.CreateSimple(AdsDatatypeId.ADST_UINT64, "ULINT");
                case TypeCode.Object:
                    break; // fall through and handle these later
                default:
                    throw new Exception("Unsupported type.");
            }

            if (type == typeof(TimeSpan))
            {
                return AdsMarshallingInfo<TimeSpan>.Create<UInt32>(AdsDatatypeId.ADST_BIGTYPE, "TIME", x => TimeSpan.FromMilliseconds(x), x => (uint)x.TotalMilliseconds);
            }

            if (type.IsValueType)
            {
                var adsName = PlcTypeNameAttribute.ReadTypeName(type);
                if (adsName != null)
                {
                    return AdsMarshallingInfo<TValue>.CreateSimple(AdsDatatypeId.ADST_BIGTYPE, adsName);
                }
            }

            if (type.IsArray)
            {
                var elementType = type.GetElementType();
                var thisMethod = typeof(Marshaller).GetMethod("GetMarshallingInfoRaw", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
                var thisMethodForElementType = thisMethod.MakeGenericMethod(elementType);

                var elementMarshalling = (AdsMarshallingInfo)thisMethodForElementType.Invoke(null, null);

                if (elementMarshalling.IsSimple)
                {
                    var resultType = typeof(AdsMarshallingInfo<>).MakeGenericType(type);
                    var createMethod = resultType.GetMethod("CreateSimple", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);

                    var arguments = new object[] { elementMarshalling.AdsTypeCode, string.Format("ARRAY OF {0}", elementMarshalling.AdsTypeName) };

                    return (AdsMarshallingInfo)createMethod.Invoke(null, arguments);
                }
                else
                {
                    throw new Exception("Unsupported array type.");
                }
            }

            throw new Exception("Unsupported type.");
        }

        public static Tuple<AdsDatatypeId, String> MapToAdsType(Type type)
        {
            var typeCode = Type.GetTypeCode(type);
            if (type.IsEnum)
            {
                if (typeCode != TypeCode.UInt16 && typeCode != TypeCode.Int16) throw new NotSupportedException("Exception types must be 16 bit types to be mapped.");
                var adsName = PlcTypeNameAttribute.ReadTypeName(type);
                if (adsName != null)
                {
                    return Tuple.Create(AdsDatatypeId.ADST_BIGTYPE, adsName.ToUpperInvariant());
                }
            }

            switch (typeCode)
            {
                case TypeCode.Boolean:
                    return Tuple.Create(AdsDatatypeId.ADST_BIT, "BOOL");
                case TypeCode.Single:
                    return Tuple.Create(AdsDatatypeId.ADST_REAL32, "REAL");
                case TypeCode.Double:
                    return Tuple.Create(AdsDatatypeId.ADST_REAL64, "LREAL");
                case TypeCode.Byte:
                    return Tuple.Create(AdsDatatypeId.ADST_UINT8, "BYTE");
                case TypeCode.SByte:
                    return Tuple.Create(AdsDatatypeId.ADST_INT8, "SINT");
                case TypeCode.Int16:
                    return Tuple.Create(AdsDatatypeId.ADST_INT16, "INT");
                case TypeCode.UInt16:
                    return Tuple.Create(AdsDatatypeId.ADST_UINT16, "UINT");
                case TypeCode.Int32:
                    return Tuple.Create(AdsDatatypeId.ADST_INT32, "DINT");
                case TypeCode.UInt32:
                    return Tuple.Create(AdsDatatypeId.ADST_UINT32, "UDINT");
                case TypeCode.Int64:
                    return Tuple.Create(AdsDatatypeId.ADST_INT64, "LINT");
                case TypeCode.UInt64:
                    return Tuple.Create(AdsDatatypeId.ADST_UINT64, "ULINT");
                case TypeCode.Object:
                    break; // fall through and handle these later
                default:
                    throw new Exception("Unsupported type.");
            }

            if (type == typeof(TimeSpan))
            {
                return Tuple.Create(AdsDatatypeId.ADST_BIGTYPE, "TIME");
            }

            if (type.IsValueType)
            {
                var adsName = PlcTypeNameAttribute.ReadTypeName(type);
                if (adsName != null)
                {
                    return Tuple.Create(AdsDatatypeId.ADST_BIGTYPE, adsName.ToUpperInvariant());
                }
            }

            throw new Exception("Unsupported type.");
        }
    }
}
