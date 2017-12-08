using FXA.DPSE.Service.Logging.Business.BusinessEntity;

namespace FXA.DPSE.Service.Logging.Business
{
    public interface ILoggingBusiness
    {
        LoggingBusinessResult Log(EventLog loggingRequest);
    }
}