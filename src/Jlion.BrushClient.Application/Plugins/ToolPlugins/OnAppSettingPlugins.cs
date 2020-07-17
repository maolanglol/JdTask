using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jlion.BrushClient.Application.Model;
using Jlion.BrushClient.Framework;

namespace Jlion.BrushClient.Application.Plugins
{
    /// <summary>
    /// 软件系统级别配置 不可修改
    /// </summary>
    public class OnAppSettingPlugins : OnBasePlugins
    {
        public OnAppSettingPlugins()
        {
        }

        /// <summary>
        /// 插件网关系统
        /// </summary>
        public string PluginsHost
        {
            get
            {
                var host= ConfigurationManager.AppSettings["host"].ToString() ?? "";
                if (string.IsNullOrWhiteSpace(host))
                    return "http://121.196.143.80:8003/";
                return host;
            }
        }

        public decimal Opacity
        {
            get {
                try
                {
                    var opactiy = ConfigurationManager.AppSettings["opacity"].ToString() ?? "1.0";
                    return opactiy.ToDecimalOrDefault(1.0M);
                }
                catch (Exception ex)
                {
                    return 1;
                }
            }
        }

        /// <summary>
        /// 版本信息
        /// </summary>
        public string Version
        {
            get
            {
                return ConfigurationManager.AppSettings["version"].ToString() ?? "";
            }
        }

        /// <summary>
        /// 版本信息
        /// </summary>
        public int VersionCode
        {
            get
            {
                //1.1.1->001001001
                var versionConfig = Version;
                if (Version.Contains("-"))
                {
                    versionConfig = Version.Split('-')[0];
                }
                var versionList = versionConfig.Split('.').ToList();
                var versionStr = "";
                versionList?.ForEach(item =>
                {
                    versionStr += item.PadLeft(3, '0');
                });

                //var version = Version.RemoveChar('.').PadRight(6,'0');
                var code = int.TryParse(versionStr, out int result);
                return result;
            }
        }
    }
}
