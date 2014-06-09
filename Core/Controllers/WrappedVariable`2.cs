using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutomationLibrary.Controllers
{
    public class WrappedVariable<TValue, TMarshalledValue>
        : IVariable<TValue>
    {
        private readonly IVariable<TMarshalledValue> _variable;
        private readonly Func<TMarshalledValue, TValue> _read;

        public WrappedVariable(IVariable<TMarshalledValue> variable, Func<TMarshalledValue, TValue> read)
        {
            if (variable == null) throw new ArgumentNullException();
            if (read == null) throw new ArgumentNullException();

            _variable = variable;
            _read = read;
        }

        public TValue Read()
        {
            var marshalledValue = _variable.Read();
            var result = _read(marshalledValue);

            return result;
        }
    }
}
