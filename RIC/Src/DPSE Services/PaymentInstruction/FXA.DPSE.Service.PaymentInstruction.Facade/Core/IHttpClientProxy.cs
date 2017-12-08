using System.Collections.Generic;

namespace FXA.DPSE.Service.PaymentInstruction.Facade.Core
{
    public interface IHttpClientProxy
    {
        TResponse PostSyncAsJson<TRequest, TResponse>(string address, TRequest content) where TResponse : new();
        TResponse PostSyncAsJson<TRequest, TResponse>(string address, TRequest content, IDictionary<string, string> headers) where TResponse : new();
        TResponse PostSyncAsJson<TRequest, TResponse>(string address, TRequest content, IDictionary<string, string> headers, int? timeOutSeconds) where TResponse : new();
    }
}