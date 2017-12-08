using System.ServiceModel;
using System.ServiceModel.Web;

namespace FXA.DPSE.Framework.Service.Test.Unit.ServiceLibrary.ErrorHandler
{
    [ServiceContract]
    public interface IServiceWrapper
    {
        [OperationContract]
        [WebGet(RequestFormat = WebMessageFormat.Json)]
        void ExecuteAction();
    }

    [ServiceContract]
    public interface IServiceWrapper<T>
    {
        [OperationContract]
        [WebGet(RequestFormat = WebMessageFormat.Json)]
        T ExecuteAction();
    }
}
