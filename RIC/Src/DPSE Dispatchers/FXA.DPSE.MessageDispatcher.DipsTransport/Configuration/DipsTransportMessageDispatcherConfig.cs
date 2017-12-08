using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FXA.DPSE.MessageDispatcher.DipsTransport.Configuration.Elements;
using System.Configuration;

namespace FXA.DPSE.MessageDispatcher.DipsTransport.Configuration
{
    public class DipsTransportMessageDispatcherConfig : IDipsTransportMessageDispatcherConfig
    {
        private IList<EndPointElement> _eodTransportEndPoints;
        private IList<EndPointElement> _payloadTransportEndPoints;
        
        private readonly DipsTransportMessageDispatcherConfigSection _config;
        
        public DipsTransportMessageDispatcherConfig()
        {
            _config = (DipsTransportMessageDispatcherConfigSection)ConfigurationManager.GetSection("dipsTransportMessageDispatcherConfig");
        }

        public IList<EndPointElement> EodTransportEndPoints
        {
            get
            {
                if (_eodTransportEndPoints == null)
                {
                    _eodTransportEndPoints = new List<EndPointElement>();

                    if (_config.EodTransportEndPoints != null && _config.EodTransportEndPoints.Count > 0)
                    {
                        foreach (var endPoint in _config.EodTransportEndPoints)
                        {
                            _eodTransportEndPoints.Add((EndPointElement)endPoint);
                        }
                    }
                }

                return _eodTransportEndPoints;
            }
        }

        public IList<EndPointElement> PayloadTransportEndPoints
        {
            get
            {
                if (_payloadTransportEndPoints == null)
                {
                    _payloadTransportEndPoints = new List<EndPointElement>();

                    if (_config.PayloadTransportEndPoints != null && _config.PayloadTransportEndPoints.Count > 0)
                    {
                        foreach (var endPoint in _config.PayloadTransportEndPoints)
                        {
                            _payloadTransportEndPoints.Add((EndPointElement)endPoint);
                        }    
                    }
                }

                return _payloadTransportEndPoints;    
            }
        }
    }
}