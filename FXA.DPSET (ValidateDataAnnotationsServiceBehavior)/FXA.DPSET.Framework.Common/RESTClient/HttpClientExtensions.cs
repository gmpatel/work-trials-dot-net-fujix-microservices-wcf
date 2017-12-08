using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace FXA.DPSET.Framework.Common.RESTClient
{
    public class HttpClientExtensions
    {
        public static HttpResult<T> GetSync<T>(string address)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    using (var response = client.GetAsync(address).Result)
                    {
                        var result = new HttpResult<T> {StatusCode = response.StatusCode };
                        
                        if (response.Content != null)
                        {
                            result.Content = response.Content.ReadAsAsync<T>().Result;
                        }
                        
                        return result;
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                return HttpResult<T>.Failure(ex.Message);
            }
        }

        public static async Task<HttpResult<T>> GetAsync<T>(string address)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    using (var response = await client.GetAsync(address))
                    {
                        var result = new HttpResult<T> {StatusCode = response.StatusCode };
                        
                        if (response.Content != null)
                        {
                            result.Content = await response.Content.ReadAsAsync<T>();
                        }

                        return result;
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                return HttpResult<T>.Failure(ex.Message);
            }
        }

        public static HttpResult<TResponse> PostSyncAsJson<TRequest, TResponse>(string address, TRequest content) where TRequest : class
        {
            try
            {
                using (var client = new HttpClient())

                using (var request = new HttpRequestMessage(HttpMethod.Post, address))
                {
                    request.Content = new ObjectContent<TRequest>(content, GetJsonFormatter());

                    using (var response = client.SendAsync(request).Result)
                    {
                        var result = new HttpResult<TResponse> {StatusCode = response.StatusCode };

                        if (response.Content != null)
                        {
                            result.Content = response.Content.ReadAsAsync<TResponse>().Result;
                        }

                        return result;
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                return HttpResult<TResponse>.Failure(ex.Message);
            }
        }

        public static async Task<HttpResult<TResponse>> PostAsyncAsJson<TRequest, TResponse>(string address, TRequest content)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    using (var request = new HttpRequestMessage(HttpMethod.Post, address))
                    {
                        request.Content = new ObjectContent<TRequest>(content, GetJsonFormatter());

                        using (var response = await client.SendAsync(request))
                        {
                            var result = new HttpResult<TResponse> {StatusCode = response.StatusCode };
                            
                            if (response.Content != null)
                            {
                                result.Content = await response.Content.ReadAsAsync<TResponse>();
                            }

                            return result;
                        }
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                return HttpResult<TResponse>.Failure(ex.Message);
            }
        }

        public static HttpResult<TResponse> PutSyncAsJson<TRequest, TResponse>(string address, TRequest content)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    using (var request = new HttpRequestMessage(HttpMethod.Put, address))
                    {
                        request.Content = new ObjectContent<TRequest>(content, GetJsonFormatter());

                        using (var response = client.SendAsync(request).Result)
                        {
                            var result = new HttpResult<TResponse> {StatusCode = response.StatusCode };
                            
                            if (response.Content != null)
                            {
                                result.Content = response.Content.ReadAsAsync<TResponse>().Result;
                            }

                            return result;
                        }
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                return HttpResult<TResponse>.Failure(ex.Message);
            }
        }

        public static async Task<HttpResult<TResponse>> PutAsyncAsJson<TRequest, TResponse>(string address, TRequest content)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    using (var request = new HttpRequestMessage(HttpMethod.Put, address))
                    {
                        request.Content = new ObjectContent<TRequest>(content, GetJsonFormatter());
                        
                        using (var response = await client.SendAsync(request))
                        {
                            var result = new HttpResult<TResponse> {StatusCode = response.StatusCode };
                            
                            if (response.Content != null)
                            {
                                result.Content = await response.Content.ReadAsAsync<TResponse>();
                            }

                            return result;
                        }
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                return HttpResult<TResponse>.Failure(ex.Message);
            }
        }

        public static HttpResult DeleteSync(string address)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    using (var response = client.DeleteAsync(address).Result)
                    {
                        return new HttpResult {StatusCode = response.StatusCode };
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                return HttpResult.Failure(ex.Message);
            }
        }

        public static async Task<HttpResult> DeleteAsync(string address)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    using (var response = await client.DeleteAsync(address))
                    {
                        return new HttpResult { StatusCode = response.StatusCode };
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                return HttpResult.Failure(ex.Message);
            }
        }

        private static MediaTypeFormatter GetJsonFormatter()
        {
            return new JsonMediaTypeFormatter
            {
                SerializerSettings = new JsonSerializerSettings {NullValueHandling = NullValueHandling.Ignore}
            };
        }
    }
}