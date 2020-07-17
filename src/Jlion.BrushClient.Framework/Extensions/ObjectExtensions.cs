using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Jlion.BrushClient.Framework
{
    /// <summary>
    /// 扩展方法
    /// </summary>
    public static partial class ObjectExtensions
    {
        #region 转换方法
        
        /// <summary>
        /// 排除@this为空的情况
        /// </summary>
        /// <param name="this"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static string ToStringOrDefault(this string @this, string defaultValue)
        {
            if (string.IsNullOrEmpty(@this))
                return defaultValue;
            return @this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="this"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static int ToInt32OrDefault(this object @this, int defaultValue)
        {
            if (@this == null)
                return defaultValue;

            if (!int.TryParse(@this.ToString(), out int value))
                return defaultValue;

            return value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="this"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static decimal ToDecimalOrDefault(this object @this, decimal defaultValue)
        {
            if (@this == null)
                return defaultValue;

            if (!decimal.TryParse(@this.ToString(), out decimal value))
                return defaultValue;

            return value;
        }

        /// <summary>
        /// 时间戳
        /// </summary>
        /// <param name="this"></param>
        /// <returns></returns>
        public static DateTime FromTimeStamp(this long @this)
        {
            if (@this <= 0)
                return DateTime.MinValue;
            var startTime = TimeZoneInfo.ConvertTime(new DateTime(1970, 1, 1, 8, 0, 0), TimeZoneInfo.Local);
            var dt = startTime.AddSeconds(@this);
            return dt;
        }

        /// <summary>
        /// An ICollection&lt;T&gt; extension method that removes value that satisfy the predicate.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="predicate">The predicate.</param>
        public static void RemoveWhere<T>(this ICollection<T> @this, Func<T, bool> predicate)
        {
            if (@this == null || @this.Count <= 0)
                return;
            var list = @this.Where(predicate).ToList();
            foreach (T item in list)
            {
                @this.Remove(item);
            }
        }
        #endregion


        /// <summary>
        /// KeyValue
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static KeyValuePair<string, string> AsPair(this string key, string value)
        {
            return new KeyValuePair<string, string>(key, value);
        }

        /// <summary>
        /// 集合中是否有数据
        /// </summary>
        /// <returns>true/false</returns>
        public static bool HasData<T>(this IEnumerable<T> self)
        {
            return self != null && self.Any();
        }

        /// <summary>
        /// 是否是http/https开头的rul
        /// </summary>
        /// <returns>true/false</returns>
        public static bool IsStartWithHttp(this string self)
        {
            if (string.IsNullOrWhiteSpace(self))
            {
                return false;
            }
            //简单判断，不采用正则。
            return self.StartsWith("http://", StringComparison.OrdinalIgnoreCase)
                || self.StartsWith("https://", StringComparison.OrdinalIgnoreCase);
        }


        /// <summary>
        /// 字符串分割
        /// </summary>
        /// <param name="input"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        public static string[] ToSplit(this string input, char c = ',', StringSplitOptions options = StringSplitOptions.RemoveEmptyEntries)
        {
            if (string.IsNullOrEmpty(input))
                return new string[0];
            return input.ToSplit(new char[] { c }, options);
        }

        /// <summary>
        /// 字符串分割
        /// </summary>
        /// <param name="input"></param>
        /// <param name="chars"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static string[] ToSplit(this string input, char[] chars, StringSplitOptions options = StringSplitOptions.RemoveEmptyEntries)
        {
            if (string.IsNullOrEmpty(input))
                return new string[0];
            return input.Split(chars, options);
        }

        /// <summary>
        /// 字符串分割
        /// </summary>
        /// <param name="input"></param>
        /// <param name="chars"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static string[] ToSplit(this string input, string[] str, StringSplitOptions options = StringSplitOptions.RemoveEmptyEntries)
        {
            if (string.IsNullOrEmpty(input))
                return new string[0];
            return input.Split(str, options);
        }

        /// <summary>
        /// 分批扩展
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="divisionCount"></param>
        /// <returns></returns>
        public static Dictionary<int, List<T>> ToDictionary<T>(this List<T> list, int divisionCount)
        {
            var data = new Dictionary<int, List<T>>();
            if (list == null)
                return null;

            if (list.Count > divisionCount)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    var index = i / divisionCount;
                    if (data.ContainsKey(index))
                    {
                        data[index].Add(list[i]);
                    }
                    else
                    {
                        data.Add(index, new List<T>() { list[i] });
                    }
                }
            }
            else
            {
                data.Add(1, list);
            }
            return data;
        }

        /// <summary>
        /// 清除字典中值为空的项。
        /// </summary>
        /// <param name="dict">待清除的字典</param>
        /// <returns>清除后的字典</returns>
        public static IDictionary<string, object> CleanupDictionary(this IDictionary<string, object> dict)
        {
            IDictionary<string, object> newDict = new Dictionary<string, object>(dict.Count);
            IEnumerator<KeyValuePair<string, object>> dem = dict.GetEnumerator();

            while (dem.MoveNext())
            {
                string name = dem.Current.Key;

                object value = dem.Current.Value;

                if (value != null && value is int && (int)value != default(int))
                {
                    newDict.Add(name, value);
                }
                else if (value != null && value is decimal && (decimal)value != default(decimal))
                {
                    newDict.Add(name, value);
                }
                else if (value != null && value is string && (string)value != default(string))
                {
                    newDict.Add(name, value);
                }
                else if (value != null && value is long && (long)value != default(long))
                {
                    newDict.Add(name, value);
                }

                else if (value != null)
                {
                    bool isAllNull = true;
                    foreach (var item in value.GetType().GetProperties())
                    {
                        if (item.GetValue(value,null) != null)
                        {
                            isAllNull = false;
                        }
                    }
                    if (!isAllNull)
                    {
                        newDict.Add(name, JsonConvert.SerializeObject(value));
                    }
                }
            }
            return newDict;
        }

        /// <summary>
        /// 将指定的字典转换成按属性名称排序过的查询参数
        /// </summary>
        /// <param name="dict"></param>
        /// <param name="urlencode"></param>
        /// <param name="exclude">排除的参数</param>
        /// <returns></returns>
        public static string ToSortQueryParameters(this IDictionary<string, object> dict, bool urlencode = false, params string[] exclude)
        {
            var sortDict = dict.OrderBy(m => m.Key).Select(m =>
            {
                if (m.Value == null || exclude.Contains(m.Key))
                    return null;
                if (m.Value is string && (string)m.Value == string.Empty)
                {
                    return null;
                }
                var val = m.Value.ToString();
                var encodeVal = "";
                if (urlencode)
                {
                    encodeVal += HttpUtility.UrlEncode(val.ToString());
                }
                else
                {
                    encodeVal = val;
                }
                return $"{m.Key}={encodeVal}";
            }).Where(m => m != null);
            var result = string.Join("&", sortDict.ToArray());
            return result;
        }
    }
}
