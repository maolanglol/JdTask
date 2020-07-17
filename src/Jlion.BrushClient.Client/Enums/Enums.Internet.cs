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
        /// 网络枚举
        /// </summary>
        public enum EnumInternetType
        {
            /// <summary>
            /// 网络正常
            /// </summary>
            [Description("..\\resource\\image\\wifi-online@2x.png")]
            Online = 0,

            /// <summary>
            /// 网络异常
            /// </summary>
            [Description("..\\resource\\image\\wifi-offline@2x.png")]
            Offline = 1,
        }

        /// <summary>
        /// 检测结果枚举
        /// </summary>
        public enum EnumCheckType
        {
            [Description("..\\resource\\image\\icon-check-success.png")]
            OK,
            [Description("..\\resource\\image\\icon-check-fail.png")]
            ERROR
        }
    }
}
