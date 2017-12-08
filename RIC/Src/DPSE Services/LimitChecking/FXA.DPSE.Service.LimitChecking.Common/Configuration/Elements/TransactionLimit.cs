using System.Configuration;

namespace FXA.DPSE.Service.LimitChecking.Common.Configuration.Elements
{
    public class TransactionLimit : ConfigurationElement
    {
        [ConfigurationProperty("Amount", DefaultValue = "500000", IsKey = true, IsRequired = true)]
        public int Amount
        {
            get { return (int)base["Amount"]; }
            set { base["Amount"] = value; }
        }
    }
}