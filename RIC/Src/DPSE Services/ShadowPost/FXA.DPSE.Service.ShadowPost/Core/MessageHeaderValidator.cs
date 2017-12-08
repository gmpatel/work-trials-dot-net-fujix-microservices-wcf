using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Web;
using FXA.DPSE.Framework.Service.WCF.Attributes.Validation;
using FXA.DPSE.Service.ShadowPost.Common;

namespace FXA.DPSE.Service.ShadowPost.Core
{
    public class MessageHeaderValidator : IDispatchMessageInspector
    {
        public object AfterReceiveRequest(ref System.ServiceModel.Channels.Message request, IClientChannel channel, InstanceContext instanceContext)
        {
            var headerKeys = new List<string>()
            {
                "x-nab-user",
                "x-nab-sessionid",
                "x-nab-lob",
                "x-nab-groups",
                "x-nab-provider",
                "x-nab-correlationid",
                "x-forwarded-for",
                "User-Agent",
                "x-nab-customer",
                "x-nab-owning-meid",
                "x-nab-channel"
            };
            
            if (WebOperationContext.Current != null)
            {
                var webRequest = WebOperationContext.Current.IncomingRequest;
                var headers = webRequest.Headers;

                foreach (var headerName in headerKeys)
                {
                    var headerFound = headers.AllKeys.Any(headerKey => headerKey == headerName);
                    if (!headerFound)
                    {
                        throw new DpseValidationException(string.Format("Header {0} not found.", headerName), StatusCode.InputValidationError);
                    }
                    if (string.IsNullOrWhiteSpace(headers[headerName]))
                        throw new DpseValidationException(string.Format("Header: {0} value is invalid or empty.", headerName), StatusCode.InputValidationError);
                }
            }

            return null;
        }

        public void BeforeSendReply(ref System.ServiceModel.Channels.Message reply, object correlationState)
        {
        }
    }
}
