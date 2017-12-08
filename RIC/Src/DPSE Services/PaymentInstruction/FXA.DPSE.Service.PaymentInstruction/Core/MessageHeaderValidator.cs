using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Web;
using FXA.DPSE.Framework.Service.WCF.Attributes;
using FXA.DPSE.Framework.Service.WCF.Attributes.Validation;
using FXA.DPSE.Service.PaymentInstruction.Common.Configuration;

namespace FXA.DPSE.Service.PaymentInstruction.Core
{
    public class MessageHeaderValidator : IDispatchMessageInspector
    {
        //TODO: 
        // 1) Move header validator to the Framework.Service project.
        // 2) Header names should be moved to the framework. Each element may have key and value attributes.
        public object AfterReceiveRequest(ref Message request, IClientChannel channel, InstanceContext instanceContext)
        {
            var methodName = OperationContext.Current.IncomingMessageProperties["HttpOperationName"] as string;
            var operations = new PaymentInstructionServiceConfiguration().HeaderValidation.Operations.Split(',');
            if (!operations.Contains(methodName)) return null;

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
                var headerFound = headers.AllKeys.Any(headerKey => headerKey == headerName);
                if (!headerFound)
                    throw new DpseValidationException(string.Format("Header {0} not found.", headerName), "DPSE-9001");
                if (string.IsNullOrWhiteSpace(headers[headerName]))
                    throw new DpseValidationException(string.Format("Header: {0} value is invalid or empty.", headerName), "DPSE-9001");
            }
            return null;
        }

        public void BeforeSendReply(ref Message reply, object correlationState)
        {
        }
    }

    public class HeaderValidationBehaviorAttribute : ServiceBehaviorBase
    {
        public HeaderValidationBehaviorAttribute()
            : base(new MessageHeaderValidator())
        {
        }
    }
}
