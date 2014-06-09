using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutomationLibrary.Mathematics
{
    [Serializable]
    public class IllConditionedProblemException : Exception
    {
        public IllConditionedProblemException() { }
        public IllConditionedProblemException(string message) : base(message) { }
        public IllConditionedProblemException(string message, Exception inner) : base(message, inner) { }
        protected IllConditionedProblemException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
