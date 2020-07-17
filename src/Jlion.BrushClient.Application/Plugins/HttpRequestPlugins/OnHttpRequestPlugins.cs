using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Jlion.BrushClient.Application.Model;
using Jlion.BrushClient.Framework;

namespace Jlion.BrushClient.Application.Plugins
{
    /// <summary>
    /// Http 请求基类
    /// </summary>
    public class OnHttpRequestPlugins : OnBasePlugins
    {
        private readonly string _httpHost;
        //private readonly ProxyConfigModel _proxyConfig;
        public OnHttpRequestPlugins(string httpHost = "")
        {
            _httpHost = httpHost;
            //_proxyConfig = proxyConfig;
        }

        #region Get Method
        /// <summary>
        /// Get请求
        /// </summary>
        /// <typeparam name="TResponse"></typeparam>
        /// <param name="requestUri"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public async Task<TResponse> Get<TResponse>(string requestUri, params KeyValuePair<string, string>[] values)
        {
            return await Get<TResponse>(requestUri, 0, values);
        }

        /// <summary>
        /// Get请求：等待秒数
        /// </summary>
        /// <typeparam name="TResponse"></typeparam>
        /// <param name="requestUri"></param>
        /// <param name="waitSecond"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public async Task<TResponse> Get<TResponse>(string requestUri, int waitSecond, params KeyValuePair<string, string>[] values)
        {
            return await Get<TResponse>(requestUri, waitSecond, null, values);
        }

        /// <summary>
        /// Get请求：Header
        /// </summary>
        /// <typeparam name="TResponse"></typeparam>
        /// <param name="requestUri"></param>
        /// <param name="hkvs"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public async Task<TResponse> Get<TResponse>(string requestUri, KeyValuePair<string, string>[] hkvs, params KeyValuePair<string, string>[] values)
        {
            return await Get<TResponse>(requestUri, 0, hkvs, values);
        }

        /// <summary>
        /// Get请求：等待秒数+Header
        /// </summary>
        /// <typeparam name="TResponse"></typeparam>
        /// <param name="requestUri"></param>
        /// <param name="waitSecond">请求等待秒数</param>
        /// <param name="values"></param>
        /// <returns></returns>
        public async Task<TResponse> Get<TResponse>(string requestUri, int waitSecond, KeyValuePair<string, string>[] hkvs, params KeyValuePair<string, string>[] values)
        {
            var result = string.Empty;
            var requestUriWithQuery = string.Empty;
            HttpWebRequest request = default(HttpWebRequest);
            try
            {


                using (var content = new FormUrlEncodedContent(values))
                {
                    var query = await content.ReadAsStringAsync();
                    requestUriWithQuery = string.Concat(_httpHost, requestUri, "?", query);

                    request = (HttpWebRequest)HttpWebRequest.Create(requestUriWithQuery);

                    #region 代理服务器
                    var proxy = GetProxy();
                    if (proxy != null)
                    {
                        request.Proxy = proxy;
                        request.UseDefaultCredentials = true;
                    }
                    #endregion

                    request.KeepAlive = false;
                    request.Method = "GET";
                    if (waitSecond > 0)
                        request.Timeout = waitSecond * 1000;
                    if (hkvs?.Length > 0)
                    {
                        foreach (var headerKv in hkvs)
                        {
                            request.Headers.Add(headerKv.Key, headerKv.Value);
                        }
                    }

                    using (var response = request.GetResponse())
                    using (var reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                    {
                        result = await reader.ReadToEndAsync();
                        return JsonConvert.DeserializeObject<TResponse>(result);
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("401"))
                    throw;
                TextHelper.Error($"OnHttpRequestPlugins: {requestUri}?{requestUriWithQuery}", ex);
                return default(TResponse);
            }
            finally
            {
                if (request != null)
                {
                    request.Abort();
                    request = null;
                }
            }
        }
        #endregion

        #region PostMethod Async
        /// <summary>
        /// Post请求
        /// </summary>
        /// <typeparam name="TResponse"></typeparam>
        /// <param name="requestUri"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public async Task<TResponse> PostAsync<TResponse>(string requestUri, params KeyValuePair<string, string>[] values)
        {
            return await PostAsync<TResponse>(requestUri, 0, null, values);
        }

        /// <summary>
        /// Post请求：等待秒数
        /// </summary>
        /// <typeparam name="TResponse"></typeparam>
        /// <param name="requestUri"></param>
        /// <param name="waitSecond"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public async Task<TResponse> PostAsync<TResponse>(string requestUri, int waitSecond, params KeyValuePair<string, string>[] values)
        {
            return await PostAsync<TResponse>(requestUri, waitSecond, null, values);
        }

        /// <summary>
        /// Post请求：Header
        /// </summary>
        /// <typeparam name="TResponse"></typeparam>
        /// <param name="requestUri"></param>
        /// <param name="hkvs"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public async Task<TResponse> PostAsync<TResponse>(string requestUri, KeyValuePair<string, string>[] hkvs, params KeyValuePair<string, string>[] values)
        {
            return await PostAsync<TResponse>(requestUri, 0, hkvs, values);
        }

        /// <summary>
        /// Post请求： waitSecond + Header
        /// </summary>
        /// <typeparam name="TResponse"></typeparam>
        /// <param name="requestUri"></param>
        /// <param name="hkvs">headers</param>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<TResponse> PostAsync<TResponse>(string requestUri, int waitSecond, KeyValuePair<string, string>[] hkvs = null, params KeyValuePair<string, string>[] values)
        {
            var result = string.Empty;
            var requestUriWithHost = string.Concat(_httpHost, requestUri);
            HttpWebRequest request = default(HttpWebRequest);
            try
            {
                System.GC.Collect();
                using (var content = new FormUrlEncodedContent(values))
                {
                    request = (HttpWebRequest)HttpWebRequest.Create(requestUriWithHost);

                    #region 代理服务器
                    var proxy = GetProxy();
                    if (proxy != null)
                    {
                        request.Proxy = proxy;
                        request.UseDefaultCredentials = true;
                    }
                    #endregion

                    request.KeepAlive = false;
                    request.Method = "POST";
                    request.ContentType = "application/x-www-form-urlencoded";
                    if (waitSecond > 0)
                        request.Timeout = waitSecond * 1000;
                    if (hkvs?.Length > 0)
                    {
                        foreach (var headerKv in hkvs)
                        {
                            request.Headers.Add(headerKv.Key, headerKv.Value);
                        }
                    }

                    #region 请求数据
                    var query = await content.ReadAsStringAsync();
                    var buffer = Encoding.UTF8.GetBytes(query);
                    using (var stream = request.GetRequestStream())
                    {
                        await stream.WriteAsync(buffer, 0, buffer.Length);
                    }
                    #endregion

                    using (var response = request.GetResponse())
                    using (var reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                    {
                        result = await reader.ReadToEndAsync();
                        return JsonConvert.DeserializeObject<TResponse>(result);
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("401"))
                    throw;

                TextHelper.Error($"OnHttpRequestPlugins: {requestUri} " + result, ex);
                return default(TResponse);
            }
            finally
            {
                if (request != null)
                {
                    request.Abort();
                    request = null;
                }
            }
        }

        /// <summary>
        /// Post Raw
        /// </summary>
        /// <typeparam name="TResponse"></typeparam>
        /// <param name="requestUri"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public async Task<TResponse> PostAsync<TResponse>(string requestUri, string contentString, string contentType = "application/json", KeyValuePair<string, string>[] hkvs = null)
        {
            return await PostAsync<TResponse>(requestUri, 0, contentString, contentType, hkvs);
        }

        /// <summary>
        /// Post Raw
        /// </summary>
        /// <typeparam name="TResponse"></typeparam>
        /// <param name="requestUri"></param>
        /// <param name="waitSecond"></param>
        /// <param name="contentString"></param>
        /// <param name="contentType"></param>
        /// <param name="hkvs"></param>
        /// <returns></returns>
        public async Task<TResponse> PostAsync<TResponse>(string requestUri, int waitSecond, string contentString, string contentType = "application/json", KeyValuePair<string, string>[] hkvs = null)
        {
            var result = string.Empty;
            var requestUriWithHost = string.Concat(_httpHost, requestUri);
            HttpWebRequest request = default(HttpWebRequest);
            try
            {
                this.SetServerCertificate();

                request = (HttpWebRequest)HttpWebRequest.Create(requestUriWithHost);
                request.Method = "POST";

                #region 代理服务器
                var proxy = GetProxy();
                if (proxy != null)
                {
                    request.Proxy = proxy;
                    request.UseDefaultCredentials = true;
                }
                #endregion

                if (!string.IsNullOrEmpty(contentType))
                    request.ContentType = contentType;
                if (waitSecond > 0)
                    request.Timeout = waitSecond * 1000;
                if (hkvs?.Length > 0)
                {
                    foreach (var headerKv in hkvs)
                    {
                        request.Headers.Add(headerKv.Key, headerKv.Value);
                    }
                }

                #region 请求数据
                var buffer = Encoding.UTF8.GetBytes(contentString);
                using (var stream = request.GetRequestStream())
                {
                    await stream.WriteAsync(buffer, 0, buffer.Length);
                }
                #endregion

                using (var response = await request.GetResponseAsync())
                {
                    using (var reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                    {
                        result = await reader.ReadToEndAsync();
                        return JsonConvert.DeserializeObject<TResponse>(result);
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("401"))
                    throw;

                TextHelper.Error($"OnHttpRequestPlugins: {requestUri} " + result, ex);
                return default(TResponse);
            }
            finally
            {
                if (request != null)
                {
                    request.Abort();
                    request = null;
                }
            }
        }

        #endregion

        #region PostMethod

        /// <summary>
        /// Post Raw
        /// </summary>
        /// <typeparam name="TResponse"></typeparam>
        /// <param name="requestUri"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public TResponse Post<TResponse>(string requestUri, string contentString, string contentType = "application/json", KeyValuePair<string, string>[] hkvs = null)
        {
            return Post<TResponse>(requestUri, 0, contentString, contentType, hkvs);
        }

        /// <summary>
        /// Post Raw
        /// </summary>
        /// <typeparam name="TResponse"></typeparam>
        /// <param name="requestUri"></param>
        /// <param name="waitSecond"></param>
        /// <param name="contentString"></param>
        /// <param name="contentType"></param>
        /// <param name="hkvs"></param>
        /// <returns></returns>
        public TResponse Post<TResponse>(string requestUri, int waitSecond, string contentString, string contentType = "application/json", KeyValuePair<string, string>[] hkvs = null)
        {
            var result = string.Empty;
            var requestUriWithHost = string.Concat(_httpHost, requestUri);
            HttpWebRequest request = default(HttpWebRequest);
            try
            {
                this.SetServerCertificate();

                request = (HttpWebRequest)HttpWebRequest.Create(requestUriWithHost);
                request.Method = "POST";
                #region 代理服务器
                var proxy = GetProxy();
                if (proxy != null)
                {
                    request.Proxy = proxy;
                    request.UseDefaultCredentials = true;
                }
                #endregion


                if (!string.IsNullOrEmpty(contentType))
                    request.ContentType = contentType;
                if (waitSecond > 0)
                    request.Timeout = waitSecond * 1000;
                if (hkvs?.Length > 0)
                {
                    foreach (var headerKv in hkvs)
                    {
                        request.Headers.Add(headerKv.Key, headerKv.Value);
                    }
                }

                #region 请求数据
                var buffer = Encoding.UTF8.GetBytes(contentString);
                using (var stream = request.GetRequestStream())
                {
                    stream.Write(buffer, 0, buffer.Length);
                }
                #endregion

                using (var response = request.GetResponse())
                {
                    using (var reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                    {
                        result = reader.ReadToEnd();
                        return JsonConvert.DeserializeObject<TResponse>(result);
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("401"))
                    throw;

                TextHelper.Error($"OnHttpRequestPlugins: {requestUri} " + result, ex);
                return default(TResponse);
            }
            finally
            {
                if (request != null)
                {
                    request.Abort();
                    request = null;
                }
            }
        }

        /// <summary>
        /// 文件上传
        /// </summary>
        /// <param name="requestUri">地址</param>
        /// <param name="stream">文件流</param>
        /// <param name="fileName">文件名称</param>
        /// <param name="hkvs">参数</param>
        /// <returns></returns>
        public async Task<TResponse> PostAsync<TResponse>(string requestUri, Stream stream, string fileName, KeyValuePair<string, string>[] values = null)
        {
            if (string.IsNullOrEmpty(requestUri) || stream == null)
                return default(TResponse);

            var requestUriWithHost = string.Concat(_httpHost, requestUri);

            using (var client = new HttpClient())
            using (var content = new MultipartFormDataContent())
            {
                var fileContent = new StreamContent(stream);
                fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data");
                fileContent.Headers.ContentDisposition.FileName = fileName;

                foreach (var kv in values)
                {
                    var kvContent = new StringContent(kv.Value);
                    kvContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                    kvContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data");
                    kvContent.Headers.ContentDisposition.Name = kv.Key;
                    content.Add(kvContent);
                    //kvContent.Headers.ContentDisposition.
                }
                content.Add(fileContent);
                try
                {
                    var result = await client.PostAsync(requestUriWithHost, content);
                    if (result.StatusCode.ToString() != "OK")
                        return default(TResponse);

                    var json = await result.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<TResponse>(json);
                }
                catch
                {
                    return default(TResponse);
                }
            }
        }
        #endregion

        #region 私有方法
        /// <summary>
        /// 获得代理信息对象
        /// </summary>
        /// <returns></returns>
        private WebProxy GetProxy()
        {
            //if (_proxyConfig == null || !_proxyConfig.IsOpen)
            //    return null;

            //var proxy = new WebProxy
            //{
            //    Address = new Uri($"http://{_proxyConfig.Ip}:{_proxyConfig.Port}")
            //};
            //if (!string.IsNullOrEmpty(_proxyConfig.UserName))
            //    proxy.Credentials = new NetworkCredential(_proxyConfig.UserName, _proxyConfig.Password);

            //return proxy;
            return null;
        }

        private void SetServerCertificate()
        {
            if (_httpHost.StartsWith("https", StringComparison.OrdinalIgnoreCase))   //https请求
            {
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
            }
        }
        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true; //总是接受  
        }

        #endregion

        #region 其他
        ///// <summary>
        ///// 处理
        ///// </summary>
        ///// <typeparam name="TResponse"></typeparam>
        ///// <param name="action"></param>
        ///// <param name="request"></param>
        ///// <returns></returns>
        //public async Task<TResponse> Request<TResponse>(Func<BaseRequest, TResponse> action, BaseRequest request)
        //{
        //    try
        //    {
        //        if (action != null)
        //            return action.Invoke(request);

        //        return default(TResponse);
        //    }
        //    catch (Exception ex)
        //    {
        //        LogHelper.Error(ex.Message, ex);
        //    }
        //    return default(TResponse);
        //}
        #endregion
    }
}
