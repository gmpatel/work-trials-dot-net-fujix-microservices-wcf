using System.Collections.Generic;
using System.Configuration;
using FXA.DPSE.Framework.Common.Configuration.Elements;
using FXA.DPSE.Framework.Common.Configuration.Section;

namespace FXA.DPSE.Framework.Common.Configuration
{
    public class FrameworkConfig : IFrameworkConfig
    {
        private readonly FrameworkConfigSection _config;

        public FrameworkConfig()
        {
            _config = 
                (FrameworkConfigSection)
                ConfigurationManager.GetSection("frameworkConfig");
        }

        public ServicesElement Services
        {
            get { return _config.Services; }
        }
    }
}