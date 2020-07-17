using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Jlion.BrushClient.Application.Enums
{
    /// <summary>
    /// 审核状态
    /// </summary>
    public enum EnumWithdrawStatus
    {
        /// <summary>
        /// 审核中
        /// </summary>
        [Description("审核中")]
        Auditing = 5,

        /// <summary>
        /// 审核通过
        /// </summary>
        [Description("审核通过")]
        Pass = 10,

        /// <summary>
        /// 提现成功
        /// </summary>
        [Description("提现成功")]
        Success = 15,

        /// <summary>
        /// 审核不通过
        /// </summary>
        [Description("审核不通过")]
        NoPass = 20
    }
}
