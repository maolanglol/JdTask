using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Jlion.BrushClient.Framework
{
    /// <summary>
    /// 扩展表达式
    /// </summary>
    public static partial class ObjectExtensions
    {
        /// <summary>
        /// 解析出金额
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static decimal ResolveMoney(this string data)
        {
            if (string.IsNullOrEmpty(data))
                return -1M;

            data = data.RemoveChar('¥');

            Match m = Regex.Match(data, "\\d*(\\.\\d+){0,1}");

            var result = -1M;
            if (m.Groups[0].ToString().Equals(""))
                return result;

            decimal.TryParse(m.Groups[0].ToString(), out result);
            return result;
        }

        /// <summary>
        /// 解析出金额
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static decimal ResovleNumber(this string data)
        {
            if (string.IsNullOrEmpty(data))
                return -1M;


            Match m = Regex.Match(data, "\\d+(\\.\\d+){0,1}");

            var result = -1M;
            if (m.Groups[0].ToString().Equals(""))
                return result;

            decimal.TryParse(m.Groups[0].ToString(), out result);
            return result;
        }

        public static decimal ResolveMoney(this string data, string matchReg)
        {
            decimal num = 0m;
            MatchCollection matchCollection = Regex.Matches(data.Replace(",", ""), matchReg);
            if (matchCollection.Count > 0)
            {
                decimal.TryParse(matchCollection[matchCollection.Count - 1].Groups["total"].Value, out num);
            }
            return num;
        }

        public static decimal ResolveMoneyBySerialPort(this string data)
        {
            var amount = 0M;
            try
            {
                if (string.IsNullOrEmpty(data))
                    return 0M;

                string serialReg = "\u001bQA(?<total>[\\d.]*)|total:(?<total>[\\d.]*)|\u0018total:(?<total>[\\d.]*)|\ftotal:(?<total>[\\d.]*)|\u001bQB(?<total>[\\d.]*)|\u0004\u0002(?<total>[\\d.]*)|TOT(?<total>[\\d.]*)|\u0015\u0002(?<total>[\\d.]*)|\u001bQ(?<total>[\\d.]*)";
                string serialReg2 = "(?<total>[0-9,]+[.][0-9]{2})";
                amount = ResolveMoney(data, serialReg);
                if (amount <= 0)
                {
                    amount = ResolveMoney(data, serialReg2);
                }
            }
            catch (Exception ex)
            {
                TextHelper.Error("ResolveMoneyBySerialPort 异常", ex);
            }
            return amount;



            //if (data.Contains("QA"))
            //{
            //    var index = data.IndexOf("QA");
            //    var dataSuffix = data.Substring(index + 2);
            //    return dataSuffix.ResolveMoney();
            //}

            //return data.ResolveMoney();
        }

        /// <summary>
        /// 移除字符
        /// </summary>
        /// <param name="data"></param>
        /// <param name="removeChar"></param>
        /// <returns></returns>
        public static string RemoveChar(this string data, char removeChar)
        {
            if (data.Contains(removeChar))
                return data.Replace(removeChar.ToString(), "");
            return data;
        }

        /// <summary>
        /// 判断是否是数字
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool IsNumber(this char data)
        {
            try
            {
                return Convert.ToInt32(data.ToString()) >= 0;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 字符串转Key
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static Key ConvertKeys(this string data)
        {
            var typeOf = Key.AbntC1.GetType();
            foreach (int item in Enum.GetValues(typeOf))
            {
                string strName = Enum.GetName(typeOf, item);//获取名称

                if (strName.Equals(data))
                    return (Key)item;
            }
            return Key.None;
        }

        /// <summary>
        /// 字符串转成虚拟码
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static int VirKeyConvertKeys(this string data)
        {
            try
            {
                var key = data.ConvertKeys();
                return KeyInterop.VirtualKeyFromKey(key);
            }
            catch
            {
                return -1;
            }
        }

        /// <summary>
        /// 键盘码组合转成组合虚拟键（Q+B）类似于这样的键盘码
        /// </summary>
        /// <param name="vkCode"></param>
        /// <returns></returns>
        public static int BitOperator(this string vkCodeStr)
        {
            var virKeyList = new List<int>();
            if (vkCodeStr.Contains("+"))
            {
                var array = vkCodeStr.Split('+').ToList();
                array.ForEach(arr =>
                {
                    var virKey = arr.VirKeyConvertKeys();
                    if (virKey != -1)
                        virKeyList.Add(virKey);
                });
            }
            else
            {
                var vk = vkCodeStr.VirKeyConvertKeys();
                if (vk == -1)
                    return -1;
                virKeyList.Add(vk);
            }
            return virKeyList.BitOperator();
        }

        public static int BitOperator(this List<int> list)
        {
            var result = 0;
            list.ForEach(item =>
            {
                if (result == 0)
                {
                    result = item;
                }
                else
                {
                    result = result | item;
                }
            });
            return result;
        }

        public static T ConvertObject<T>(this string data)
        {
            if (string.IsNullOrEmpty(data))
                return default(T);

            return JsonConvert.DeserializeObject<T>(data);
        }

        /// <summary>
        /// 是否是金额
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static bool IsMoney(this string text)
        {
            try
            {
                if (!text.Contains(".") && text.Equals("00"))
                    return false;

                Regex numbeRegex = new Regex("^[0-9]+$|^[0-9]*[.]{0,1}[0-9]?[0-9]?$");
                return numbeRegex.IsMatch(text);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 是否是数字
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static bool IsNumberic(this string text)
        {
            try
            {
                return Convert.ToInt32(text) >= 0;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 是否可以输入
        /// 前提是已经是数字的情况
        /// </summary>
        /// <param name="text"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static bool IsInputMoney(this string text, Key key, int selectStart = 0, int count = 2)
        {
            try
            {
                if (key == Key.Back || key == Key.Delete)
                {
                    return false;
                }
                if (key == Key.Decimal)
                {
                    return text.Contains(".");
                }
                if (string.IsNullOrEmpty(text))
                    return false;


                if (!text.Contains('.'))
                    return false;

                var decimalIndex = text.IndexOf('.');
                var decimalText = text.Substring(decimalIndex + 1);

                if (selectStart <= decimalIndex)
                    return false;

                return decimalText.Length >= 2;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 判断字符是否为大写字母
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>       
        public static bool IsUpper(this char c)
        {
            if (c >= 'A' && c <= 'Z')
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// 退换
        /// </summary>
        /// <param name="str">原字符串</param>
        /// <param name="startIndex">开始索引</param>
        /// <param name="startLength">选择的长度</param>
        /// <param name="value">需要插入的值</param>
        /// <returns></returns>
        public static string Insert(this string str, int startIndex, int startLength, string value)
        {
            if (str.Length < startIndex)
                return str;

            if (startLength <= 0)
                return str.Insert(startIndex, value);

            var prefix = str.Substring(0, startIndex);

            var suffixStart = startIndex + startLength;
            if (suffixStart > str.Length)
                return $"{prefix}{value}";

            var suffix = str.Substring(startIndex + startLength);
            return $"{prefix}{value}{suffix}";
        }

        /// <summary>
        /// 匹配
        /// </summary>
        /// <param name="data"></param>
        /// <param name="match"></param>
        /// <returns></returns>
        public static bool IsMatchRegular(this string data, string match)
        {
            bool result;
            try
            {
                Regex regex = new Regex(match);
                result = regex.IsMatch(data);
            }
            catch
            {
                result = false;
            }
            return result;
        }
    }
}
