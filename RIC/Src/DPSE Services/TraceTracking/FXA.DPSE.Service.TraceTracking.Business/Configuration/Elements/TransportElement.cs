using System.Configuration;

namespace FXA.DPSE.Service.DipsTransport.Business.Configuration.Elements
{
    public class TransportElement : ConfigurationElement
    {
        [ConfigurationProperty("name", DefaultValue = null, IsKey = true, IsRequired = true)]
        public string Name
        {
            get { return (string)base["name"]; }
            set { base["name"] = value; }
        }

        [ConfigurationProperty("source", IsRequired = true)]
        public SourceElement Source
        {
            get { return ((SourceElement)(base["source"])); }
            set { base["source"] = value; }
        }

        [ConfigurationProperty("destination", IsRequired = true)]
        public DestinationElement Destination
        {
            get { return (DestinationElement)base["destination"]; }
            set { base["destination"] = value; }
        }

        [ConfigurationProperty("tempLocation", IsRequired = true)]
        public TempLocationElement TempLocation
        {
            get { return (TempLocationElement)base["tempLocation"]; }
            set { base["tempLocation"] = value; }
        }
    }
}