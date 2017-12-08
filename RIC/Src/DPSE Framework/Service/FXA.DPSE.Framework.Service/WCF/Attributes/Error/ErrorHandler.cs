using System;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Web;
using FXA.DPSE.Framework.Common.Configuration;
using FXA.DPSE.Framework.Service.WCF.Attributes.Validation;
using FXA.DPSE.Framework.Service.WCF.Proxy.Logging;
using FXA.DPSE.Service.DTO.Core.Response;

namespace FXA.DPSE.Framework.Service.WCF.Attributes.Error
{
    //TODO: Need to developr ValidationErrorHandler to handle Deserialization exception and DpseValidation exceptions.
    //      This filter has to handle the error and send the response back by itself.
    //      The current generic error handler could check some exception types and pass validation related errors to ValidationErrorHandler.
    public class ErrorHandler : IErrorHandler
    {
        private readonly string _errorCode;

        public ErrorHandler(string errorCode)
        {
            _errorCode = errorCode;
        }

        public bool HandleError(Exception error)
        {
            //TODO: Message is already closed in the Fault mode.
            // Create an extension to the OperationContext.Extension.
            // All the extension object properties (e.g. ServiceName, OperationName) could be set earlier in after recieved request 
            // which all independent to the message.

            //var frameworkConfiguration = new FrameworkConfig();
            //if (frameworkConfiguration.Services.LoggingService.Enabled)
            //{
            //    var exception = error as DpseValidationException;
            //    var errorMessage = exception != null ? exception.Message : string.Format("A critical error occurred processing the request.{0}", error.Message);
            //    var methodName = OperationContext. Current.IncomingMessageProperties["HttpOperationName"] as string;
            //    var serviceName = OperationContext.Current.InstanceContext.Host.Description.Name;

            //    var result = new LoggingProxy(frameworkConfiguration).LogEventAsync(string.Empty, "ApplicationException",
            //        errorMessage, LogLevel.Error.ToString(), string.Empty, string.Empty,
            //        string.Empty, string.Empty, serviceName, methodName);

            //    if (result.HasException)
            //    {
            //        //Do nothing ....
            //    }
            //}
            
            return true;
        }

        public void ProvideFault(Exception error, MessageVersion version, ref Message fault)
        {
            if (error == null) return;
            var response = new DpseResponseBase();

            var exception = error as DpseValidationException;
            if (exception != null)
            {
                if (
                    exception.ServiceValidationErrorCode.StartsWith("DPSE-7",
                        StringComparison.InvariantCultureIgnoreCase) ||
                    exception.ServiceValidationErrorCode.StartsWith("DPSE-8",
                        StringComparison.InvariantCultureIgnoreCase) ||
                    exception.ServiceValidationErrorCode.StartsWith("DPSE-9",
                        StringComparison.InvariantCultureIgnoreCase))
                {
                    response.Code = null;
                }
                else
                {
                    response.Code = exception.ServiceValidationErrorCode;
                }
                
                response.Message = exception.Message;

                var input = exception.ServiceInputs.FirstOrDefault();

                if (input != null)
                {
                    var externalCorrelationId = input.GetType().GetProperty("ExternalCorrelationId", typeof(string));

                    if (externalCorrelationId != null)
                    {
                        var externalCorrelationIdValue = externalCorrelationId.GetValue(input, null);
                        response.ExternalCorrelationId = externalCorrelationIdValue.ToString();
                    }

                    var trackingId = input.GetType().GetProperty("TrackingId", typeof(string));

                    if (trackingId != null)
                    {
                        var trackingIdValue = trackingId.GetValue(input, null);
                        response.TrackingId = trackingIdValue.ToString();
                    }

                    var documentReferenceNumber = input.GetType().GetProperty("DocumentReferenceNumber", typeof(string));

                    if (documentReferenceNumber != null)
                    {
                        var documentReferenceNumberValue = documentReferenceNumber.GetValue(input, null);
                        response.DocumentReferenceNumber = documentReferenceNumberValue.ToString();
                    }
                }

                WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.BadRequest;
            }
            else
            {
                if (
                    _errorCode.StartsWith("DPSE-7",
                        StringComparison.InvariantCultureIgnoreCase) ||
                    _errorCode.StartsWith("DPSE-8",
                        StringComparison.InvariantCultureIgnoreCase) ||
                    _errorCode.StartsWith("DPSE-9",
                        StringComparison.InvariantCultureIgnoreCase))
                {
                    response.Code = null;
                }
                else
                {
                    response.Code = _errorCode;    
                }

                response.Message = string.Format("A critical error occurred processing the request.{0}", error.Message);
                WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.InternalServerError;
            }

            fault = WebOperationContext.Current.CreateJsonResponse(response);
        }
    }
}