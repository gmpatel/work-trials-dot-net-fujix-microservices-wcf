using System.Configuration;

namespace FXA.DPSE.Service.PaymentValidation.Common
{
    public class RoutingSlipElement : ConfigurationElement
    {
        public int SequenceOrder { get; set; }
        public string ServiceName { get; set; }
        public string ServiceEndpoint { get; set; }
        public bool Enabled { get; set; }
    }

    [ConfigurationCollection(typeof(RoutingSlipElement))]
    public class AssemblyElementCollection : ConfigurationElementCollection
    {
        public override ConfigurationElementCollectionType CollectionType
        {
            get { return ConfigurationElementCollectionType.BasicMap; }
        }

        protected override string ElementName
        {
            get { return "routingSlip"; }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new RoutingSlipElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((RoutingSlipElement) element);
        }

        public RoutingSlipElement this[string name]
        {
            get { return ((RoutingSlipElement)base.BaseGet(name)); }
        }
    }
}
