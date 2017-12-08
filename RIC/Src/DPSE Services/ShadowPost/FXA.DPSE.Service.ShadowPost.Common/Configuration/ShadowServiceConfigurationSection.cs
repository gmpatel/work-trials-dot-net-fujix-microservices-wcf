using System.Configuration;

namespace FXA.DPSE.Service.ShadowPost.Common.Configuration
{
    public class ShadowPostServiceConfiguration : IShadowPostServiceConfiguration
    {
        private readonly ShadowServiceConfigurationSection _serviceConfiguration;

        public ShadowPostServiceConfiguration()
        {
            _serviceConfiguration =
                (ShadowServiceConfigurationSection)ConfigurationManager.GetSection("serviceConfig");
        }


        public InternetBanking InternetBanking
        {
            get { return _serviceConfiguration.InternetBanking; }
        }
    }

    public class ShadowServiceConfigurationSection : ConfigurationSection
    {
        [ConfigurationProperty("InternetBanking")]
        public InternetBanking InternetBanking
        {
            get { return ((InternetBanking)(base["InternetBanking"])); }
            set { base["InternetBanking"] = value; }
        }
    }

    public class InternetBanking : ConfigurationElement
    {
        [ConfigurationProperty("url", DefaultValue = "http://localhost:91/nab/internetbanking/updateaccountbalance", IsKey = true, IsRequired = true)]
        public string Url
        {
            get { return (string)base["url"]; }
            set { base["url"] = value; }
        }

        [ConfigurationProperty("timeOutSeconds", DefaultValue = null)]
        public int? TimeOutSeconds
        {
            get { return (int?)base["timeOutSeconds"]; }
            set { base["timeOutSeconds"] = value; }
        }

        [ConfigurationProperty("enabled", DefaultValue = true)]
        public bool Enabled
        {
            get { return (bool)base["enabled"]; }
            set { base["enabled"] = value; }
        }
    }
}