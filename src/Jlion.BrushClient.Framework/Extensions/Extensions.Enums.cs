using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using Jlion.BrushClient.Framework.Enums;

namespace Jlion.BrushClient.Framework
{
    /// <summary>
    /// 扩展方法
    /// </summary>
    public static partial class ObjectExtensions
    {
        /// <summary>
        /// 获取枚举的描述信息
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public static string GetDescription(this Enum @this)
        {
            if (@this == null)
                return @this.ToString();
            var type = @this.GetType();
            var f = type.GetField(@this.ToString());
            if (f == null)
                return @this.ToString();
            var da = (DescriptionAttribute)Attribute.GetCustomAttribute(f, typeof(DescriptionAttribute));
            return da?.Description ?? @this.ToString();
        }

        /// <summary>
        /// 获取枚举属性
        /// </summary>
        /// <typeparam name="TAttribute"></typeparam>
        /// <param name="this"></param>
        /// <returns></returns>
        public static TAttribute GetEnumAttribute<TAttribute>(this Enum @this) where TAttribute : Attribute
        {
            if (@this == null)
                return default(TAttribute);

            var type = @this.GetType();
            var f = type.GetField(@this.ToString());
            if (f == null)
                return default(TAttribute);
            var da = (TAttribute)Attribute.GetCustomAttribute(f, typeof(TAttribute));
            return da;
        }

        /// <summary>
        /// 将枚举转成整型
        /// </summary>
        public static int ToInt(this Enum self)
        {
            return int.Parse(self.ToString("D"));
        }

        /// <summary>
        /// 遍历所有的枚举
        /// </summary>
        /// <param name="this"></param>
        /// <returns></returns>
        public static Dictionary<string, int> GetEnums(this Enum @this)
        {
            var result = new Dictionary<string, int>();
            var typeOf = @this.GetType();
            foreach (int item in Enum.GetValues(typeOf))
            {
                string strName = Enum.GetName(typeOf, item);//获取名称

                var f = typeOf.GetField(strName);
                if (f == null)
                {
                    result.Add(strName, item);
                    continue;
                }
                var da = (DescriptionAttribute)Attribute.GetCustomAttribute(f, typeof(DescriptionAttribute));
                result.Add((da?.Description ?? @this.ToString()), item);
            }
            return result;
        }
    }
}
