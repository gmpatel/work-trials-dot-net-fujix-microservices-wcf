using System.ServiceModel;
using System.ServiceModel.Web;
using FXA.DPSE.Service.DTO.DuplicateIdentification;

namespace FXA.DPSE.Service.DuplicateIdentification
{
    [ServiceContract]
    public interface IDuplicateIdentificationService
    {
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "duplicateidentification",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        DuplicateIdentificationResponse DuplicateIdentification(DuplicateIdentificationRequest duplicateIdentificationRequest);
    }
}
