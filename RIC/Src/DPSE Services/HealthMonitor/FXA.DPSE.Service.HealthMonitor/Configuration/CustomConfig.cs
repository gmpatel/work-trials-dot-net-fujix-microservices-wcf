using System.Collections.Generic;
using System.Configuration;
using FXA.DPSE.Service.HealthMonitor.Configuration.Elements;
using FXA.DPSE.Service.HealthMonitor.Configuration.Section;

namespace FXA.DPSE.Service.HealthMonitor.Configuration
{
    public class CustomConfig : ICustomConfig
    {
        private readonly CustomConfigSection _config;
        
        private IList<EndPointElement> _endPointElements;

        public CustomConfig()
        {
            _config = (CustomConfigSection)ConfigurationManager.GetSection("serviceConfig");
        }

        public IList<EndPointElement> EndPoints
        {
            get
            {
                if (_endPointElements == null)
                {
                    _endPointElements = new List<EndPointElement>();

                    foreach (var endPoint in _config.EndPoints)
                    {
                        _endPointElements.Add((EndPointElement)endPoint);
                    }
                }

                return _endPointElements;    
            }
        }
    }
}