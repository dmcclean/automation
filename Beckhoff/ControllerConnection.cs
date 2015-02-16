using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutomationLibrary.Controllers;
using TwinCAT.Ads;

namespace MassBayEngineering.Interop.Beckhoff
{
    public sealed class ControllerConnection
        : IControllerConnection<string>
    {
        private readonly TcAdsClient _client;
        private readonly DeclarationSpace _declarationSpace;
        private readonly TcAdsSymbolInfoLoader _infoLoader;
        private readonly TcAdsSymbolInfoCollection _symbols;

        static ControllerConnection()
        {
            _variableConstructorFormalParameters = new Type[] { typeof(ControllerConnection), typeof(TcAdsSymbolInfo), typeof(int), typeof(bool) };
        }

        internal ControllerConnection(TcAdsClient client)
        {
            _client = client;
            _declarationSpace = new DeclarationSpace(this);
            _infoLoader = _client.CreateSymbolInfoLoader();
            _infoLoader.GetFirstSymbol(true);
            _symbols = _infoLoader.GetSymbols(forceReload: true);
        }

        internal TcAdsSymbolInfo GetSymbol(string name)
        {
            var result = GetSymbolFrom(_symbols, name);
            if (result == null)
                throw new ArgumentException(string.Format("Unable to find symbol {0}.", name));
            else return result;
        }

        internal TcAdsSymbolInfo GetSymbolFrom(TcAdsSymbolInfoCollection symbols, string name)
        {
            foreach(var subname in GetPrefixes(name)) 
            {
                var sym = symbols.Cast<TcAdsSymbolInfo>().Where(s => s.ShortName == subname).SingleOrDefault();

                if (sym != null) 
                {
                    if (name == subname) return sym;
                    else
                    {
                        var resiudalName = name.Substring(subname.Length + 1);
                        return GetSymbolFrom(sym.SubSymbols, resiudalName);
                    }
                }
            }

            return null;
        }

        internal IEnumerable<string> GetPrefixes(string name)
        {
            string remaining = name;
            while(remaining != null)
            {
                yield return remaining;
                remaining = StripEndOfSymbolName(remaining);
            }   
        }
        internal string StripEndOfSymbolName(string name)
        {
            var idx = name.LastIndexOf('.');
            if (idx <= 0) return null;
            else return name.Substring(0, idx);
        }

        internal int CreateVariableHandle(TcAdsSymbolInfo symbol)
        {
            return _client.CreateVariableHandle(symbol.Name);
        }

        public IDeclarationSpace<string> RootNamespace
        {
            get { return _declarationSpace; }
        }

        internal TValue ReadVariable<TValue>(int handle)
        {
            var result = _client.ReadAny(handle, typeof(TValue));
            if (result is TValue) return (TValue)result;
            else throw new Exception(); // TODO: correct exception
        }

        internal TValue ReadVariable<TValue>(int handle, int[] arrayDimensions)
        {
            var result = _client.ReadAny(handle, typeof(TValue), arrayDimensions);
            if (result is TValue) return (TValue)result;
            else throw new Exception(); // TODO: correct exception
        }

        internal void WriteVariable<TValue>(int handle, TValue value)
        {
            _client.WriteAny(handle, value);
        }

        private static readonly Type[] _variableConstructorFormalParameters;

        internal IVariable<TValue> CreateVariable<TValue>(TcAdsSymbolInfo symbol, bool isWriteable)
        {
            if (symbol == null) throw new ArgumentNullException();
            var marshalling = Marshaller.GetMarshallingInfo<TValue>();

            if (marshalling == null || !marshalling.IsCompatible(symbol))
            {
                throw new ApplicationException("Incompatible variable type.");
            }

            var handle = CreateVariableHandle(symbol);

            if (typeof(TValue).IsArray)
            {
                if (marshalling.IsSimple)
                {
                    int arraySize = symbol.SubSymbolCount;

                    return new Variable<TValue>(this, symbol, handle, isWriteable, new int[] { arraySize });
                }
                else throw new NotSupportedException();
            }

            if (marshalling.IsSimple)
            {
                return new Variable<TValue>(this, symbol, handle, isWriteable);
            }
            else
            {
                Type rawVariableType = typeof(Variable<>).MakeGenericType(marshalling.AdsType);
                var constructor = rawVariableType.GetConstructor(_variableConstructorFormalParameters);

                var parameters = new object[] { this, symbol, handle, isWriteable };

                var rawVariable = constructor.Invoke(parameters);
                var resultVariable = rawVariable;

                if (marshalling.ReadDelegate != null)
                {
                    // we need to wrap
                    // construct the type of the wrapper
                    Type wrapperVariableType = typeof(WrappedMutableVariable<,>).MakeGenericType(typeof(TValue), marshalling.AdsType);

                    // construct the three argument types to the constructor
                    var wrappedVariableConstructorFormalParameters = new Type[] {
                        typeof(IMutableVariable<>).MakeGenericType(marshalling.AdsType),
                        typeof(Func<,>).MakeGenericType(marshalling.AdsType, typeof(TValue)),
                        typeof(Func<,>).MakeGenericType(typeof(TValue), marshalling.AdsType)
                    };

                    var wrappedVariableConstructor = wrapperVariableType.GetConstructor(wrappedVariableConstructorFormalParameters);

                    var wrapperParameters = new object[] { rawVariable, marshalling.ReadDelegate, marshalling.WriteDelegate };

                    resultVariable = wrappedVariableConstructor.Invoke(wrapperParameters);
                }

                return (IVariable<TValue>)resultVariable;
            }
        }
    }
}
