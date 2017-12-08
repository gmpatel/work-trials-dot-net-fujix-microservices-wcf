using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FXA.DPSE.MessageDispatcher.DipsTransport.Configuration.Elements;

namespace FXA.DPSE.MessageDispatcher.DipsTransport.Configuration.ElementCollections
{
    [ConfigurationCollection(typeof(EndPointElement))]
    public class EodTransportEndPointElementCollection : ConfigurationElementCollection
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