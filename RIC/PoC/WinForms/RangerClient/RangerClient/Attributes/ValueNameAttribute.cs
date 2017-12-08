using System;

namespace FujiXerox.RangerClient.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ValueNameAttribute : Attribute
    {
        private readonly string _resolvedName;

        public ValueNameAttribute(string name)
        {
            _resolvedName = name;
        }

        public string GetName()
        {
            return _resolvedName;
        }
    }
}