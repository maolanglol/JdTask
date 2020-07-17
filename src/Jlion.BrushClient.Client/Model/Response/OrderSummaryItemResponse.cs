using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jlion.BrushClient.Framework;
using static Jlion.BrushClient.Client.Enums.OptionEnums;

namespace Jlion.BrushClient.Client.Model.Response
{
    public class OrderSummaryResponse
    {

        public OrderSummaryResponse()
        {
            this.List = new List<OrderSummaryItemResponse>();
            this.TotalSummary = new OrderSummaryItemResponse();
        }

        /// <summary>
        /// 总的收款信息
        /// </summary>
        public OrderSummaryItemResponse TotalSummary { set; get; }

        /// <summary>
        /// 净收入(实收金额-实退金额)
        /// </summary>
        public decimal IncomeMoney
        {
            get
            {
                return this.TotalSummary.RealReceiptMoney - this.TotalSummary.RealRefundMoney;
            }
        }

        /// <summary>
        /// 上次交班时间
        /// </summary>
        public string PreSettledTime { set; get; }

        /// <summary>
        /// 是否可以补打小票
        /// </summary>
        public bool CanPatchTicket { set; get; }

        /// <summary>
        /// 支付详细列表
        /// </summary>
        public List<OrderSummaryItemResponse> List { set; get; }

        /// <summary>
        /// 搜索条件
        /// </summary>
        public SearchItemResquest Search { set; get; }
    }

    public class SearchItemResquest
    {
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartTime { set; get; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndTime { set; get; }
    }

    /// <summary>
    /// 订单汇总
    /// </summary>
    public class OrderSummaryItemResponse
    {
        /// <summary>
        /// 收款类型
        /// </summary>
        public EnumGatewayPayType PayType { set; get; }

        /// <summary>
        /// 图标
        /// </summary>
        public string PayIcon
        {
            get
            {
                return $"..\\resource\\image\\icon-{PayType.ToString().ToLower()}.png";
            }
        }

        /// <summary>
        /// 收款类型名称
        /// </summary>
        public string PayTypeName
        {
            get
            {
                return PayType.GetDescription();
            }
        }


        /// <summary>
        /// 收款金额
        /// </summary>
        public decimal ReceiptMoney { set; get; }

        /// <summary>
        /// 实收金额(不减去退款的金额)
        /// </summary>
        public decimal RealReceiptMoney { set; get; }

        /// <summary>
        /// 优惠金额
        /// </summary>
        public decimal DiscountsMoney { set; get; }

        /// <summary>
        /// 收款笔数
        /// </summary>
        public int ReceiptCount { set; get; }

        /// <summary>
        /// 退款金额
        /// </summary>
        public decimal RefundMoney { set; get; }

        /// <summary>
        /// 实退金额
        /// </summary>
        public decimal RealRefundMoney { set; get; }

        /// <summary>
        /// 退款笔数
        /// </summary>
        public int RefundCount { set; get; }

        /// <summary>
        /// 储值卡收款金额
        /// </summary>
        public decimal StoreTotalFee { set; get; }

        /// <summary>
        /// 储值卡收款笔数
        /// </summary>
        public int StoreTotalCount { set; get; }

        /// <summary>
        /// 储值卡退款金额
        /// </summary>
        public decimal StoreRefundTotalFee { set; get; }

        /// <summary>
        /// 储值卡退款笔数
        /// </summary>
        public int StoreRefundTotalCount { set; get; }
    }
}
