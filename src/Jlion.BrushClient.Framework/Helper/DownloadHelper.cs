using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Jlion.BrushClient.Framework.Helper
{
    /// <summary>
    /// 文件下载帮助类
    /// </summary>
    public class DownloadHelper
    {
        /// <summary>
        /// 从URL地址下载文件到本地磁盘
        /// </summary>
        /// <param name="fileName">保存得地址</param>
        /// <param name="url">远程Url</param>
        /// <param name="processAction">回调</param>
        /// <param name="proxyIp">代理ip 默认从upgrade.ini 中获取</param>
        /// <param name="proxyPort">代理端口  默认从upgrade.ini 中获取</param>
        /// <returns></returns>
        public static bool DownloadFromUrl(string fileName, string url, Action<double> processAction, string proxyIp = "", string proxyPort = "")
        {
            WebResponse response = null;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            try
            {
                #region 代理服务器
                var proxy = GetProxy(proxyIp, proxyPort);
                if (proxy != null)
                {
                    request.Proxy = proxy;
                    request.UseDefaultCredentials = true;
                }
                #endregion

                response = request.GetResponse();
                using (var stream = response.GetResponseStream())
                {
                    return SaveBinaryFile(response, fileName, processAction);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                request.Abort();
                request = null;
            }
        }

        /// <summary>
        /// Save a binary file to disk.
        /// </summary>
        /// <param name="response">The response used to save the file</param>
        // 将二进制文件保存到磁盘
        private static bool SaveBinaryFile(WebResponse response, string FileName, Action<double> processAction)
        {
            bool Value = true;
            byte[] buffer = new byte[response.ContentLength];

            try
            {
                if (File.Exists(FileName))
                    File.Delete(FileName);

                using (var outStream = System.IO.File.Create(FileName))
                {
                    using (var inStream = response.GetResponseStream())
                    {
                        int l;
                        var totalDownLength = 0;
                        do
                        {
                            l = inStream.Read(buffer, 0, buffer.Length);
                            if (l > 0)
                            {
                                totalDownLength += l;
                                var percent = Convert.ToDouble(totalDownLength) / Convert.ToDouble(response.ContentLength);
                                outStream.Write(buffer, 0, l);
                                processAction?.Invoke(percent);
                            }
                            else
                            {
                                processAction?.Invoke(1);
                            }
                        }
                        while (l > 0);
                    }
                }
            }
            catch(Exception ex)
            {
                TextHelper.Error($"下载异常: message:{ex.Message}",ex);
                Value = false;
            }
            return Value;
        }

        /// <summary>
        /// 获得代理信息对象
        /// </summary>
        /// <returns></returns>
        private static WebProxy GetProxy(string ip = "", string port = "")
        {
            ip = string.IsNullOrEmpty(ip) ? ServiceIniUpgradeHelper.ReadProxyIp() : ip;
            port = string.IsNullOrEmpty(port) ? ServiceIniUpgradeHelper.ReadProxyPort() : port;
            if (string.IsNullOrEmpty(ip) || string.IsNullOrEmpty(port))
                return null;

            var proxy = new WebProxy
            {
                Address = new Uri($"http://{ip}:{port}")
            };
            //if (!string.IsNullOrEmpty(_proxyConfig.UserName))
            //    proxy.Credentials = new NetworkCredential(_proxyConfig.UserName, _proxyConfig.Password);

            return proxy;
        }
    }
}
