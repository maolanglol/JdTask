using System;
using System.Collections.Generic;
using System.Text;

namespace Jlion.BrushClient.Application.Model
{

    public class SystemSettingsResponse
    {
        public long Id { set; get; }

        /// <summary>
        /// 转化率
        /// </summary>
        public decimal Rate { set; get; }

        /// <summary>
        /// 会员提现手续费
        /// </summary>
        public decimal HandleFee { set; get; }

        /// <summary>
        /// 代理商提现手续费
        /// </summary>
        public decimal AgentHandleFee { set; get; }

        /// <summary>
        /// 代理商分成利润比
        /// </summary>
        public decimal AgentProfit { set; get; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { set; get; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime UpdateTime { set; get; }
    }
}
