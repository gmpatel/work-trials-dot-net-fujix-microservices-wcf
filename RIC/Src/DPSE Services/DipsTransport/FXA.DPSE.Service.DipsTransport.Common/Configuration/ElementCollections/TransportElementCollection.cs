using System.Configuration;
using FXA.DPSE.Service.DipsTransport.Common.Configuration.Elements;

namespace FXA.DPSE.Service.DipsTransport.Common.Configuration.ElementCollections
{
    [ConfigurationCollection(typeof(TransportElement))]
    public class TransportElementCollection : ConfigurationElementCollection
    {
        public override ConfigurationElementCollectionType CollectionType
        {
            get { return ConfigurationElementCollectionType.BasicMap; }
        }

        protected override string ElementName
        {
            get { return "transport"; }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new TransportElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((TransportElement)element).Name;
        }

        public TransportElement this[string url]
        {
            get { return ((TransportElement)base.BaseGet(url)); }
        }
    }
}