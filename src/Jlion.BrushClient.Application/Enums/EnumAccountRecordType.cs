using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Jlion.BrushClient.Application.Enums
{
    /// <summary>
    /// 审核状态
    /// </summary>
    public enum EnumAccountRecordType
    {
        All = 0,

        /// <summary>
        /// 任务分成
        /// </summary>
        [Description("任务奖励")]
        Task = 1,

        /// <summary>
        /// 佣金
        /// </summary>
        [Description("佣金")]
        Commission = 2,

        /// <summary>
        /// 提现
        /// </summary>
        [Description("提现")]
        Withdraw = 3,

        /// <summary>
        /// 提现退回
        /// </summary>
        [Description("提现退回")]
        WithdrawFail=4,
    }
}
