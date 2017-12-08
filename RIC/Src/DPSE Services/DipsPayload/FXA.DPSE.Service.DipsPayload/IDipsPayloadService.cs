using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using FXA.DPSE.Service.DTO.DipsPayload;

namespace FXA.DPSE.Service.DipsPayload
{
    [ServiceContract]
    public interface IDipsPayloadService
    {
        [WebInvoke(Method = "POST",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "dipspayload/batch/all"
        )]
        [OperationContract]
        DipsPayloadBatchResponse DipsPayloadBatch(DipsPayloadBatchRequest request);

        [WebInvoke(Method = "POST",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "dipspayload/batch/single"
        )]
        [OperationContract]
        DipsPayloadSingleBatchResponse DipsPayloadSingleBatch(DipsPayloadSingleBatchRequest request);
    }
}
