using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutomationLibrary.Controllers
{
    public interface IDeferedTransferDeclarationSpace<TName>
        : IDeclarationSpace<TName>
        where TName : IEquatable<TName>
    {
        void ExchangeVariables(int millisecondsTimeout);

        // semantics: the variable will still exist, but won't participate in this transfer group anymore
        void RemoveVariable<TValue>(IVariable<TValue> variable);
    }
}
