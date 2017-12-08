using System;

namespace FXA.DPSE.Framework.Service.WCF.Attributes.Logging
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class IgnoreLoggingAttribute : Attribute
    {
    }
}
