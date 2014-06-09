using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutomationLibrary.Concurrency;

namespace AutomationLibrary.Controllers
{
    public class WrappedMutableVariable<TValue, TMarshalledValue>
        : WrappedVariable<TValue, TMarshalledValue>,
          IMutableVariable<TValue>
    {
        private readonly IMutableVariable<TMarshalledValue> _variable;
        private readonly Func<TValue, TMarshalledValue> _write;

        public WrappedMutableVariable(IMutableVariable<TMarshalledValue> variable, Func<TMarshalledValue, TValue> read, Func<TValue, TMarshalledValue> write)
            : base(variable, read)
        {
            if (write == null) throw new ArgumentNullException();

            _variable = variable;
            _write = write;
        }

        public void SynchronousWrite(TValue value)
        {
            var marshalledValue = _write(value);

            _variable.SynchronousWrite(marshalledValue);
        }

        public ILatch DeferredWrite(TValue value)
        {
            var marshalledValue = _write(value);

            return _variable.DeferredWrite(marshalledValue);
        }
    }
}
