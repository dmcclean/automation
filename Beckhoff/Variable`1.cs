using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutomationLibrary.Controllers;
using TwinCAT.Ads;
using AutomationLibrary.Concurrency;

namespace MassBayEngineering.Interop.Beckhoff
{
    internal sealed class Variable<TValue>
        : IMutableVariable<TValue>
    {
        private readonly ControllerConnection _connection;
        private readonly TcAdsSymbolInfo _symbol;
        private readonly int _handle;
        private readonly bool _isWriteable;
        private readonly int[] _arrayDimensions;

        public Variable(ControllerConnection connection, TcAdsSymbolInfo symbol, int handle, bool isWriteable)
            : this(connection, symbol, handle, isWriteable, null)
        {
        }

        public Variable(ControllerConnection connection, TcAdsSymbolInfo symbol, int handle, bool isWriteable, int[] arrayDimensions)
        {
            _connection = connection;
            _symbol = symbol;
            _handle = handle;
            _isWriteable = isWriteable;
            _arrayDimensions = arrayDimensions;
        }

        /*
        internal Variable<TSubValue> GetArrayElement<TSubValue>(int index)
        {
            if (index < 0) throw new ArgumentException();
            var fullName = string.Format("{0}[{1}]", _symbol.Name, index);

            var symbol = _connection.GetSymbol(fullName);

            return _connection.CreateVariable<TSubValue>(symbol, _isWriteable);
        }

        internal Variable<TSubValue> GetStructureField<TSubValue>(string field)
        {
            var fullName = string.Format("{0}.{1}", _symbol.Name, field);

            var symbol = _connection.GetSymbol(fullName);

            return _connection.CreateVariable<TSubValue>(symbol, _isWriteable);
        }
         */

        public TValue Read()
        {
            try
            {
                if (_arrayDimensions != null)
                {
                    return _connection.ReadVariable<TValue>(_handle, _arrayDimensions);
                }
                else return _connection.ReadVariable<TValue>(_handle);
            }
            catch(AdsException ex)
            {
                throw new ControllerCommunicationException("Unable to read variable due to issues with controller connection.", ex);
            }
        }

        public void SynchronousWrite(TValue value)
        {
            if (!_isWriteable) throw new InvalidOperationException("An attempt was made to write to a read-only variable.");
            _connection.WriteVariable(_handle, value);
        }

        public ILatch DeferredWrite(TValue value)
        {
            SynchronousWrite(value);
            return Latch.AlreadyCompleted;
        }
    }
}
