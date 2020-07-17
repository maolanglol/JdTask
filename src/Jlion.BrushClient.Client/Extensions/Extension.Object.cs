//using System;
//using System.Collections.Generic;
//using System.Drawing;
//using System.Linq;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;
//using Jlion.BrushClient.Application.Model;
//using Jlion.BrushClient.Client.Model;
//using Jlion.BrushClient.UControl;
//using static Jlion.BrushClient.Client.Enums.OptionEnums;
//using Jlion.BrushClient.Application.Enums;
//using System.Collections.ObjectModel;
//using Newtonsoft.Json;
//using Jlion.BrushClient.Client.Model.Response;
//using System.Data;
//using Jlion.BrushClient.Framework;
//using System.Windows.Media;
//using System.Windows.Media.Imaging;

//namespace Jlion.BrushClient.Client
//{
//    /// <summary>
//    /// 打印扩展方法
//    /// </summary>
//    public static partial class Extensions
//    {
//        //public static SerialPortConfig ToConfig(this PrintCommonConfigModel commonConfigModel)
//        //{
//        //    if (commonConfigModel == null)
//        //        return null;

//        //    return new SerialPortConfig()
//        //    {
//        //        BaudRate = commonConfigModel.BaudRate,
//        //        OutputPortName = commonConfigModel.InputPortName
//        //    };
//        //}


//        //public static MainHostApiResponse<T> ToResponse<T>(this MainHostApiResponse<string> data)
//        //{
//        //    if (data == null)
//        //        return null;

//        //    if (!(data?.IsError ?? true))
//        //    {
//        //        var obj = JsonConvert.DeserializeObject<T>(data.Data.ToString());
//        //        return MainHostApiResponse<T>.(obj, data.Msg);
//        //    }

//        //    return MainHostApiResponse<T>.Error(ApiCodeEnums.FAIL, default(T), data.Msg);
//        //}

//        public static ObservableCollection<T> ToCollection<T>(this List<T> data)
//        {
//            var result = new ObservableCollection<T>();
//            if (data == null)
//                return null;

//            data.ForEach(item =>
//            {
//                result.Add(item);
//            });

//            return result;
//        }

//        public static OcrConfigModel ToModel(this OcrConfigModel model, ScreenConfig data)
//        {
//            if (data == null)
//                return model;

//            model.IsCheck = false;
//            model.PointItems = new PointItemCollection();
//            model.AnnotationPath = data.AnnotationPath;
//            data.CoordinateConfigs?.ForEach(item =>
//            {
//                model.PointItems.Add(new PointItem()
//                {
//                    CoordinateEnumType = (int)item.CoordinateEnumType,
//                    Height = item.Height,
//                    Width = item.Width,
//                    X = item.X,
//                    Y = item.Y
//                });
//            });
//            return model;
//        }


//        public static OrderPageRequest ToPageModel(this OrderDataRequest data)
//        {
//            if (data == null)
//                return null;
//            return new OrderPageRequest()
//            {
//                Rows = data.Rows,
//                LoginCashId = data.CashId,
//                Page = data.PageIndex,
//                State = (int)data.State,
//                PayType = (int)data.PayType,
//                GteEndTime = data.PayStartTime,
//                LteEndTime = data.PayEndTime,
//                StoreId = data.StoresId,
//                GteTotalAmt = 0,
//                LteTotalAmt = 0
//            };
//        }

//        public static CardOrderPageRequest ToPageModel(this CardOrderRequest data)
//        {
//            if (data == null)
//                return null;
//            return new CardOrderPageRequest()
//            {
//                Rows = data.Rows,
//                LoginCashId = data.CashId,
//                Page = data.PageIndex,
//                StartTime = data.StartTime,
//                EndTime = data.EndTime,
//                StoreId = data.StoresId,
//                Type = data.Type,
//                CardNo = data.PhoneOrCardNo,
//                OrderNo = data.OrderNo,
//                Phone = data.PhoneOrCardNo
//            };
//        }

//        public static ReceiveContentRequest ToModel(this OrderPageResponse data, bool isRepair = true)
//        {
//            if (data == null)
//                return null;
//            return new ReceiveContentRequest()
//            {
//                IsRepair = isRepair,
//                ReceiptAmount = data.ReceiptAmt,
//                TotalAmount = data.TotalAmt,
//                PayType = data.PayType.ToPayType().GetDescription(),
//                TradeNo = data.OutTradeNo,
//                TradeTime = data.PayTime
//            };
//        }


//        public static ReceiveContentRequest ToModel(this OrderItemResponse data, bool isRepair = true)
//        {
//            if (data == null)
//                return null;
//            return new ReceiveContentRequest()
//            {
//                IsRepair = isRepair,
//                TotalAmount = data.TotalFee,
//                ReceiptAmount = data.ReceiptAmt,
//                PayType = data.PayType.ToPayType().GetDescription(),
//                TradeNo = data.OutTradeNo,
//                TradeTime = Convert.ToDateTime(data.PayTime)
//            };
//        }

//        public static RefundContentRequest ToRefundModel(this OrderItemResponse data, bool isRepair = true)
//        {
//            if (data == null)
//                return null;
//            return new RefundContentRequest()
//            {
//                IsRepair = isRepair,
//                TotalAmount = data.TotalFee,
//                ReceiptAmount = data.ReceiptAmt,
//                RefundAmount = data.ReceiptAmt,
//                PayType = data.PayType.ToPayType().GetDescription(),
//                TradeNo = data.OutTradeNo,
//                TradeTime = Convert.ToDateTime(data.PayTime)
//            };
//        }

//        public static RefundContentRequest ToRefundModel(this CardOrderItemResponse data, bool isRepir = true)
//        {
//            if (data == null)
//                return null;

//            var payType = "";
//            switch (data.Type)
//            {
//                case (int)CardOrderTypeEnums.Recharge:
//                    payType = "支付宝";
//                    break;
//                case (int)CardOrderTypeEnums.Consumer:
//                case (int)CardOrderTypeEnums.Refund:
//                    payType = "储值卡";
//                    break;
//                default:
//                    throw new Exception("不支持的类型");
//            }
//            return new RefundContentRequest()
//            {
//                IsRepair = isRepir,
//                ReceiptAmount = data.TotalAmount,
//                RefundAmount = data.TotalAmount,
//                IsCard = true,
//                PayType = payType,
//                TradeNo = data.OrderNo,
//                TradeTime = Convert.ToDateTime(data.OrderTradeTime)
//            };
//        }

//        public static ReceiveContentRequest ToModel(this CardOrderItemResponse data, bool isRepir = true)
//        {
//            if (data == null)
//                return null;

//            var payType = "";
//            switch (data.Type)
//            {
//                case (int)CardOrderTypeEnums.Recharge:
//                    payType = "支付宝";
//                    break;
//                case (int)CardOrderTypeEnums.Consumer:
//                case (int)CardOrderTypeEnums.Refund:
//                    payType = "储值卡";
//                    break;
//                default:
//                    throw new Exception("不支持的类型");
//            }
//            return new ReceiveContentRequest()
//            {
//                IsRepair = isRepir,
//                ReceiptAmount = data.TotalAmount,
//                IsCard = true,
//                PayType = payType,
//                TradeNo = data.OrderNo,
//                TradeTime = Convert.ToDateTime(data.OrderTradeTime)
//            };
//        }

//        public static OrderSummaryResponse ToModel(this CashierHandOverResponse data, bool IsMemberPermission)
//        {
//            if (data == null)
//                return null;

//            var result = new OrderSummaryResponse();
//            result.TotalSummary.DiscountsMoney = (data.SumData.DiscountAmount + 0.00M);
//            result.TotalSummary.RealReceiptMoney = (data.SumData.ReceiptAmount + 0.00M);
//            result.TotalSummary.RealRefundMoney = (data.SumData.RefundAmount + 0.00M);
//            result.TotalSummary.ReceiptCount = data.SumData.TotalCount;
//            result.TotalSummary.ReceiptMoney = (data.SumData.TotalFee + 0.00M);
//            result.TotalSummary.RefundCount = data.SumData.RefundTotalCount;
//            result.TotalSummary.RefundMoney = (data.SumData.RefundTotalFee + 0.00M);
//            result.TotalSummary.StoreTotalFee = (data.SumData.StoreTotalFee + 0.00M);
//            result.TotalSummary.StoreTotalCount = data.SumData.StoreTotalCount;
//            result.TotalSummary.StoreRefundTotalFee = (data.SumData.StoreRefundTotalFee + 0.00M);
//            result.TotalSummary.StoreRefundTotalCount = data.SumData.StoreRefundTotalCount;

//            result.CanPatchTicket = data.CanPatchTicket;
//            result.PreSettledTime = string.IsNullOrEmpty(data.LastHandOverTime) ? "" : Convert.ToDateTime(data.LastHandOverTime).ToString("yyyy/MM/dd HH:mm");

//            result.List = data.PayDatas?.Select(item => new OrderSummaryItemResponse()
//            {
//                DiscountsMoney = (item.DiscountAmount + 0.00M),
//                PayType = item.PayType.ToPayType(),
//                RealReceiptMoney = (item.ReceiptAmount + 0.00M),
//                RealRefundMoney = (item.RefundAmount + 0.00M),
//                ReceiptCount = item.TotalCount,
//                ReceiptMoney = (item.ReceiptAmount + 0.00M),
//                RefundCount = item.RefundTotalCount,
//                RefundMoney = (item.RefundTotalFee + 0.00M)
//            }).ToList();

//            if (!IsMemberPermission)
//                result.List.RemoveAll(item => item.PayType == EnumGatewayPayType.MemberPay);

//            return result;
//        }

//        public static DataTable ToModel(this List<OrderPageResponse> data)
//        {
//            if (data == null)
//                return null;

//            var dt = new DataTable()
//            {
//                TableName = $"订单数据_{DateTime.Now.ToString("yyyy-MM-dd")}"
//            };

//            dt.Columns.Add("金额");
//            dt.Columns.Add("支付方式");
//            dt.Columns.Add("订单状态");
//            dt.Columns.Add("交易时间");
//            dt.Columns.Add("订单号");


//            data.ForEach(item =>
//            {
//                dt.Rows.Add(
//                    item.ReceiptAmt,
//                    item.PayType.ToPayType().GetDescription(),
//                    item.State.ToClientOrderStatus().GetDescription(),
//                    item.PayTime.ToString("yyyy-MM-dd HH:mm:ss"),
//                    item.OutTradeNo);
//            });

//            return dt;
//        }

//        public static DataTable ToModel(this List<CardOrderItemResponse> data)
//        {
//            if (data == null)
//                return null;

//            var dt = new DataTable()
//            {
//                TableName = $"储值订单数据_{DateTime.Now.ToString("yyyy-MM-dd")}"
//            };

//            dt.Columns.Add("交易金额");
//            dt.Columns.Add("赠送金额");
//            dt.Columns.Add("交易类型");
//            dt.Columns.Add("交易时间");
//            dt.Columns.Add("订单号");


//            data.ForEach(item =>
//            {
//                dt.Rows.Add(
//                    item.TotalAmount,
//                    item.GiveAmount,
//                    item.TypeName,
//                    item.OrderTradeTime,
//                    item.OrderNo);
//            });

//            return dt;
//        }

//        //public static OrderDataItem ToModel(this OrderPageResponse data)
//        //{
//        //    if (data == null)
//        //        return null;

//        //    return new OrderDataItem()
//        //    {
//        //        Amount = data.TotalAmt.ToString(),
//        //        CanRefundAmount = data.CanRefundAmt.ToString(),
//        //        CashName = data.CashName,
//        //        OrderType = data.PayType,
//        //        OutTradeNo = data.OtNo,
//        //        PayTime = data.EndTime.ToString("yyyy-MM-dd HH:mm"),
//        //        State = data.State,
//        //        StoresId = data.StoreId,
//        //        TotalFee = data.TotalAmt,
//        //         ReceiptAmount=data.ReceiptAmount,
//        //    };
//        //}

//        public static CardOrderItemResponse ToModel(this CardOrderPageResponse data)
//        {
//            if (data == null)
//                return null;

//            return new CardOrderItemResponse()
//            {
//                CanRefundAmt = (data.CanRefundAmt + 0.00M),
//                GiveAmount = Convert.ToDecimal(data.GiveAmount).ToString("0.00"),
//                OrderTradeTime = (new DateTime(data.OrderTradeTime)).ToString("yyyy-MM-dd HH:mm:ss"),
//                TotalAmount = (data.TotalAmount + 0.00M),
//                Type = data.Type,
//                OrderNo = data.OrderTradeNo,
//                TypeName = ((CardOrderTypeEnums)data.Type).GetDescription()
//            };
//        }

//        #region 配置
//        public static ScreenConfig ToModel(this OcrConfigModel data)
//        {
//            if (data == null)
//                return null;

//            var resp = new ScreenConfig()
//            {
//                Algorithm = data.ImageProcess.Algorithm,
//                ErodeLevel = data.ImageProcess.ErodeLevel,
//                OCRResourcePath = data.OCRResourcePath,
//                ThresholdType = data.ImageProcess.ThresholdType,
//                ThresholdValue = data.ImageProcess.ThresholdValue,
//                ZoomLevel = data.ImageProcess.ZoomLevel,
//                DilateLevel = data.ImageProcess.DilateLevel,
//                OcrType = (Kernel.Enums.OcrOptionType.OcrTypeEnums)data.OcrType,
//                OcrModelType = (Kernel.Enums.OcrOptionType.OcrModeEnums)data.ImageProcess.OcrModelEnums,
//                AnnotationPath = data.AnnotationPath,
//                ChannelType = (OcrChannelTypeEnums)data.ImageProcess.ChannelType
//            };

//            for (var i = 0; i < data.PointItems.Count; i++)
//            {
//                resp.CoordinateConfigs.Add(new CoordinateConfig()
//                {
//                    CoordinateEnumType = (CoordinateEnumType)data.PointItems[i].CoordinateEnumType,
//                    Height = data.PointItems[i].Height,
//                    Width = data.PointItems[i].Width,
//                    X = data.PointItems[i].X,
//                    Y = data.PointItems[i].Y
//                });
//            }

//            return resp;
//        }
//        #endregion

//        public static Bitmap ToBitMap(this ImageSource imageSource)
//        {
//            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
//            {
//                BmpBitmapEncoder encoder = new BmpBitmapEncoder();
//                encoder.Frames.Add(BitmapFrame.Create((BitmapSource)imageSource));
//                encoder.Save(ms);

//                Bitmap bp = new Bitmap(ms);
//                ms.Close();
//                return bp;
//            }
//        }

//        public static OrderPayResponse ToModel(this OrderPayQueryResponse orderPayQueryResponse)
//        {
//            if (orderPayQueryResponse == null)
//                return null;
//            return new OrderPayResponse()
//            {
//                BuyerId = orderPayQueryResponse.BuyerId,
//                DiscountFee = orderPayQueryResponse.DiscountFee,
//                Error = "",
//                MerchantNo = orderPayQueryResponse.MerchantNo,
//                OutTradeNo = orderPayQueryResponse.OutTradeNo,
//                PayPayment = orderPayQueryResponse.PayPayment,
//                PayTime = orderPayQueryResponse.PayTime,
//                PayType = orderPayQueryResponse.PayType,
//                ReceiptFee = orderPayQueryResponse.ReceiptFee,
//                Result = "",
//                State = orderPayQueryResponse.State,
//                TotalFee = orderPayQueryResponse.TotalFee,
//                TransactionId = orderPayQueryResponse.OutTradeNo
//            };
//        }
//    }
//}