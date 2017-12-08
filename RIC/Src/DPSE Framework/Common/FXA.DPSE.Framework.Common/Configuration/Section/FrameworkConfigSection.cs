using System.Configuration;
using FXA.DPSE.Framework.Common.Configuration.Elements;

namespace FXA.DPSE.Framework.Common.Configuration.Section
{
    public class FrameworkConfigSection : ConfigurationSection
    {
        [ConfigurationProperty("services", DefaultValue = null, IsRequired = true)]
        public ServicesElement Services
        {
            get { return ((ServicesElement)(base["services"])); }
            set { base["services"] = value; }
        }
    }
}