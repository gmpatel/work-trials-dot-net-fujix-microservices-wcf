using System;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using FXA.DPSET.Framework.Service.Infrastructure.Exceptions;
using FXA.DPSET.Framework.Service.Infrastructure.Model;

namespace FXA.DPSET.Framework.Service.Infrastructure.Attributes.Errors
{
    public class ErrorHandler : IErrorHandler
    {
        public bool HandleError(Exception error)
        {
            if (error is ProcessingException)
                return true;
            
            if (error != null)
               return true;
            
            return true;
        }

        public void ProvideFault(Exception error, MessageVersion version, ref Message fault)
        {
            Response response = null;

            var exception = error as ProcessingException;

            if (exception != null)
            {
                var processingException = exception;

                response = new Response()
                {
                    Code = ResponseStatusCode.Code400,
                    Message = string.Format("{0} ({1})", processingException.Error.ShortDescription, exception.Message)
                };

            }
            else if (error != null)
            {
                response = new Response()
                {
                    Code = ResponseStatusCode.Code500,
                    Message = string.Format("Critical unknown server error ({0})", error.Message) 
                };
            }

            fault = Message.CreateMessage(version, "", response);
        }
    }
}