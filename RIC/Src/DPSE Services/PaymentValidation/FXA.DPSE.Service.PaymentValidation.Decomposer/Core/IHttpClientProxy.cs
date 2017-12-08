using System.Collections.Generic;

namespace FXA.DPSE.Service.PaymentValidation.Decomposer.Core
{
    public interface IHttpClientProxy
    {
        TResponse PostSyncAsJson<TRequest, TResponse>(string address, TRequest content, IDictionary<string, string> headers = null, int? timeOutSeconds = null) where TResponse : new();
    }
}