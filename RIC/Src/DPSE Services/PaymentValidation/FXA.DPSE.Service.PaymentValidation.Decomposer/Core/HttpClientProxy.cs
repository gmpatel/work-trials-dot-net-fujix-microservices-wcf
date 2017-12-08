using System.Collections.Generic;
using FXA.DPSE.Framework.Common.RESTClient;

namespace FXA.DPSE.Service.PaymentValidation.Decomposer.Core
{
    public class HttpClientProxy : IHttpClientProxy
    {
        public TResponse PostSyncAsJson<TRequest, TResponse>(string address, TRequest content, IDictionary<string, string> headers = null, int? timeOutSeconds = null) where TResponse : new()
        {
            return HttpClientExtensions.PostSyncAsJson<TRequest, TResponse>(address, content).Content;
        }
    }
}