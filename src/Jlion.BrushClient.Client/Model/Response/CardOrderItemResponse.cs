using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jlion.BrushClient.Client.Model.Response
{
    public class CardOrderItemResponse
    {
        /// <summary>
        /// 消费金额
        /// </summary>
        public decimal TotalAmount { set; get; }

        /// <summary>
        /// 赠送金额
        /// </summary>
        public string GiveAmount { set; get; }

        /// <summary>
        /// 交易类型
        /// </summary>
        public int Type { set; get; }

        /// <summary>
        /// 交易类型名称
        /// </summary>
        public string TypeName { set; get; }

        /// <summary>
        /// 订单号
        /// </summary>
        public string OrderNo { set; get; }

        /// <summary>
        /// 订单交易时间
        /// </summary>
        public string OrderTradeTime { set; get; }

        /// <summary>
        /// 可退金额
        /// </summary>
        public decimal CanRefundAmt { set; get; }
    }
}
