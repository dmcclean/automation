using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutomationLibrary.Controllers
{
    public static class VariableExtensions
    {
        public static IVariable<TValue> Wrap<TValue, TMarshalledValue>(this IVariable<TMarshalledValue> handle, Func<TMarshalledValue, TValue> read)
        {
            return new WrappedVariable<TValue, TMarshalledValue>(handle, read);
        }

        public static IMutableVariable<TValue> Wrap<TValue, TMarshalledValue>(this IMutableVariable<TMarshalledValue> handle, Func<TMarshalledValue, TValue> read, Func<TValue, TMarshalledValue> write)
        {
            return new WrappedMutableVariable<TValue, TMarshalledValue>(handle, read, write);
        }
    }
}
