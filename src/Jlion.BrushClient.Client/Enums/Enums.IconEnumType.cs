using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Jlion.BrushClient.Client.Enums
{
    public partial class OptionEnums
    {
        /// <summary>
        /// 图标 
        /// </summary>
        public enum IconEnumType
        {
            /// <summary>
            /// 更新图标
            /// </summary>
            [Description("..\\resource\\image\\exe.ico")]
            ExeIcon = 0,

            /// <summary>
            /// 更新图标
            /// </summary>
            [Description("..\\resource\\image\\icon-upgrade.png")]
            UpgradeIcon = 1,

            /// <summary>
            /// 检测客户端完整性成功图标
            /// </summary>
            [Description("..\\resource\\image\\icon-client-check.png")]
            CheckClientIconSuccess = 2,

            /// <summary>
            /// 检测客户端完整性失败图标
            /// </summary>
            [Description("..\\resource\\image\\icon-client-check-fail.png")]
            CheckClientIconFail = 3,

            /// <summary>
            /// 检测网络成功图标
            /// </summary>
            [Description("..\\resource\\image\\icon-internet-check.png")]
            CheckInternetIconSuccess = 4,

            /// <summary>
            /// 检测网络失败图标
            /// </summary>
            [Description("..\\resource\\image\\icon-internet-check-fail.png")]
            CheckInternetIconFail = 5,
        }

        /// <summary>
        /// OEM 图片地址
        /// </summary>
        public enum ImageOemEnumType
        {
            [Description("resource//image//icon-logo-max{0}.png")]
            LoginImage,

            [Description("resource//image//main-logo{0}.png")]
            MainLogo,

            [Description("resource//image//exe.ico")]
            MenuLogo,

            [Description("resource//image//exe-oem.ico")]
            MenuOemLogo,
        }
    }
}
