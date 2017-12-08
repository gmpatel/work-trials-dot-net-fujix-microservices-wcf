using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using FXA.DPSET.Framework.Service.Infrastructure.Attributes.Ping;
using FXA.DPSET.Framework.Service.Library.Model;

namespace FXA.DPSET.Framework.Service.Library
{
    [ServiceContract]
    public interface IHealthMonitorService
    {   
        [WebInvoke(Method = "POST",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "post"
        )]
        [OperationContract]
        HealthMonitorResponse Post(HealthMonitorRequest message);

        [WebInvoke(Method = "POST",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "exception"
        )]
        [OperationContract]
        bool Exception(string data);
    }
}