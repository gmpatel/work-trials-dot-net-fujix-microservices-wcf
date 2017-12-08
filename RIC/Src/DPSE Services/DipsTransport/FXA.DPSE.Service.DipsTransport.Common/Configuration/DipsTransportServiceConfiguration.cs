using System.Collections.Generic;
using System.Configuration;
using FXA.DPSE.Service.DipsTransport.Common.Configuration.Elements;
using FXA.DPSE.Service.DipsTransport.Common.Configuration.Section;

namespace FXA.DPSE.Service.DipsTransport.Common.Configuration
{
    public enum SourceTypes
    {
        Path,
        Sftp
    }

    public enum DestinationTypes
    {
        Path,
        Sftp
    }

    public enum TempLocationTypes
    {
        Path
    }

    public class DipsTransportServiceConfiguration : IDipsTransportServiceConfiguration
    {
        private readonly DipsTransportConfigurationSection _config;

        private IList<TransportElement> _transportElements;

        public DipsTransportServiceConfiguration()
        {
            _config = (DipsTransportConfigurationSection)ConfigurationManager.GetSection("serviceConfig");
        }

        public IList<TransportElement> Transports
        {
            get
            {
                if (_transportElements == null)
                {
                    _transportElements = new List<TransportElement>();

                    foreach (var transport in _config.Transports)
                    {
                        var trans = (TransportElement) transport;

                        //if (trans.Destination.Connection.Port == -1 &&  string.IsNullOrEmpty(trans.Destination.Connection.Server))
                        //    trans.Destination.Connection = null;
                     
                        //if (trans.Source.Connection.Port == -1 && string.IsNullOrEmpty(trans.Source.Connection.Server))
                        //    trans.Source.Connection = null;

                        _transportElements.Add(trans);
                    }
                }

                return _transportElements;    
            }
        }

        public PgpPublicKeyFileElement PgpPublicKeyFile
        {
            get { return _config.PgpPublicKeyFile; }
            set { _config.PgpPublicKeyFile = value; }
        }
    }
}