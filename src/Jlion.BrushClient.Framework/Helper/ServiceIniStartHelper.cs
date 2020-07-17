using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jlion.BrushClient.Framework.Enums;
using Jlion.BrushClient.Framework.Model;

namespace Jlion.BrushClient.Framework.Helper
{
    /// <summary>
    /// 升级组件相关配置文件
    /// </summary>
    public class ServiceIniStartHelper
    {
        #region 常量定义
        private const string _baseInfo = "baseInfo";
        #endregion

        #region 写入
        /// <summary>
        /// 写入启动
        /// </summary>
        /// <param name="startProcess"></param>
        public static void WriteStartProcess(string startProcess)
        {
            ServiceIniHelper.Write(_baseInfo, "startProcess", startProcess,fileType: EnumIniFileType.Start);
        }

        #endregion

        #region 读取
        
        /// <summary>
        /// 读取
        /// </summary>
        /// <param name="sectionName"></param>
        /// <returns></returns>
        public static string ReadStartProcess(string sectionName=_baseInfo)
        {

            if (string.IsNullOrWhiteSpace(sectionName))
                throw new ArgumentNullException("sectionName value is not null");

            return ServiceIniHelper.Read(sectionName, "startProcess", fileType: EnumIniFileType.Start);
        }

        #endregion
    }
}
