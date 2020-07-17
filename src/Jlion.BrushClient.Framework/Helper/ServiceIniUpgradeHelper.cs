using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jlion.BrushClient.Framework.Model;

namespace Jlion.BrushClient.Framework.Helper
{
    /// <summary>
    /// 升级组件相关配置文件
    /// </summary>
    public class ServiceIniUpgradeHelper
    {
        #region 常量定义
        private const string _baseInfo = "baseInfo";
        #endregion

        #region 写入
        private static void WriteModuleUrl(string sectionName, string value)
        {
            if (sectionName.Equals(_baseInfo))
                throw new ArgumentNullException("sectionName value is a built-in definition module");

            ServiceIniHelper.Write(sectionName, "url", value);
        }

        /// <summary>
        /// 下入该模块对应本地得文件名称
        /// </summary>
        /// <param name="sectionName"></param>
        /// <param name="value"></param>
        private static void WriteModuleFileName(string sectionName, string value)
        {
            if (sectionName.Equals(_baseInfo))
                throw new ArgumentNullException("sectionName value is a built-in definition module");

            ServiceIniHelper.Write(sectionName, "fileName", value);
        }

        private static void WriteModuleDescription(string sectionName, string value)
        {
            if (sectionName.Equals(_baseInfo))
                throw new ArgumentNullException("sectionName value is a built-in definition module");

            ServiceIniHelper.Write(sectionName, "description", value);
        }


        /// <summary>
        /// 写入是否是主程序
        /// </summary>
        private static void WriteModuleIsMainProcess(string sectionName, bool isMainProcess)
        {
            if (sectionName.Equals(_baseInfo))
                throw new ArgumentNullException("sectionName value is a built-in definition module");

            ServiceIniHelper.Write(sectionName, "isMainProcess", isMainProcess ? "1" : "0");
        }

        /// <summary>
        /// 写入升级模块信息
        /// </summary>
        /// <param name="sectionName"></param>
        /// <param name="model"></param>
        public static void WriteModuleInfo(string sectionName, UpgradeModel model)
        {
            if (sectionName.Equals(_baseInfo))
                throw new ArgumentNullException("sectionName value is a built-in definition module");
            if (model == null)
                throw new ArgumentNullException("mode is not null");

            WriteModuleUrl(sectionName, model.Url);
            WriteModuleDescription(sectionName, model.Description);
            WriteModuleFileName(sectionName, model.FileName);
            WriteModuleIsMainProcess(sectionName, model.IsMainProcess);

        }

        /// <summary>
        /// 写入升级标题
        /// </summary>
        /// <param name="title"></param>
        public static void WriteTitle(string title)
        {
            ServiceIniHelper.Write(_baseInfo, "title", title);
        }

        /// <summary>
        /// 写入当前得进程文件名称,避免存在文件被重新命名问题
        /// </summary>
        /// <param name="exeName"></param>
        public static void WriteExeName(string exeName)
        {
            ServiceIniHelper.Write(_baseInfo, "exeName", exeName);
        }

        /// <summary>
        /// 写入升级描述内容
        /// </summary>
        /// <param name="upgradeDescription"></param>
        public static void WriteUpgradeDescription(string upgradeDescription)
        {
            upgradeDescription = DESHelper.DESEncrypt(upgradeDescription);
            ServiceIniHelper.Write(_baseInfo, "description", upgradeDescription);
        }

        /// <summary>
        /// 写入更新得模块列表,多个用|号隔开
        /// </summary>
        /// <param name="module"></param>
        public static void WriteModuleList(string module)
        {
            ServiceIniHelper.Write(_baseInfo, "moduleList", module);
        }

        //public static void WriteStartProcess(string startProcess)
        //{
        //    ServiceIniHelper.Write(_baseInfo, "startProcess", startProcess);
        //}

        #region 代理服务器
        /// <summary>
        /// 写入代理服务器Ip地址
        /// </summary>
        /// <param name="ip"></param>
        public static void WriteProxyIp(string ip)
        {
            ServiceIniHelper.Write(_baseInfo, "ip", ip);
        }

        /// <summary>
        /// 写入代理服务器端口地址
        /// </summary>
        /// <param name="port"></param>
        public static void WriteProxyPort(string port)
        {
            ServiceIniHelper.Write(_baseInfo, "port", port);
        }

        /// <summary>
        /// 写入代理服务器Ip地址
        /// </summary>
        /// <param name="ip"></param>
        public static string ReadProxyIp()
        {
            return ServiceIniHelper.Read(_baseInfo, "ip");
        }

        /// <summary>
        /// 写入代理服务器端口地址
        /// </summary>
        /// <param name="port"></param>
        public static string ReadProxyPort()
        {
            return ServiceIniHelper.Read(_baseInfo, "port");
        }
        #endregion

        #endregion

        #region 读取
        /// <summary>
        /// 读取升级地址
        /// </summary>
        /// <param name="sectionName"></param>
        /// <param name="value"></param>
        public static string ReadModuleUrl(string sectionName)
        {
            if (sectionName.Equals(_baseInfo))
                throw new ArgumentNullException("sectionName value is a built-in definition module");

            return ServiceIniHelper.Read(sectionName, "url");
        }

        /// <summary>
        /// 下入该模块对应本地得文件名称
        /// </summary>
        /// <param name="sectionName"></param>
        /// <param name="value"></param>
        public static string ReadModuleFileName(string sectionName)
        {
            if (sectionName.Equals(_baseInfo))
                throw new ArgumentNullException("sectionName value is a built-in definition module");

            return ServiceIniHelper.Read(sectionName, "fileName");
        }

        //public static string ReadStartProcess(string sectionName=_baseInfo)
        //{

        //    if (string.IsNullOrWhiteSpace(sectionName))
        //        throw new ArgumentNullException("sectionName value is not null");

        //    return ServiceIniHelper.Read(sectionName, "startProcess");
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sectionName"></param>
        /// <param name="value"></param>
        public static string ReadModuleDescription(string sectionName)
        {
            if (sectionName.Equals(_baseInfo))
                throw new ArgumentNullException("sectionName value is a built-in definition module");

            return ServiceIniHelper.Read(sectionName, "description");
        }

        /// <summary>
        /// 写入升级标题
        /// </summary>
        /// <param name="title"></param>
        public static string ReadTitle()
        {
            return ServiceIniHelper.Read(_baseInfo, "title");
        }

        /// <summary>
        /// 写入升级描述内容
        /// </summary>
        /// <param name="upgradeDescription"></param>
        public static string ReadUpgradeDescription()
        {
            var description = ServiceIniHelper.Read(_baseInfo, "description");
            try
            {
                return DESHelper.DESDecrypt(description);
            }
            catch { }
            return description;
        }

        /// <summary>
        /// 获得需要升级得主进程文件名
        /// </summary>
        /// <returns></returns>
        public static string ReadExeName()
        {
            return ServiceIniHelper.Read(_baseInfo, "exeName");
        }

        /// <summary>
        /// 读取是否是主程序
        /// </summary>
        public static bool ReadModuleIsMainProcess(string sectionName)
        {
            if (sectionName.Equals(_baseInfo))
                throw new ArgumentNullException("sectionName value is a built-in definition module");

            var isMainProcess = ServiceIniHelper.Read(sectionName, "isMainProcess");
            return isMainProcess.Equals("1");
        }

        /// <summary>
        /// 写入更新得模块列表,多个用|号隔开
        /// </summary>
        /// <param name="module"></param>
        public static List<string> ReadModuleList()
        {
            var listStr = ServiceIniHelper.Read(_baseInfo, "moduleList");
            if (string.IsNullOrEmpty(listStr))
                return null;
            if (listStr.Contains("|"))
                return listStr.Split('|').ToList();

            return new List<string>() {
                listStr
            };
        }

        /// <summary>
        /// 获得需要更新得模块信息
        /// </summary>
        /// <returns></returns>
        public static List<UpgradeModel> GetUpgradeModule()
        {
            var resp = new List<UpgradeModel>();
            try
            {
                var list = ServiceIniUpgradeHelper.ReadModuleList();
                list?.ForEach(item =>
                {
                    var response = new UpgradeModel();
                    response.Url = ServiceIniUpgradeHelper.ReadModuleUrl(item);
                    response.FileName = ServiceIniUpgradeHelper.ReadModuleFileName(item);
                    response.Description = ServiceIniUpgradeHelper.ReadModuleDescription(item);
                    response.IsMainProcess = ServiceIniUpgradeHelper.ReadModuleIsMainProcess(item);
                    resp.Add(response);
                });
            }
            catch (Exception ex)
            {
                TextHelper.Error("GetUpgradeModule 异常", ex);
            }
            return resp;
        }
        #endregion
    }
}
