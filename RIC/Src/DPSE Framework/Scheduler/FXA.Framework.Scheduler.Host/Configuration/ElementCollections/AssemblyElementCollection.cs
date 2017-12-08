using System.Configuration;
using FXA.Framework.Scheduler.Host.Configuration.Elements;

namespace FXA.Framework.Scheduler.Host.Configuration.ElementCollections
{
    [ConfigurationCollection(typeof(AssemblyElement))]
    public class AssemblyElementCollection : ConfigurationElementCollection
    {
        public override ConfigurationElementCollectionType CollectionType
        {
            get { return ConfigurationElementCollectionType.BasicMap; }
        }

        protected override string ElementName
        {
            get { return "assembly"; }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new AssemblyElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((AssemblyElement)element).Name;
        }

        public AssemblyElement this[string name]
        {
            get { return ((AssemblyElement)base.BaseGet(name)); }
        }
    }
}