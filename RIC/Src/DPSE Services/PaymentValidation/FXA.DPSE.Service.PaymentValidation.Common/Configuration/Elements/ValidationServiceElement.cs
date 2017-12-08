using System;
using System.Configuration;

namespace FXA.DPSE.Service.PaymentValidation.Common.Configuration.Elements
{
    [ConfigurationCollection(typeof(ValidationServiceElement))]
    public class ValidationServiceElementCollection : ConfigurationElementCollection
    {
        public override ConfigurationElementCollectionType CollectionType
        {
            get { return ConfigurationElementCollectionType.BasicMap; }
        }

        protected override string ElementName
        {
            get { return "validationService"; }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new ValidationServiceElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ValidationServiceElement)element).ServiceName;
        }

        public ValidationServiceElement this[string name]
        {
            get { return ((ValidationServiceElement)base.BaseGet(name)); }
        }
    }

    public class ValidationServiceElement : ConfigurationElement
    {
        [ConfigurationProperty("name", DefaultValue = "", IsKey = true, IsRequired = true)]
        public string ServiceName
        {
            get { return (string)base["name"]; }
            set { base["name"] = value; }
        }

        [ConfigurationProperty("endpoint", DefaultValue = "", IsKey = true, IsRequired = true)]
        public string ServiceEndpoint
        {
            get { return (string)base["endpoint"]; }
            set { base["endpoint"] = value; }
        }

        [ConfigurationProperty("order", DefaultValue = "1", IsKey = true, IsRequired = true)]
        public int SequenceOrder
        {
            get { return (int)base["order"]; }
            set { base["order"] = value; }
        }
    }
}
