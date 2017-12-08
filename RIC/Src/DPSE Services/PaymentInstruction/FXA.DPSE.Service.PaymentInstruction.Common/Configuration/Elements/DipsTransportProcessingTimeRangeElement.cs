using System.Configuration;

namespace FXA.DPSE.Service.PaymentInstruction.Common.Configuration.Elements
{
    public class DipsTransportProcessingTimeRangeElement : ConfigurationElement
    {
        [ConfigurationProperty("start", DefaultValue = 0, IsRequired = true)]
        public int StartHour
        {
            get { return (int)base["start"]; }
            set { base["start"] = value; }
        }

        [ConfigurationProperty("end", DefaultValue = 0, IsRequired = true)]
        public int EndHour
        {
            get { return (int)base["end"]; }
            set { base["end"] = value; }
        }
    }
}
