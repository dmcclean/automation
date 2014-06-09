using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutomationLibrary.Controllers
{
    public interface IVariable<TValue>
    {
        TValue Read();
    }
}
