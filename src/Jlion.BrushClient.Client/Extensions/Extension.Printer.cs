//using System;
//using System.Collections.Generic;
//using System.Drawing;
//using System.Linq;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;
//using Jlion.BrushClient.Application.Model;
//using Jlion.BrushClient.Client.Model;
//using Jlion.BrushClient.Client.Model.Response;
//using Jlion.BrushClient.Framework;
//using WP.Device.Plugins.Kernel;
//using Jlion.BrushClient.UControl;
//using static Jlion.BrushClient.Client.Enums.OptionEnums;

//namespace Jlion.BrushClient.Client
//{
//    /// <summary>
//    /// 打印扩展方法
//    /// </summary>
//    public static partial class Extensions
//    {
//        #region 收银小票模板(收银系统转发过来)
//        /// <summary>
//        /// 打印模板扩展
//        /// </summary>
//        /// <param name="printer"></param>
//        /// <param name="content"></param>
//        public static void PrintContent(this IBasePrinter printer, string content)
//        {
//            var list = content.Split('\n');
//            var index = 0;
//            foreach (var item in list)
//            {
//                var itemStr = item.RemoveChar('\r');
//                if (itemStr.Equals(" ") || itemStr.Equals(""))
//                {
//                    printer.AddLine();
//                }
//                else
//                {
//                    index++;
//                    if (index == 1)
//                    {
//                        itemStr = itemStr.RemoveChar(' ');
//                        printer.AddTitleLine(itemStr, FontSize.Large);
//                        continue;
//                    }
//                    printer.AddTextLine(itemStr, FontSize.NormalSmall);
//                }
//            }
//        }
//        #endregion

//        #region 收款凭证模板
//        /// <summary>
//        /// 打印模板扩展
//        /// </summary>
//        /// <param name="printer">打印对象</param>
//        /// <param name="storeConfig">门店相关信息对象</param>
//        /// <param name="orderModel">订单相关信息对象</param>
//        /// <param name="isRefund">是否退款</param>
//        /// <param name="isRepair">是否补打</param>
//        public static void PrintTemplate(this IBasePrinter printer, MerchantResponse merchant, ReceiveContentRequest request)
//        {
//            if (printer == null)
//                throw new ArgumentNullException("printer is not null");
//            if (merchant == null)
//                throw new ArgumentNullException("merchant is not null");
//            if (request == null)
//                throw new ArgumentNullException("request is not null");

//            var title = "收款凭证";
//            printer.AddTitleLine(title, FontSize.Large);
//            printer.AddTextLine($"商户名称：{merchant.UserName}");
//            printer.AddTextLine($"商户编号：{merchant.UserId}");
//            printer.AddTextLine($"门店名称：{merchant.StoresName}");
//            printer.AddTextLine($"收 银 员：{merchant.CashName}");

//            if (request.TotalAmount > 0)
//            {
//                printer.AddTextLine($"订单金额：{request.TotalAmount.ToString("0.00")}");
//                printer.AddTextLine($"优惠金额：{request.DiscountMoney.ToString("0.00")}");
//            }
//            printer.AddTextLine($"实收金额：{ request.ReceiptAmount.ToString("0.00")}");

//            printer.AddTextLine($"支付方式：{request.PayType}");
//            printer.AddTextLine($"商户订单号：{request.TradeNo}");
//            printer.AddTextLine($"交易时间：{request.TradeTime.ToString("yyyy-MM-dd HH:mm")}");
//            printer.AddTextLine($"打印时间：{DateTime.Now.ToString("yyyy-MM-dd HH:mm")}");
//            if (request.IsRepair)
//            {
//                printer.AddTextLine($"【补打小票】");
//            }
//            if (!merchant.IsOem)
//            {
//                printer.AddTextLine($"技术支持：杭州微盘信息技术有限公司");
//                printer.AddTextLine($"联系电话：400-5862-856212555");
//            }
//        }

//        /// <summary>
//        /// 打印模板扩展
//        /// </summary>
//        /// <param name="orderModel"></param>
//        /// <param name="storeConfig"></param>
//        /// <param name="isRefund"></param>
//        /// <param name="isRepair"></param>
//        public static string PrintTemplate(this ReceiveContentRequest request, MerchantResponse merchant)
//        {
//            var sb = new StringBuilder();
//            if (merchant == null)
//                throw new ArgumentNullException("merchant is not null");
//            if (request == null)
//                throw new ArgumentNullException("request is not null");

//            var title = "\t\t收款凭证";
//            sb.AppendLine(title);
//            sb.AppendLine($"商户名称：{merchant.UserName}");
//            sb.AppendLine($"商户编号：{merchant.UserId}");
//            sb.AppendLine($"门店名称：{merchant.StoresName}");
//            sb.AppendLine($"收 银 员：{merchant.CashName}");

//            if (request.TotalAmount > 0)
//            {
//                sb.AppendLine($"订单金额：{request.TotalAmount.ToString("0.00")}");
//                sb.AppendLine($"优惠金额：{request.DiscountMoney.ToString("0.00")}");
//            }
//            sb.AppendLine($"实收金额：{request.ReceiptAmount.ToString("0.00")}");

//            sb.AppendLine($"支付方式：{request.PayType}");
//            sb.AppendLine($"商户订单号：{request.TradeNo}");
//            sb.AppendLine($"交易时间：{request.TradeTime.ToString("yyyy-MM-dd HH:mm")}");
//            sb.AppendLine($"打印时间：{DateTime.Now.ToString("yyyy-MM-dd HH:mm")}");
//            if (request.IsRepair)
//            {
//                sb.AppendLine($"【补打小票】");
//            }
//            if (!merchant.IsOem)
//            {
//                sb.AppendLine($"技术支持：杭州微盘信息技术有限公司");
//                sb.AppendLine($"联系电话：400-5862-856212555");
//            }
//            sb.AppendLine($"");
//            sb.AppendLine($"");
//            sb.AppendLine($"================================");

//            return sb.ToString();
//        }
//        #endregion

//        #region 退款凭证模板

//        /// <summary>
//        /// 打印模板扩展
//        /// </summary>
//        /// <param name="printer">打印对象</param>
//        /// <param name="storeConfig">门店相关信息对象</param>
//        /// <param name="orderModel">订单相关信息对象</param>
//        /// <param name="isRefund">是否退款</param>
//        /// <param name="isRepair">是否补打</param>
//        public static void PrintTemplate(this IBasePrinter printer, MerchantResponse merchant, RefundContentRequest request)
//        {
//            if (printer == null)
//                throw new ArgumentNullException("printer is not null");
//            if (merchant == null)
//                throw new ArgumentNullException("merchant is not null");
//            if (request == null)
//                throw new ArgumentNullException("request is not null");

//            var title = "退款凭证";
//            printer.AddTitleLine(title, FontSize.Large);
//            printer.AddTextLine($"商户名称：{merchant.UserName}");
//            printer.AddTextLine($"商户编号：{merchant.UserId}");
//            printer.AddTextLine($"门店名称：{merchant.StoresName}");
//            printer.AddTextLine($"收 银 员：{merchant.CashName}");

//            if (request.TotalAmount > 0)
//            {
//                printer.AddTextLine($"订单金额：{request.TotalAmount.ToString("0.00")}");
//                printer.AddTextLine($"优惠金额：{request.DiscountMoney.ToString("0.00")}");
//            }
//            printer.AddTextLine($"实退金额：{ request.RefundAmount.ToString("0.00")}");

//            printer.AddTextLine($"支付方式：{request.PayType}");
//            printer.AddTextLine($"商户订单号：{request.TradeNo}");
//            printer.AddTextLine($"交易时间：{request.TradeTime.ToString("yyyy-MM-dd HH:mm")}");
//            printer.AddTextLine($"打印时间：{DateTime.Now.ToString("yyyy-MM-dd HH:mm")}");
//            if (request.IsRepair)
//            {
//                printer.AddTextLine($"【补打小票】");
//            }
//            if (!merchant.IsOem)
//            {
//                printer.AddTextLine($"技术支持：杭州微盘信息技术有限公司");
//                printer.AddTextLine($"联系电话：400-5862-856212555");
//            }
//        }

//        /// <summary>
//        /// 打印模板扩展
//        /// </summary>
//        /// <param name="orderModel"></param>
//        /// <param name="storeConfig"></param>
//        /// <param name="isRefund"></param>
//        /// <param name="isRepair"></param>
//        public static string PrintTemplate(this RefundContentRequest request, MerchantResponse merchant)
//        {
//            var sb = new StringBuilder();
//            if (merchant == null)
//                throw new ArgumentNullException("merchant is not null");
//            if (request == null)
//                throw new ArgumentNullException("request is not null");

//            var title = "\t\t退款凭证";
//            sb.AppendLine(title);
//            sb.AppendLine($"商户名称：{merchant.UserName}");
//            sb.AppendLine($"商户编号：{merchant.UserId}");
//            sb.AppendLine($"门店名称：{merchant.StoresName}");
//            sb.AppendLine($"收 银 员：{merchant.CashName}");

//            if (request.TotalAmount > 0)
//            {
//                sb.AppendLine($"订单金额：{request.TotalAmount.ToString("0.00")}");
//                sb.AppendLine($"优惠金额：{request.DiscountMoney.ToString("0.00")}");
//            }
//            sb.AppendLine($"实退金额：{request.RefundAmount.ToString("0.00")}");

//            sb.AppendLine($"支付方式：{request.PayType}");
//            sb.AppendLine($"商户订单号：{request.TradeNo}");
//            sb.AppendLine($"交易时间：{request.TradeTime.ToString("yyyy-MM-dd HH:mm")}");
//            sb.AppendLine($"打印时间：{DateTime.Now.ToString("yyyy-MM-dd HH:mm")}");
//            if (request.IsRepair)
//            {
//                sb.AppendLine($"【补打小票】");
//            }
//            if (!merchant.IsOem)
//            {
//                sb.AppendLine($"技术支持：杭州微盘信息技术有限公司");
//                sb.AppendLine($"联系电话：400-5862-856212555");
//            }
//            sb.AppendLine($"");
//            sb.AppendLine($"");
//            sb.AppendLine($"================================");

//            return sb.ToString();
//        }
//        #endregion

//        #region 交班结算模板
//        /// <summary>
//        /// 打印交班结算模板
//        /// </summary>
//        /// <param name="printer"></param>
//        /// <param name="merchant"></param>
//        /// <param name="orderSummary"></param>
//        public static void PrintTemplate(this IBasePrinter printer, CashierMerchantInfo merchant, OrderSummaryResponse orderSummary, EnumPrinterType enumPrinterType = EnumPrinterType.WorkSettle)
//        {
//            if (printer == null)
//                throw new ArgumentNullException("printer is not null");
//            if (merchant == null)
//                throw new ArgumentNullException("merchant is not null");
//            if (orderSummary == null)
//                throw new ArgumentNullException("orderSummary is not null");

//            printer.AddTitleLine(enumPrinterType.GetDescription(), FontSize.Large);
//            printer.AddTextLine($"");

//            if (enumPrinterType == EnumPrinterType.WorkSettle)
//            {
//                printer.AddTextLine($"上次交接：{Convert.ToDateTime(orderSummary.PreSettledTime).ToString("yyyy-MM-dd HH:mm:ss")}");
//                printer.AddTextLine($"本次交接：{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");
//            }
//            else
//            {
//                if (orderSummary.Search == null)
//                {
//                    throw new ArgumentNullException("orderSummary Search is not null");
//                }
//                printer.AddTextLine($"开始时间：{Convert.ToDateTime(orderSummary.Search.StartTime).ToString("yyyy-MM-dd HH:mm:ss")}");
//                printer.AddTextLine($"结束时间：{orderSummary.Search.EndTime.ToString("yyyy-MM-dd HH:mm:ss")}");
//            }

//            printer.AddTextLine($"商户名称：{merchant.UserName}");
//            printer.AddTextLine($"商户编号：{merchant.UserId}");
//            printer.AddTextLine($"门店名称：{merchant.StoresName}");
//            printer.AddTextLine($"收 银 员：{merchant.CashName}");
//            printer.AddTextLine($"收银账号：{merchant.CashId}");

//            printer.AddTextLine($"-------收款方式统计-------");

//            orderSummary.List.ForEach(item =>
//            {
//                if (merchant.IsMemberPermission && item.PayType == EnumGatewayPayType.MemberPay)
//                {
//                    printer.AddTextLine(item.PayTypeName);
//                    printer.AddTextLine($"收款：{item.ReceiptMoney.ToString("0.00")}元,  笔数：{item.ReceiptCount}笔");
//                    if (item.RefundMoney > 0)
//                        printer.AddTextLine($"退款：{item.RefundMoney.ToString("0.00")}元,  笔数：{item.RefundCount}笔");

//                    return;
//                }
//                printer.AddTextLine(item.PayTypeName);
//                printer.AddTextLine($"收款：{item.ReceiptMoney.ToString("0.00")}元,  优惠：{item.DiscountsMoney.ToString("0.00")}元");
//                printer.AddTextLine($"实收：{item.RealReceiptMoney.ToString("0.00")}元,  笔数：{item.ReceiptCount}笔");

//                if (item.RealRefundMoney > 0)
//                {
//                    printer.AddTextLine($"退款：{item.RefundMoney.ToString("0.00")}元,  实退：{item.RealRefundMoney.ToString("0.00")}元");
//                    printer.AddTextLine($"笔数：{item.RefundCount}笔");

//                }
//            });

//            printer.AddTextLine($"------------------------");

//            printer.AddTextLine($"合计");
//            printer.AddTextLine($"收款：{orderSummary.TotalSummary.ReceiptMoney.ToString("0.00")}元,  优惠：{orderSummary.TotalSummary.DiscountsMoney.ToString("0.00")}元");
//            printer.AddTextLine($"实收：{orderSummary.TotalSummary.RealReceiptMoney.ToString("0.00")}元,  笔数：{orderSummary.TotalSummary.ReceiptCount}笔");
//            printer.AddTextLine($"退款：{orderSummary.TotalSummary.RefundMoney.ToString("0.00")}元,  实退：{orderSummary.TotalSummary.RealRefundMoney.ToString("0.00")}元");
//            printer.AddTextLine($"笔数：{orderSummary.TotalSummary.RefundCount}笔");

//            if (merchant.IsMemberPermission)
//            {
//                printer.AddTextLine($"储值卡收款：{orderSummary.TotalSummary.StoreTotalFee.ToString("0.00")}元");
//                printer.AddTextLine($"收款笔数：{orderSummary.TotalSummary.StoreTotalCount}笔");

//                //if (orderSummary.TotalSummary.StoreRefundTotalFee > 0)
//                //{
//                    printer.AddTextLine($"储值卡退款：{orderSummary.TotalSummary.StoreRefundTotalFee.ToString("0.00")}元");
//                    printer.AddTextLine($"退款笔数：{orderSummary.TotalSummary.StoreRefundTotalCount}笔");
//                //}

//            }

//            printer.AddTextLine($"");
//            printer.AddTextLine($"------------------------");
//            var incomeTitle = enumPrinterType == EnumPrinterType.WorkSettle ? "交班净收款":"净收款";

//            printer.AddTextLine($"{incomeTitle}：{orderSummary.IncomeMoney.ToString("0.00")}元");
//            if (enumPrinterType == EnumPrinterType.WorkSettle)
//            {
//                printer.AddTextLine($"收银员签名：      财务签名：");
//            }

//            printer.AddTextLine($"------------------------");
//            printer.AddTextLine($"打印时间：{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");
//            if (!merchant.IsOem)
//            {
//                printer.AddTextLine($"技术支持：杭州微盘信息技术有限公司");
//                printer.AddTextLine($"联系电话：400-5862-856212555");
//            }
//            printer.AddTextLine($"");
//            printer.AddTextLine($"");

//        }

//        /// <summary>
//        /// 打印交班结算模板
//        /// </summary>
//        /// <param name="printer"></param>
//        /// <param name="merchant"></param>
//        /// <param name="orderSummary"></param>
//        public static string PrintTemplate(this OrderSummaryResponse orderSummary, CashierMerchantInfo merchant, EnumPrinterType enumPrinterType = EnumPrinterType.WorkSettle)
//        {
//            var sb = new StringBuilder();
//            if (merchant == null)
//                throw new ArgumentNullException("merchant is not null");
//            if (orderSummary == null)
//                throw new ArgumentNullException("orderSummary is not null");

//            sb.AppendLine($"\t\t{enumPrinterType.GetDescription()}");

//            if (enumPrinterType == EnumPrinterType.WorkSettle)
//            {
//                sb.AppendLine($"上次交接：{Convert.ToDateTime(orderSummary.PreSettledTime)}");
//                sb.AppendLine($"本次交接：{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");
//            }
//            else
//            {
//                if (orderSummary.Search == null)
//                    throw new ArgumentNullException("orderSummary search is not null");

//                sb.AppendLine($"开始时间：{orderSummary.Search.StartTime.ToString("yyyy-MM-dd HH:mm:ss")}");
//                sb.AppendLine($"结束时间：{orderSummary.Search.EndTime.ToString("yyyy-MM-dd HH:mm:ss")}");
//            }

//            sb.AppendLine($"商户名称：{merchant.UserName}");
//            sb.AppendLine($"商户编号：{merchant.UserId}");
//            sb.AppendLine($"门店名称：{merchant.StoresName}");
//            sb.AppendLine($"收 银 员：{merchant.CashName}");
//            sb.AppendLine($"收银账号：{merchant.CashId}");

//            sb.AppendLine($"");
//            sb.AppendLine($"----------收款方式统计----------");
//            sb.AppendLine($"");

//            orderSummary.List.ForEach(item =>
//            {
//                if (merchant.IsMemberPermission && item.PayType == EnumGatewayPayType.MemberPay)
//                {
//                    sb.AppendLine(item.PayTypeName);
//                    sb.AppendLine($"收款：{item.ReceiptMoney.ToString("0.00")}元,  笔数：{item.ReceiptCount}笔");
//                    if (item.RefundMoney > 0)
//                        sb.AppendLine($"退款：{item.RefundMoney.ToString("0.00")}元,  笔数：{item.RefundCount}笔");

//                    return;
//                }
//                sb.AppendLine(item.PayTypeName);
//                sb.AppendLine($"收款：{item.ReceiptMoney.ToString("0.00")}元,  优惠：{item.DiscountsMoney.ToString("0.00")}元");
//                sb.AppendLine($"实收：{item.RealReceiptMoney.ToString("0.00")}元,  笔数：{item.ReceiptCount}笔");

//                if (item.RealRefundMoney > 0)
//                {
//                    sb.AppendLine($"退款：{item.RefundMoney.ToString("0.00")}元,  实退：{item.RealRefundMoney.ToString("0.00")}元");
//                    sb.AppendLine($"笔数：{item.RefundCount}笔");

//                }
//            });

//            sb.AppendLine($"");
//            sb.AppendLine($"------------------------------");
//            sb.AppendLine($"");

//            sb.AppendLine($"合计");
//            sb.AppendLine($"收款：{orderSummary.TotalSummary.ReceiptMoney.ToString("0.00")}元,  优惠：{orderSummary.TotalSummary.DiscountsMoney.ToString("0.00")}元");
//            sb.AppendLine($"实收：{orderSummary.TotalSummary.RealReceiptMoney.ToString("0.00")}元,  笔数：{orderSummary.TotalSummary.ReceiptCount}笔");
//            sb.AppendLine($"退款：{orderSummary.TotalSummary.RefundMoney.ToString("0.00")}元,  实退：{orderSummary.TotalSummary.RealRefundMoney.ToString("0.00")}元");
//            sb.AppendLine($"笔数：{orderSummary.TotalSummary.RefundCount}笔");

//            if (merchant.IsMemberPermission)
//            {
//                sb.AppendLine($"储值卡收款：{orderSummary.TotalSummary.StoreTotalFee.ToString("0.00")}元");
//                sb.AppendLine($"收款笔数：{orderSummary.TotalSummary.StoreTotalCount}笔");
//                //if (orderSummary.TotalSummary.StoreRefundTotalFee > 0)
//                //{
//                    sb.AppendLine($"储值卡退款：{orderSummary.TotalSummary.StoreRefundTotalFee.ToString("0.00")}元");
//                    sb.AppendLine($"退款笔数：{orderSummary.TotalSummary.StoreRefundTotalCount}笔");
//                //}
//            }

//            sb.AppendLine($"");
//            sb.AppendLine($"------------------------------");
//            sb.AppendLine($"交班净收入：{orderSummary.IncomeMoney.ToString("0.00")}元");
//            if (enumPrinterType == EnumPrinterType.WorkSettle)
//            {
//                sb.AppendLine($"收银员签名：        财务签名：");
//            }

//            sb.AppendLine($"");
//            sb.AppendLine($"");
//            sb.AppendLine($"------------------------------");
//            sb.AppendLine($"打印时间：{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");
//            if (!merchant.IsOem)
//            {
//                sb.AppendLine($"技术支持：杭州微盘信息技术有限公司");
//                sb.AppendLine($"联系电话：400-5862-856212555");
//            }
//            sb.AppendLine($"");
//            sb.AppendLine($"");
//            sb.AppendLine($"------------------------------");
//            return sb.ToString();
//        }
//        #endregion
//    }
//}