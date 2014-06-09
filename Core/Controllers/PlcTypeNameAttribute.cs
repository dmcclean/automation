using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutomationLibrary.Controllers
{
    [AttributeUsage(AttributeTargets.Struct | AttributeTargets.Enum, Inherited = true, AllowMultiple = false)]
    public sealed class PlcTypeNameAttribute
        : Attribute
    {
        private readonly string _typeName;

        public PlcTypeNameAttribute(string typeName)
        {
            _typeName = typeName;
        }

        public string TypeName
        {
            get { return _typeName; }
        }

        public static string ReadTypeName(Type type)
        {
            var attr = type.GetCustomAttributes(typeof(PlcTypeNameAttribute), true).OfType<PlcTypeNameAttribute>().SingleOrDefault();
            if (attr == null) return null;
            else return attr.TypeName;
        }
    }
}
