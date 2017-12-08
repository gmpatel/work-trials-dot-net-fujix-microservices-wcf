using System.Configuration;
using FXA.DPSE.Service.HealthMonitor.Configuration.Elements;

namespace FXA.DPSE.Service.HealthMonitor.Configuration.ElementCollections
{
    [ConfigurationCollection(typeof(EndPointElement))]
    public class EndPointElementCollection : ConfigurationElementCollection
    {
        public override ConfigurationElementCollectionType CollectionType
        {
            get { return ConfigurationElementCollectionType.BasicMap; }
        }

        protected override string ElementName
        {
            get { return "endPoint"; }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new EndPointElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((EndPointElement)element).Url;
        }

        public EndPointElement this[string url]
        {
            get { return ((EndPointElement)base.BaseGet(url)); }
        }
    }
}