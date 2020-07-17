using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Jlion.BrushClient.Framework.Enums
{
    /// <summary>
    /// 升级模块
    /// </summary>
    public enum UpgradeModuleEnums
    {
        /// <summary>
        /// 全部
        /// </summary>
        [Description("")]
        ALL = 0,

        /// <summary>
        /// 主安装包模块
        /// </summary>
        [Description("com.wp.plugins")]
        MAINModule = 1,


        /// <summary>
        /// 程序自身得exe 配置
        /// </summary>
        [Description("WP.Device.Plugins.Client.exe.config")]
        CLIENTEXECONFIG = 3,

        /// <summary>
        /// 程序自身得exe
        /// </summary>
        [Description("WP.Device.Plugins.Client.exe")]
        CLIENTEXE = 4,
        
        /// <summary>
        /// Device.Config
        /// </summary>
        [Description("Device.config")]
        DEVICECONFIG = 5,

        /// <summary>
        /// StartClient
        /// </summary>
        [Description("WP.Device.Plugins.StartClient.exe")]
        STARTClIENT = 6,

        /// <summary>
        /// StartClient.CONFIG
        /// </summary>
        [Description("WP.Device.Plugins.StartClient.exe.config")]
        STARTClIENTCONFIG = 7,

        #region 升级相关组件
        /// <summary>
        /// 升级组件得 exe
        /// </summary>
        [Description("Wp.Device.UpgradeClient.exe")]
        UPGRADECLIENTEXT = 80,

        /// <summary>
        /// 升级组件得config
        /// </summary>
        [Description("Wp.Device.UpgradeClient.exe.config")]
        UPGRADECLIENTEXTCONFIG = 81,

        /// <summary>
        /// 升级组件基础DLL
        /// </summary>
        [Description("Jlion.BrushClient.Framework.dll")]
        UPGRADEFRAMEWORK = 82,

        #endregion

        #region 系统级别组件配置
        /// <summary>
        /// 系统级别配置
        /// </summary>
        [Description("System.config")]
        SYSTEMCONFIG = 100,
        /// <summary>
        /// OCR 训练库
        /// </summary>
        [Description("Traindata.xml")]
        OCRTRAINDATA = 101,

        /// <summary>
        /// 图片处理算法库
        /// </summary>
        [Description("ImageProcess.Json")]
        IMAGEPROCESSJSON = 102,
        #endregion

    }

    /// <summary>
    /// 更新状态
    /// </summary>
    public enum UpgradeModuleStatusEnums
    {
        /// <summary>
        /// 不需要更新
        /// </summary>
        NONE = 0,

        /// <summary>
        /// 需要更新
        /// </summary>
        Need = 5,

        /// <summary>
        /// 强制更新
        /// </summary>
        Force = 10
    }
}
