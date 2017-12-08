using System.Collections.Generic;
using FXA.DPSE.Framework.Common.RESTClient;

namespace FXA.DPSE.Service.PaymentInstruction.Facade.Core
{
    public class HttpClientProxy : IHttpClientProxy
    {
        public TResponse PostSyncAsJson<TRequest, TResponse>(string address, TRequest content) where TResponse : new()
        {
            return HttpClientExtensions.PostSyncAsJson<TRequest, TResponse>(address, content).Content;
        }

        public TResponse PostSyncAsJson<TRequest, TResponse>(string address, TRequest content, IDictionary<string, string> headers) where TResponse : new()
        {
            return HttpClientExtensions.PostSyncAsJson<TRequest, TResponse>(address, content, headers).Content;
        }

        public TResponse PostSyncAsJson<TRequest, TResponse>(string address, TRequest content, IDictionary<string, string> headers, int? timeOutSeconds) where TResponse : new()
        {
            return HttpClientExtensions.PostSyncAsJson<TRequest, TResponse>(address, content, headers, timeOutSeconds).Content;
        }
    }
}