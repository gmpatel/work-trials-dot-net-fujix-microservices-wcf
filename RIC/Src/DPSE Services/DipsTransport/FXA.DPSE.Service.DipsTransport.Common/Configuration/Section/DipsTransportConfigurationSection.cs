using System.Configuration;
using FXA.DPSE.Service.DipsTransport.Common.Configuration.ElementCollections;
using FXA.DPSE.Service.DipsTransport.Common.Configuration.Elements;

namespace FXA.DPSE.Service.DipsTransport.Common.Configuration.Section
{
    public class DipsTransportConfigurationSection : ConfigurationSection
    {
        [ConfigurationProperty("pgpPublicKeyFile")]
        public PgpPublicKeyFileElement PgpPublicKeyFile
        {
            get { return ((PgpPublicKeyFileElement)(base["pgpPublicKeyFile"])); }
            set { base["pgpPublicKeyFile"] = value; }
        }

        [ConfigurationProperty("transports")]
        public TransportElementCollection Transports
        {
            get { return ((TransportElementCollection)(base["transports"])); }
            set { base["transports"] = value; }
        }
    }
}