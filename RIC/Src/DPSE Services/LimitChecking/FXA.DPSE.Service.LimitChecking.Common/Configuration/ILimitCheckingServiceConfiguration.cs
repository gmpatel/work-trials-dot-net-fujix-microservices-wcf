using FXA.DPSE.Service.LimitChecking.Common.Configuration.Elements;

namespace FXA.DPSE.Service.LimitChecking.Common.Configuration
{
    public interface ILimitCheckingServiceConfiguration
    {
        TransactionLimit TransactionLimit { get; }
    }
}