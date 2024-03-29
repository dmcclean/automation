﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutomationLibrary.Controllers
{
    public interface IControllerConnection<TName>
        : IDisposable
        where TName : IEquatable<TName>
    {
        IDeclarationSpace<TName> RootNamespace { get; }
    }
}
