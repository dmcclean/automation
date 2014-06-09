using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutomationLibrary.Controllers;

namespace MassBayEngineering.Interop.Beckhoff
{
    internal sealed class DeclarationSpace
        : IDeclarationSpace<string>
    {
        private readonly ControllerConnection _connection;

        public DeclarationSpace(ControllerConnection connection)
        {
            _connection = connection;
        }

        public IVariable<TValue> GetReadOnlyVariable<TValue>(string name)
        {
            var symbol = _connection.GetSymbol(name);

            return _connection.CreateVariable<TValue>(symbol, isWriteable: false);
        }

        public IMutableVariable<TValue> GetVariable<TValue>(string name)
        {
            var symbol = _connection.GetSymbol(name);

            return (IMutableVariable<TValue>)_connection.CreateVariable<TValue>(symbol, isWriteable: true);
        }

        public AutomationLibrary.Concurrency.IWaitHandle GetWaitHandle(string name)
        {
            throw new NotImplementedException();
        }

        public IProducerChannel<TValue> GetProducerChannel<TValue>(string name)
        {
            throw new NotImplementedException();
        }

        public IConsumerChannel<TValue> GetConsumerChannel<TValue>(string name)
        {
            throw new NotImplementedException();
        }

        public ISignal<TValue> GetSignal<TValue>(string name)
        {
            var buffersSymbol = _connection.GetSymbol(name + "_Buffers");

            IVariable<TValue[]>[] bufferVariables = new IVariable<TValue[]>[buffersSymbol.SubSymbolCount];
            for (int i = 0; i < bufferVariables.Length; i++)
            {
                bufferVariables[i] = _connection.CreateVariable<TValue[]>(buffersSymbol.SubSymbols[i], isWriteable: false);
            }

            var clientRequestsBuffer = GetVariable<bool>(name + "_Signal.AdsClientRequestsBuffer");
            var clientReleasesBuffer = GetVariable<bool>(name + "_Signal.AdsClientReleasesBuffer");
            var indexControlledByClient = GetReadOnlyVariable<sbyte>(name + "_Signal.BufferControlledByAdsClient");

            return new Signal<TValue>(clientRequestsBuffer, clientReleasesBuffer, indexControlledByClient, bufferVariables);
        }
    }
}
