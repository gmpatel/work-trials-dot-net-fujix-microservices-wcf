using System.Collections.Generic;
using FXA.DPSE.Framework.Common.RESTClient;

namespace FXA.DPSE.Service.ShadowPost.Facade.Core
{
    public interface IHttpClientProxy
    {
        HttpResult<TResponse> PostSyncAsJson<TRequest, TResponse>(string address, TRequest content, IDictionary<string, string> headers = null, int? timeOutSeconds = null) where TResponse : new();
    }
}