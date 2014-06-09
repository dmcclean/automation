using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutomationLibrary.Concurrency;

namespace AutomationLibrary.Controllers
{
    public interface IDeclarationSpace<TName>
        where TName : IEquatable<TName>
    {
        // these operations throw NotSupportedException if the TValue type is unsupported

        IVariable<TValue> GetReadOnlyVariable<TValue>(TName name);
        IMutableVariable<TValue> GetVariable<TValue>(TName name);

        // TODO: make sure this works for both directions, both PLC managed / PC acquired and PC managed / PLC acquired
        IWaitHandle GetWaitHandle(TName name);

        IProducerChannel<TValue> GetProducerChannel<TValue>(TName name);
        IConsumerChannel<TValue> GetConsumerChannel<TValue>(TName name);

        ISignal<TValue> GetSignal<TValue>(TName name);
    }
}
