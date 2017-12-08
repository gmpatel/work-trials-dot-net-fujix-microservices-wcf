using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FXA.DPSE.Framework.Scheduler.Service.Configuration.Elements;

namespace FXA.DPSE.Framework.Scheduler.Service.Configuration.ElementCollections
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