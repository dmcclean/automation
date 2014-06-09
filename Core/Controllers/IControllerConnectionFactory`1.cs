using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutomationLibrary.Controllers
{
    public interface IControllerConnectionFactory<TName>
        where TName : IEquatable<TName>
    {
        IControllerConnection<TName> Connect(TimeSpan timeout);
    }
}
