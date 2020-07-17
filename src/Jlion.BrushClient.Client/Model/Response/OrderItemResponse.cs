using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jlion.BrushClient.Application.Model.Response;
using Jlion.BrushClient.Framework;
using WP.Device.Plugins.Kernel;

namespace Jlion.BrushClient.Client.Model.Response
{
    public class OrderItemResponse
    {
        /// <summary>
        /// 订单编号
        /// </summary>
        public string OutTradeNo { get; set; }

        /// <summary>
        /// 订单总金额(应收总金额或者退款总金额)
        /// </summary>
        public decimal TotalFee { get; set; }


        /// <summary>
        /// 支付方式（1、微信 2、支付宝 3、银联刷卡（pos机刷卡））
        /// </summary>
        public string PayTypeName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int PayType { set; get; }

        /// <summary>
        /// 订单类型
        /// </summary>
        public int OrderType { set; get; }

        /// <summary>
        /// 收银员名称
        /// </summary>
        public string CashName { get; set; }

        /// <summary>
        /// 交易状态
        /// </summary>
        public string StateName { get; set; }

        public int State { set; get; }

        /// <summary>
        /// 交易时间
        /// </summary>
        public string PayTime { get; set; }

        /// <summary>
        /// 门店编号
        /// </summary>
        public int StoresId { get; set; }

        /// <summary>
        /// 实收金额
        /// </summary>
        public decimal ReceiptAmt { set; get; }


        public static OrderItemResponse ToResponse(OrderPageResponse data)
        {
            if (data == null)
                return null;

            return new OrderItemResponse()
            {
                ReceiptAmt = (data.ReceiptAmt+0.00M),
                CashName = data.CashName,
                PayTypeName = data.PayType.ToPayType().GetDescription(),
                PayType = data.PayType,
                OrderType = data.OrderType,
                OutTradeNo = data.OutTradeNo,
                PayTime = data.PayTime.ToString("yyyy-MM-dd HH:mm:ss"),
                StateName = data.State.ToClientOrderStatus().GetDescription(),
                State = data.State,
                StoresId = data.StoreId,
                TotalFee = (data.TotalAmt + 0.00M),
            };
        }

        //public static OrderItemResponse ToResponse(OrderPageResponse data)
        //{
        //    if (data == null)
        //        return null;

        //    return new OrderItemResponse()
        //    {
        //        Amount = $"{data.TotalAmt}",
        //        CashName = "",
        //        OrderType = data.PayType.ToClientPayType().GetDescription(),
        //        OutTradeNo = data.OtNo,
        //        PayTime = data.EndTime.ToString("yyyy-MM-dd HH:mm"),
        //        State = data.State.ToClientOrderStatus().GetDescription(),
        //        StoresId = data.StoreId,
        //        TotalFee = data.TotalAmt
        //    };
        //}
    }
}
