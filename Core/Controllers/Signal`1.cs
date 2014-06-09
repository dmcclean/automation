using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutomationLibrary.Collections;

namespace AutomationLibrary.Controllers
{
    public sealed class Signal<TValue>
        : ISignal<TValue>
    {
        private readonly IMutableVariable<bool> _clientRequestsBuffer;
        private readonly IMutableVariable<bool> _clientReleasesBuffer;
        private readonly IVariable<sbyte> _indexControlledByClient;
        private readonly IVariable<TValue[]>[] _bufferVariables;

        public Signal(IMutableVariable<bool> clientRequestsBuffer, IMutableVariable<bool> clientReleasesBuffer, IVariable<sbyte> indexControlledByClient, params IVariable<TValue[]>[] bufferVariables)
        {
            _clientRequestsBuffer = clientRequestsBuffer;
            _clientReleasesBuffer = clientReleasesBuffer;
            _indexControlledByClient = indexControlledByClient;
            _bufferVariables = bufferVariables;
        }

        public bool ReadAvailable(RingBuffer<TValue> buffer)
        {
            var index = _indexControlledByClient.Read();
            if (index < 0)
            {
                // we don't own a buffer, so we can end our request to release one
                _clientReleasesBuffer.SynchronousWrite(false);
                // we need to request a buffer
                _clientRequestsBuffer.SynchronousWrite(true);

                // we didn't read anything
                return false;
            }
            else
            {
                // our buffer request has been granted, so we can end the request
                _clientRequestsBuffer.SynchronousWrite(false);
                var values = _bufferVariables[index].Read();
                // we are finished reading our buffer, so we can release it
                _clientReleasesBuffer.SynchronousWrite(true);

                for (int i = 0; i < values.Length; i++)
                {
                    buffer.Add(values[i]); // TODO: have an AddRange method here
                }

                // we did read something
                return true;
            }
        }
    }
}
