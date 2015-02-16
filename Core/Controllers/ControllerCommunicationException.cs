using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutomationLibrary.Controllers
{
    [Serializable]
    public class ControllerCommunicationException : Exception
    {
        public ControllerCommunicationException() { }
        public ControllerCommunicationException(string message) : base(message) { }
        public ControllerCommunicationException(string message, Exception inner) : base(message, inner) { }
        protected ControllerCommunicationException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
