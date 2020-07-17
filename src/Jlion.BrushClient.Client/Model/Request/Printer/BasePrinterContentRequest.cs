using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jlion.BrushClient.Client.Model
{
    public class BasePrinterContentRequest
    {
        /// <summary>
        /// 订单总金额
        /// </summary>
        public decimal TotalAmount { set; get; }

        /// <summary>
        /// 支付金额
        /// </summary>
        public decimal DiscountMoney
        {
            get
            {
                return TotalAmount - ReceiptAmount;
            }
        }

        /// <summary>
        /// 实收金额
        /// </summary>
        public decimal ReceiptAmount { set; get; }

        /// <summary>
        /// 支付方式
        /// </summary>
        public string PayType { set; get; }

        /// <summary>
        /// 是否是储蓄卡
        /// </summary>
        public bool IsCard { set; get; }

        /// <summary>
        /// 订单时间
        /// </summary>
        public string TradeNo { set; get; }

        /// <summary>
        /// 交易时间
        /// </summary>
        public DateTime TradeTime { set; get; }

        /// <summary>
        /// 是否补打
        /// </summary>
        public bool IsRepair { set; get; }

    }
}
