using System.ServiceModel;
using System.ServiceModel.Web;

namespace FXA.DPSE.Framework.Service.Test.Unit.ServiceLibrary.Logging
{
    [ServiceContract]
    public interface IServiceWrapper
    {
        [OperationContract]
        [WebGet(RequestFormat = WebMessageFormat.Json)]
        void MethodInterceptedByLogging();

        [OperationContract]
        [WebGet(RequestFormat = WebMessageFormat.Json)]
        void MethodIgnoredByLogging();
        
        [OperationContract]
        [WebGet(RequestFormat = WebMessageFormat.Json)]
        void MethodThatCallsLoggingServiceInternally();
    }
}