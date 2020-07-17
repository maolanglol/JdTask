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
//using static WP.Device.OCR.Enums.OCREnums;

//namespace Jlion.BrushClient.Client
//{
//    /// <summary>
//    /// 打印扩展方法
//    /// </summary>
//    public static partial class Extensions
//    {
//        public static List<string> ToTTSList(this List<Char> list)
//        {
//            var ttsList = new List<string>();
//            var count = ttsList.Count;
//            foreach (var item in list)
//            {
//                ttsList.Add(string.Format(TTSType.Number.GetDescription(), item));
//            }
//            return ttsList;
//        }

//        /// <summary>
//        /// 转换为金额语音文件路径列表
//        /// </summary>
//        /// <param name="list"></param>
//        /// <returns></returns>
//        public static List<string> ToConvertTTS(this int amount)
//        {
//            var oldAmount = amount;
//            var ttsList = new List<string>();
//            var decade = amount / 10;
//            var hundred = amount / 100;
//            var thousand = amount / 1000;
//            var tenThousand = amount / 10000;

//            if (tenThousand > 0 && tenThousand <= 9)
//            {
//                ttsList.Add(string.Format(@TTSType.Number.GetDescription(), tenThousand));
//                ttsList.Add(TTSType.TenThousand.GetDescription());

//                amount = (amount - 10000 * tenThousand);
//                var list = amount.ToConvertTTS();
//                if ((amount / 1000) <= 0 && (oldAmount % 10000) > 0)
//                {
//                    ttsList.Add(string.Format(@TTSType.Number.GetDescription(), 0));
//                }
//                if (list != null)
//                {
//                    ttsList.AddRange(list);
//                }
//                return ttsList;
//            }
//            if (thousand > 0 && thousand <= 9)
//            {
//                ttsList.Add(string.Format(@TTSType.Number.GetDescription(), thousand));
//                ttsList.Add(TTSType.Thousand.GetDescription());

//                amount = (amount - 1000 * thousand);
//                var list = amount.ToConvertTTS();
//                if ((amount / 100) <= 0 && (oldAmount % 1000) > 0)
//                {
//                    ttsList.Add(string.Format(@TTSType.Number.GetDescription(), 0));
//                }
//                if (list != null)
//                {
//                    ttsList.AddRange(list);
//                }
//                return ttsList;
//            }
//            if (hundred > 0 && hundred <= 9)
//            {
//                ttsList.Add(string.Format(@TTSType.Number.GetDescription(), hundred));
//                ttsList.Add(TTSType.Tundred.GetDescription());

//                amount = (amount - 100 * hundred);
//                var list = amount.ToConvertTTS();
//                if ((amount / 10) <= 0 && (oldAmount % 100) > 0)
//                {
//                    ttsList.Add(string.Format(@TTSType.Number.GetDescription(), 0));
//                }
//                if (list != null)
//                {
//                    ttsList.AddRange(list);
//                }
//                return ttsList;
//            }
//            if (decade > 0 && decade <= 9)
//            {
//                ttsList.Add(string.Format(@TTSType.Number.GetDescription(), decade));
//                ttsList.Add(TTSType.Ten.GetDescription());

//                amount = (amount - 10 * decade);
//                var list = amount.ToConvertTTS();
//                if (list != null)
//                {
//                    ttsList.AddRange(list);
//                }
//                return ttsList;
//            }
//            if (amount > 0 && amount <= 9)
//            {
//                ttsList.Add(string.Format(@TTSType.Number.GetDescription(), amount));
//                return ttsList;
//            }
//            return null;
//        }

//        /// <summary>
//        /// 金额转换成语音文件相对路径集合
//        /// </summary>
//        /// <param name="money"></param>
//        /// <returns></returns>
//        public static List<string> ToTTSPathList(this decimal money)
//        {
//            var ttsList = new List<string>();
//            try
//            {
//                var moneyInteger = money.ToString().Split('.')[0];
//                var moneyIntegerList = moneyInteger.ToList();

//                var moneyDecimal = money.ToString().Split('.')[1];
//                var moneyDecimalList = moneyDecimal.ToList();

//                if (moneyInteger.ToInt32OrDefault(0) > 99999)
//                {
//                    throw new Exception("超过语音播报的数量大小");
//                }

//                #region 整数位
//                var list = moneyInteger.ToInt32OrDefault(0).ToConvertTTS();
//                if (list == null)
//                {
//                    ttsList.Add(string.Format(@TTSType.Number.GetDescription(), 0));
//                }
//                else
//                {
//                    ttsList.AddRange(list);
//                }
//                #endregion

//                #region 小数位
//                if (Convert.ToInt32(moneyDecimal) <= 0)
//                {
//                    return ttsList;
//                }

//                ttsList.Add(TTSType.Decimal.GetDescription());
//                ttsList.AddRange(moneyDecimal.ToList().ToTTSList());
//                return ttsList;
//                #endregion
//            }
//            catch (Exception ex)
//            {
//                return null;
//            }
//            return ttsList;
//        }

//        /// <summary>
//        /// 编码转换
//        /// </summary>
//        /// <param name="encoding"></param>
//        /// <returns></returns>
//        public static Encoding ToEncoding(this string encoding)
//        {
//            try
//            {
//                if (encoding.Equals("-1"))
//                    return Encoding.Default;

//                if (string.IsNullOrWhiteSpace(encoding))
//                    return Encoding.GetEncoding("GBK");

//                return Encoding.GetEncoding(encoding);
//            }
//            catch (Exception ex)
//            {
//                return Encoding.GetEncoding("GBK");
//            }
//        }
//    }
//}