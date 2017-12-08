using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Web;
using FXA.DPSE.Framework.Service.WCF.Attributes;
using FXA.DPSE.Framework.Service.WCF.Attributes.Validation;
using FXA.DPSE.Service.PaymentValidation.Common;

namespace FXA.DPSE.Service.PaymentValidation.Core
{
    //TODO: Message header validation should be moved to the Framework.
    //      1) Header setting in the framework config
    //      2) Able to get input validation error from the active WCF service.    
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
            var webRequest = WebOperationContext.Current.IncomingRequest;
            var headers = webRequest.Headers;

            foreach (var headerName in headerKeys)
            {
                var headerFound = headers.AllKeys.Any(headerKey => String.Equals(headerKey, headerName, StringComparison.CurrentCultureIgnoreCase));
                if (!headerFound)
                    throw new DpseValidationException(string.Format("Header {0} not found.", headerName), ResponseCode.InputValidationError);
                if (string.IsNullOrWhiteSpace(headers[headerName]))
                    throw new DpseValidationException(string.Format("Header: {0} value is invalid or empty.", headerName), ResponseCode.InputValidationError);
            }
            return null;
        }

        public void BeforeSendReply(ref System.ServiceModel.Channels.Message reply, object correlationState)
        {
        }
    }

    public class HeaderValidationBehaviorAttribute : ServiceBehaviorBase
    {
        public HeaderValidationBehaviorAttribute() : base(new MessageHeaderValidator())
        {
        }
    }
}
