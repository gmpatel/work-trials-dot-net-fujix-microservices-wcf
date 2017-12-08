using System.Configuration;

namespace FXA.DPSE.Service.TraceTracking.Common.Configuration.Elements
{
    public class TraceTrackingDuplicateRequestElement : ConfigurationElement
    {
        [ConfigurationProperty("timeOutMiliseconds", DefaultValue = 60000, IsRequired = true)]
        public int TimeOutMiliseconds
        {
            get { return (int)base["timeOutMiliseconds"]; }
            set { base["timeOutMiliseconds"] = value; }
        }
    }
}