using System;
using System.Reflection;
using System.Windows.Forms.VisualStyles;
using FujiXerox.RangerClient.Attributes;

namespace FujiXerox.RangerClient.Helpers
{
    public static class AttributeHelper
    {
        public static string GetSectionNameFromAttribute(Type type)
        {
            var attr = (SectionNameAttribute)Attribute.GetCustomAttribute(type, typeof(SectionNameAttribute));
            return attr == null ? type.Name : attr.GetName();
        }

        public static string GetValueNameFromAttribute(Type type, string propertyName)
        {
            var props = type.GetProperty(propertyName);
            var attr = (ValueNameAttribute) props.GetCustomAttribute(typeof (ValueNameAttribute));
            return attr == null ? propertyName : attr.GetName();
        }

        public static void UpdatePropertyValueFromAttribute<T>(T item, PropertyInfo property, Func<string, string, object> func)
        {
            var sectionName = GetSectionNameFromAttribute(typeof(T));
            var valueName = GetValueNameFromAttribute(typeof(T), property.Name);
            var result = func.Invoke(sectionName, valueName);
            property.SetValue(item, result);
        }

        public static void UpdatePropertyValueFromAttribute<T>(T item, Func<string, string, object> func)
        {
            var props = typeof (T).GetProperties();
            var sectionName = GetSectionNameFromAttribute(typeof(T));
            foreach (var property in props)
            {
                var valueName = GetValueNameFromAttribute(typeof(T), property.Name);
                var temp = (string) func.Invoke(sectionName, valueName);
                if (temp.ToLower() == "true" || temp.ToLower() == "false") property.SetValue(item, temp.ToLower() == "true");
                else property.SetValue(item, temp);
            }
        }
    }
}