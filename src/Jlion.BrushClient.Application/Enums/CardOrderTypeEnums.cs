using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Jlion.BrushClient.Application.Enums
{
    public enum CardOrderTypeEnums
    {
        /// <summary>
        /// 全部
        /// </summary>
        All = 0,

        /// <summary>
        /// 充值
        /// </summary>
        [Description("充值")]
        Recharge = 1,

        /// <summary>
        /// 赠送
        /// </summary>
        [Description("赠送")]
        Give = 2,

        /// <summary>
        /// 消费
        /// </summary>
        [Description("消费")]
        Consumer = 3,
        
        /// <summary>
        /// 退款
        /// </summary>
        [Description("退款")]
        Refund = 4
    }
}
