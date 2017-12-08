using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.Text;
using FXA.DPSE.Framework.Common.Configuration;
using FXA.DPSE.Framework.Service.WCF.Proxy.Logging;
using Newtonsoft.Json.Linq;

namespace FXA.DPSE.Framework.Service.WCF.Attributes.Logging
{
    public class LoggingMessageInspector : IDispatchMessageInspector
    {
        public object AfterReceiveRequest(ref Message request, IClientChannel channel, InstanceContext instanceContext)
        {
            if (!new FrameworkConfig().Services.LoggingService.Enabled) return null;
            
            var httpReq = (HttpRequestMessageProperty)request.Properties[HttpRequestMessageProperty.Name];
            var requestUri = request.Headers.To;

            var requestInfo = new StringBuilder();
            requestInfo.Append(string.Format("Method:{0} Uri:{1}", httpReq.Method, requestUri));

            foreach (var header in httpReq.Headers.AllKeys)
            {
                requestInfo.Append(string.Format("HeaderKey:{0} HeaderValue:{1}", header, httpReq.Headers[header]));
            }
            var requestJsonContent = string.Empty;
            var trackingId = string.Empty;
            var sessionId = string.Empty;
            var channelType = string.Empty;

            /* TODO: 
             * 1) The following fields are not available (event at the same level) in all the request. (Design issue)
             * 2) Inject logging impl here by IoC (per domain)
             * 3) The common fields below should be available in the RequestBase DTO object which \
             *     means no parsing or dynamic is required.   
             */
            
            //if (!request.IsEmpty)
            //{
            //    requestJsonContent = request.MessageToString(ref request);
            //    dynamic dpseRequest = JToken.Parse(requestJsonContent);
            //    trackingId = dpseRequest.tracking_id;
            //    sessionId = dpseRequest.session_id;
            //    channelType = dpseRequest.channel_type;
            //}

            var correlationState = new DpseMessageCorrelationState()
            {
                MethodName = OperationContext.Current.IncomingMessageProperties["HttpOperationName"] as string,
                ServiceName = OperationContext.Current.InstanceContext.Host.Description.Name,
                Metadata = requestInfo.ToString(),
                Request = requestJsonContent,
                TrackingId = (string.IsNullOrWhiteSpace(trackingId)) ? trackingId : string.Empty,
                ChannelType = (string.IsNullOrWhiteSpace(channelType)) ? channelType : string.Empty,
                SessionId = (string.IsNullOrWhiteSpace(sessionId)) ? sessionId : string.Empty,
            };
            return correlationState;
        }

        public void BeforeSendReply(ref Message reply, object correlationState)
        {
            if (!new FrameworkConfig().Services.LoggingService.Enabled) return;

            var serviceInvocationResult = correlationState as DpseMessageCorrelationState;
            if (serviceInvocationResult != null)
            {
                //TODO: Complete this when got the ResponseDTO base to read the Code and Message.
                //var responseJsonContent = reply.MessageToString(ref reply);
                //serviceInvocationResult.Response = responseJsonContent;
                //dynamic dpseResponseBase = JToken.Parse(responseJsonContent);
                //serviceInvocationResult.ResponseCode = dpseResponseBase.code;
                //serviceInvocationResult.ResponseMessage = dpseResponseBase.message;
                //TODO: Check reply.IsFault and add exception details in the log message ?
                LogServiceInvocation(serviceInvocationResult);
            }
        }

        private void LogServiceInvocation(DpseMessageCorrelationState invocationResult)
        {
            var loggingResult = new LoggingProxy(new FrameworkConfig()).LogEventAsync(invocationResult.TrackingId,
                "ServiceInvocation", string.Format("ResponseCode:{0} ResponseMessage:{1}", invocationResult.ResponseCode,
                    invocationResult.ResponseMessage), LogLevel.Information.ToString(),
                "Request:" + invocationResult.Request, "Response:" + invocationResult.Response,
                invocationResult.ChannelType, invocationResult.SessionId, invocationResult.ServiceName,
                invocationResult.MethodName);

            if (loggingResult.HasException)
            {
                //TODO: FXA MCC REC 091
                
            }
        }
    }
}