//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using static Jlion.BrushClient.Client.Enums.OptionEnums;

//namespace Jlion.BrushClient.Client.Model
//{
//    public class OrderDataRequest : PagerBaseRequest
//    {
//        /// <summary>
//        /// 收银员编号
//        /// </summary>
//        public int CashId { set; get; }

//        /// <summary>
//        /// 交易状态(-1 全部（默认） 、8、退款成功 10、支付成功 )
//        /// </summary>
//        public EnumOrderStatus State { get; set; }

//        /// <summary>
//        /// 支付方式
//        /// </summary>
//        public EnumGatewayPayType PayType { get; set; }

//        /// <summary>
//        /// 交易开始时间（默认今天） 注意：交易时间不允许跨月选择
//        /// </summary>
//        public DateTime PayStartTime { get; set; } = Convert.ToDateTime(DateTime.Now.ToShortDateString() + " 00:00:00");

//        /// <summary>
//        /// 交易结束时间（默认今天） 注意：交易时间不允许跨月选择
//        /// </summary>
//        public DateTime PayEndTime { get; set; } = Convert.ToDateTime(DateTime.Now.ToShortDateString() + " 23:59:59");

//        /// <summary>
//        /// 门店编号
//        /// </summary>
//        public int StoresId { get; set; }

//        /// <summary>
//        /// 订单号
//        /// </summary>
//        public string OrderNo { set; get; }
//    }
//}
