using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using FXA.DPSE.Framework.Model.DipsTransport;

namespace FXA.DPSE.Framework.Web.Service.Library.DipsTransport
{
    [ServiceContract]
    public interface IDipsTransportService
    {
        [WebInvoke(Method = "POST", 
            RequestFormat = WebMessageFormat.Json, 
            ResponseFormat = WebMessageFormat.Json, 
            BodyStyle = WebMessageBodyStyle.Bare, 
            UriTemplate = "post"
        )]
        [OperationContract]
        DipsTransportResponse Post(DipsTransportRequest message);
    }
}