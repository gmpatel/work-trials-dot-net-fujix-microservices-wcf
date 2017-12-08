using System;

namespace FujiXerox.RangerClient.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class SectionNameAttribute : Attribute
    {
        private readonly string _resolvedName;

        public SectionNameAttribute(string name)
        {
            _resolvedName = name;
        }

        public string GetName()
        {
            return _resolvedName;
        }
    }
}