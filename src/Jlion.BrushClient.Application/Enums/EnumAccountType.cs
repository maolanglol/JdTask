using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Jlion.BrushClient.Application.Enums
{
    public enum EnumAccountType
    {

        /// <summary>
        /// 代理商
        /// </summary>
        [Description("代理商")]
        Agent = 2,

        /// <summary>
        /// 会员
        /// </summary>
        [Description("会员")]
        User = 3,
    }
}
