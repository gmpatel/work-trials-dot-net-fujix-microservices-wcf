using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using FXA.DPSE.Framework.Service.WCF.Attributes.Error;
using FXA.DPSE.Framework.Service.WCF.Attributes.Validation;
using FXA.DPSE.Service.DTO.DipsTransport;

namespace FXA.DPSE.Service.DipsTransport
{
    [ServiceContract]
    public interface IDipsTransportService
    {
        [WebInvoke(Method = "POST",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "dipstransport/batch"
        )]
        [OperationContract]
        DipsTransportPayloadResponse DipsTransportBatch(DipsTransportPayloadRequest request);

        [WebInvoke(Method = "POST",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "dipspayload/eod"
        )]
        [OperationContract]
        DipsTransportEodResponse DipsPayloadEod(DipsTransportEodRequest request);
    }
}