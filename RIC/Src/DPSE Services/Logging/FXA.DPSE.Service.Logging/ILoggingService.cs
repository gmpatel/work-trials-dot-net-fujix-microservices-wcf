using System;
using System.ServiceModel;
using System.ServiceModel.Web;
using FXA.DPSE.Service.DTO.Logging;

namespace FXA.DPSE.Service.Logging
{
    [ServiceContract]
    public interface ILoggingService
    {
        [OperationContract]
        [WebInvoke(Method = "POST",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
        UriTemplate = "logging") ]
        LoggingResponse Logging(LoggingRequest loggingRequest);
    }
}