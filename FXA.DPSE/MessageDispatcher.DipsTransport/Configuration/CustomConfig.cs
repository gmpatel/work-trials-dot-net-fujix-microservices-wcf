using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FXA.DPSE.MessageDispatcher.DipsTransport.Configuration.Elements;
using System.Configuration;

namespace FXA.DPSE.MessageDispatcher.DipsTransport.Configuration
{
    public class CustomConfig
    {
        private readonly CustomConfigSection _config;
        private static volatile CustomConfig _instance;

        private IList<EndPointElement> _endPointElements;

        private static readonly object ConsturctorLock = new object();

        private CustomConfig()
        {
            if (_config == null)
            {
                _config = (CustomConfigSection)ConfigurationManager.GetSection("dipsTransport");
            }
        }

        public static CustomConfig Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (ConsturctorLock)
                    {
                        if (_instance == null) _instance = new CustomConfig();
                    }
                }

                return _instance;
            }
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