using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using FXA.DPSE.Framework.Common.RESTClient;
using FXA.DPSE.Framework.Common.Utils;
using RestSharp;
using RestSharp.Extensions;

namespace FXA.DPSE.Framework.Common.RESTClient
{
    public class HttpClientExtensions
    {
        public static T Deserialize<T>(IRestResponse response) where T : new()
        {
	        if (response == null) throw new ArgumentNullException("response");
 
	        var dataContractJsonSerializer = new DataContractJsonSerializer(typeof(T)); T result;
 
	        using (var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(response.Content)))
	        {
		        memoryStream.Seek(0, SeekOrigin.Begin);
		        result = (T)dataContractJsonSerializer.ReadObject(memoryStream);
		        memoryStream.Close();
	        }
 
	        return result;
        }

        public static T Deserialize<T>(string json) where T : new()
        {
            if (string.IsNullOrEmpty(json)) throw new ArgumentNullException("json");

            var dataContractJsonSerializer = new DataContractJsonSerializer(typeof(T)); T result;

            using (var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(json)))
            {
                memoryStream.Seek(0, SeekOrigin.Begin);
                result = (T)dataContractJsonSerializer.ReadObject(memoryStream);
                memoryStream.Close();
            }

            return result;
        }


        public static HttpResult<TResponse> PostSyncAsJson<TRequest, TResponse>(string address, TRequest content) where TResponse : new()
        {
            return PostSyncAsJson<TRequest, TResponse>(address, content, null);
        }

        public static HttpResult<TResponse> PostSyncAsJson<TRequest, TResponse>(string address, TRequest content, IDictionary<string, string> headers) where TResponse : new()
        {
            return PostSyncAsJson<TRequest, TResponse>(address, content, headers, null);
        }

        public static HttpResult<TResponse> PostSyncAsJson<TRequest, TResponse>(string address, TRequest content, IDictionary<string, string> headers, int? timeOutSeconds) where TResponse : new()
        {
            try
            {
                var request = new RestRequest(Method.POST);
                var message = ConversionUtils.Serialize(content);
                request.AddParameter("application/json; charset=utf-8", message, ParameterType.RequestBody);
                request.RequestFormat = DataFormat.Json;

                if (timeOutSeconds.HasValue) request.Timeout = timeOutSeconds.Value;

                if (headers != null && headers.Count > 0)
                {
                    foreach (var header in headers)
                    {
                        if ((header.Key.StartsWith("x-", StringComparison.CurrentCultureIgnoreCase) || header.Key.StartsWith("user-", StringComparison.CurrentCultureIgnoreCase)))
                        {
                            request.AddHeader(header.Key, header.Value);
                        }
                    }
                }

                var restClient = new RestClient(address);
                var response = restClient.Post(request); //<TResponse>(request);
                var json = response.Content;
                var result = Deserialize<TResponse>(json);
                var httpResult = new HttpResult<TResponse>
                {
                    StatusCode = response.StatusCode,
                    Content = result
                };

                return httpResult;
            }
            catch (System.Exception exception)
            {
                return HttpResult<TResponse>.Failure(exception.Message);
            }
        }

        public static HttpResult<TResponse> PutSyncAsJson<TRequest, TResponse>(string address, TRequest content, IDictionary<string, string> headers = null, int? timeOutSeconds = null) where TResponse : new()
        {
            try
            {
                var restClient = new RestClient(address);
                var request = new RestRequest(Method.PUT);
                var message = ConversionUtils.Serialize(content);
                request.AddParameter("application/json; charset=utf-8", message, ParameterType.RequestBody);
                request.RequestFormat = DataFormat.Json;

                if (timeOutSeconds.HasValue) request.Timeout = timeOutSeconds.Value;
                if (headers != null && headers.Count > 0)
                {
                    foreach (var header in headers)
                    {
                        if ((header.Key.StartsWith("x-", StringComparison.CurrentCultureIgnoreCase) || header.Key.StartsWith("user-", StringComparison.CurrentCultureIgnoreCase)))
                        {
                            request.AddHeader(header.Key, header.Value);
                        }
                    }
                }
                var response = restClient.Put(request); //<TResponse>(request);
                var json = response.Content;
                var result = Deserialize<TResponse>(json);
                var httpResult = new HttpResult<TResponse> { StatusCode = response.StatusCode, Content = result };
                return httpResult;
            }
            catch (System.Exception exception)
            {
                return HttpResult<TResponse>.Failure(exception.Message);
            }
        }

        public static async Task<HttpResult<TResponse>> PostAsyncAsJson<TRequest, TResponse>(string address, TRequest content, IDictionary<string, string> headers = null, int? timeOutSeconds = null) where TResponse : new()
        {
            try
            {
                var request = new RestRequest(Method.POST);
                var message = ConversionUtils.Serialize(content);
                request.AddParameter("application/json; charset=utf-8", message, ParameterType.RequestBody);
                request.RequestFormat = DataFormat.Json;

                if (timeOutSeconds.HasValue) request.Timeout = timeOutSeconds.Value;
                if (headers != null && headers.Count > 0)
                {
                    foreach (var header in headers)
                    {
                        if ((header.Key.StartsWith("x-", StringComparison.CurrentCultureIgnoreCase) || header.Key.StartsWith("user-", StringComparison.CurrentCultureIgnoreCase)))
                        {
                            request.AddHeader(header.Key, header.Value);
                        }
                    }
                }

                var restClient = new RestClient(address);
                var taskResponse = await restClient.ExecutePostTaskAsync(request);
                var response = taskResponse.toAsyncResponse<TResponse>();

                var httpResult = new HttpResult<TResponse>
                {
                    StatusCode = response.StatusCode,
                    Content = response.Data
                };
                return httpResult;
            }
            catch (System.Exception exception)
            {
                return HttpResult<TResponse>.Failure(exception.Message);
            }
        }

        public static async Task<HttpResult<TResponse>> PutAsyncAsJson<TRequest, TResponse>(string address, TRequest content, IDictionary<string, string> headers = null, int? timeOutSeconds = null) where TResponse : new()
        {
            try
            {
                var request = new RestRequest(Method.PUT);
                var message = ConversionUtils.Serialize(content);
                request.AddParameter("application/json; charset=utf-8", message, ParameterType.RequestBody);
                request.RequestFormat = DataFormat.Json;

                if (timeOutSeconds.HasValue) request.Timeout = timeOutSeconds.Value;
                if (headers != null && headers.Count > 0)
                {
                    foreach (var header in headers)
                    {
                        if ((header.Key.StartsWith("x-", StringComparison.CurrentCultureIgnoreCase) || header.Key.StartsWith("user-", StringComparison.CurrentCultureIgnoreCase)))
                        {
                            request.AddHeader(header.Key, header.Value);
                        }
                    }
                }
                var restClient = new RestClient(address);
                var taskResponse = await restClient.ExecuteTaskAsync(request);
                var response = taskResponse.toAsyncResponse<TResponse>();

                var httpResult = new HttpResult<TResponse>
                {
                    StatusCode = response.StatusCode,
                    Content = response.Data
                };
                return httpResult;
            }
            catch (System.Exception exception)
            {
                return HttpResult<TResponse>.Failure(exception.Message);
            }
        }
    }
}