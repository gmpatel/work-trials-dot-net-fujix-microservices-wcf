using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;
using FXA.DPSE.Framework.Common.Extensions;
using Newtonsoft.Json;

namespace FXA.DPSE.Framework.Common
{
    public class RESTClient
    {
        public Uri BaseUrl { get; private set; }

        public string Token { get; private set; }

        public RESTClient() : this(null, null)
        {
        }

        public RESTClient(string baseUrl) : this(baseUrl, null)
        {
        }

        public RESTClient(string baseUrl, string token)
        {
            BaseUrl = baseUrl == null ? null : new Uri(baseUrl);
            Token = token;
        }

        public void SetToken(string token)
        {
            Token = token;
        }

        public void SetBaseUrl(string baseUrl)
        {
            BaseUrl = new Uri(baseUrl);
        }

        public string GetToken()
        {
            return Token;
        }

        public async Task<T> TryGet<T>(string relativeUrl)
        {
            var url = (relativeUrl.IsAbsoluteUrl()) ? new Uri(relativeUrl) : new Uri(BaseUrl, relativeUrl);

            using (var client = new HttpClient())
            {
                using (var request = new HttpRequestMessage(HttpMethod.Get, url))
                {
                    if (!string.IsNullOrEmpty(this.Token) && !string.IsNullOrWhiteSpace(this.Token))
                    {
                        var authorizationHeader = CreateBasicAuthenticationHeaderValue(this.Token.Trim());

                        if (authorizationHeader != null)
                            request.Headers.Authorization = authorizationHeader;
                    }

                    using (var response = await client.SendAsync(request))
                    {
                        switch (response.StatusCode)
                        {
                            case HttpStatusCode.NoContent:
                                {
                                    return default(T);
                                }
                            case HttpStatusCode.OK:
                                {
                                    if (typeof(T) == typeof(string))
                                    {
                                        var data = await response.Content.ReadAsStringAsync();
                                        return (T)((object)data);
                                    }
                                    if (typeof(T) == typeof(byte[]))
                                    {
                                        var data = await response.Content.ReadAsByteArrayAsync();
                                        return (T)((object)data);
                                    }
                                    if (typeof(T) == typeof(Stream))
                                    {
                                        var data = await response.Content.ReadAsStreamAsync();
                                        return (T)((object)data);
                                    }

                                    var model = await response.Content.ReadAsAsync<T>();
                                    return model;
                                }
                            case HttpStatusCode.Unauthorized:
                                {
                                    throw new UnauthorizedAccessException(string.Format("Unauthorized (401): You are unauthorized for the resource you are trying to access (token = {0})", Token));
                                }
                            case HttpStatusCode.InternalServerError:
                                {
                                    throw new ServerException(string.Format("InternalServerError (500): There was an error at server side (token = {0})", Token));
                                }
                            default:
                                {
                                    throw new SystemException(string.Format("{0} ({1}): Invalid status code of the response received (token = {2})", response.StatusCode, ((int)response.StatusCode), Token));
                                }
                        }
                    }
                }
            }

            return default(T);
        }

        public async Task<T> TryPost<T1, T>(string relativeUrl, T1 content) //where T1 : class
        {
            var url = (relativeUrl.IsAbsoluteUrl()) ? new Uri(relativeUrl) : new Uri(BaseUrl, relativeUrl);

            using (var client = new HttpClient())
            {
                using (var request = new HttpRequestMessage(HttpMethod.Post, url))
                {
                    if (!string.IsNullOrEmpty(this.Token) && !string.IsNullOrWhiteSpace(this.Token))
                    {
                        var authorizationHeader = CreateBasicAuthenticationHeaderValue(this.Token.Trim());

                        if (authorizationHeader != null)
                            request.Headers.Authorization = authorizationHeader;
                    }

                    if (content != null)
                    {
                        var json = default(string);

                        if (typeof (T1) == typeof (string))
                        {
                            json = (content).ToString().IsValidJson() ? (content).ToString() : JsonConvert.SerializeObject(content);
                        }
                        else
                        {
                            json = JsonConvert.SerializeObject(content);    
                        }
                        
                        request.Content = new StringContent(json, Encoding.UTF8, "application/json");
                    }

                    using (var response = await client.SendAsync(request))
                    {
                        switch (response.StatusCode)
                        {
                            case HttpStatusCode.NoContent:
                                {
                                    return default(T);
                                }
                            case HttpStatusCode.OK:
                                {
                                    if (typeof(T) == typeof(string))
                                    {
                                        var data = await response.Content.ReadAsStringAsync();
                                        return (T) ((object) data);
                                    }
                                    if (typeof(T) == typeof(byte[]))
                                    {
                                        var data = await response.Content.ReadAsByteArrayAsync();
                                        return (T)((object)data);
                                    }
                                    if (typeof(T) == typeof(Stream))
                                    {
                                        var data = await response.Content.ReadAsStreamAsync();
                                        return (T)((object)data);
                                    }

                                    var model = await response.Content.ReadAsAsync<T>();
                                    return model;
                                }
                            case HttpStatusCode.Unauthorized:
                                {
                                    throw new UnauthorizedAccessException(string.Format("Unauthorized (401): You are unauthorized for the resource you are trying to access (token = {0})", Token));
                                }
                            case HttpStatusCode.InternalServerError:
                                {
                                    throw new ServerException(string.Format("InternalServerError (500): There was an error at server side (token = {0})", Token));
                                }
                            default:
                                {
                                    throw new SystemException(string.Format("{0} ({1}): Invalid status code of the response received (token = {2})", response.StatusCode, ((int)response.StatusCode), Token));
                                }
                        }
                    }
                }
            }

            return default(T);
        }

        public async Task TryPost<T1>(string relativeUrl, T1 content) //where T1 : class
        {
            var url = (relativeUrl.IsAbsoluteUrl()) ? new Uri(relativeUrl) : new Uri(BaseUrl, relativeUrl);

            using (var client = new HttpClient())
            {
                using (var request = new HttpRequestMessage(HttpMethod.Post, url))
                {
                    if (!string.IsNullOrEmpty(this.Token) && !string.IsNullOrWhiteSpace(this.Token))
                    {
                        var authorizationHeader = CreateBasicAuthenticationHeaderValue(this.Token.Trim());

                        if (authorizationHeader != null)
                            request.Headers.Authorization = authorizationHeader;
                    }

                    if (content != null)
                    {
                        var json = JsonConvert.SerializeObject(content);
                        request.Content = new StringContent(json, Encoding.UTF8, "application/json");
                    }

                    using (var response = await client.SendAsync(request))
                    {
                        switch (response.StatusCode)
                        {
                            case HttpStatusCode.NoContent:
                                {
                                    return;
                                }
                            case HttpStatusCode.OK:
                                {
                                    return;
                                }
                            case HttpStatusCode.Unauthorized:
                                {
                                    throw new UnauthorizedAccessException(string.Format("Unauthorized (401): You are unauthorized for the resource you are trying to access (token = {0})", Token));
                                }
                            case HttpStatusCode.InternalServerError:
                                {
                                    throw new ServerException(string.Format("InternalServerError (500): There was an error at server side (token = {0})", Token));
                                }
                            default:
                                {
                                    throw new SystemException(string.Format("{0} ({1}): Invalid status code of the response received (token = {2})", response.StatusCode, ((int)response.StatusCode), Token));
                                }
                        }
                    }
                }
            }

            return;
        }

        private static AuthenticationHeaderValue CreateBasicAuthenticationHeaderValue(string token)
        {
            if (string.IsNullOrEmpty(token))
                return null;

            var toEncode
                = string.Format("{0}:{1}", "Token", token);

            var encoding
                = Encoding.GetEncoding("iso-8859-1");

            var toBase64
                = encoding.GetBytes(toEncode);

            var parameter
                = Convert.ToBase64String(toBase64);

            return
                new AuthenticationHeaderValue(
                    "Basic", parameter
                );
        }
    }
}