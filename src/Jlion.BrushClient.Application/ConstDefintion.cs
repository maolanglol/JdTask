using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jlion.BrushClient.Application
{
    /// <summary>
    /// 常量定义
    /// </summary>
    public class ConstDefintion
    {
        #region 支付网关地址定义
        /// <summary>
        /// 授权登陆
        /// </summary>
        public const string Auth_Url = "api/Login";

        /// <summary>
        /// 注册用户
        /// </summary>
        public const string User_Reg_Url = "api/Login/reg";

        /// <summary>
        /// 产品列表
        /// </summary>
        public const string Product_Query_Url = "api/Products";

        /// <summary>
        /// 提现Api
        /// </summary>
        public const string Withdraw_Url = "api/Withdraw";

        /// <summary>
        /// 账户Api
        /// </summary>
        public const string Account_Url = "api/Account";

        /// <summary>
        /// 系统设置API
        /// </summary>
        public const string Settings_Url = "api/Settings";

        /// <summary>
        /// 账户记录api
        /// </summary>
        public const string AcccountRecord_Url = "api/AcccountRecord";

        /// <summary>
        /// 获取任务
        /// </summary>
        public const string Task_Url = "api/Task";

        public const string Pay_Url = "api/Pay";

        #endregion

        #region 注册表信息
        /// <summary>
        /// Windows 机器GUID 注册码路径
        /// </summary>
        public const string WindowMachinePath = @"SOFTWARE\Microsoft\Cryptography";

        /// <summary>
        /// 用户桌面
        /// </summary>
        public const string LocalUserPath = @"Control Panel\Desktop";

        /// <summary>
        /// Windows 机器GUID Key
        /// </summary>
        //[Obsolete("已经过时")]
        //public const string WindowMachineName = "MachineGuid";
        #endregion
    }
}
