//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Jlion.BrushClient.Framework.Helper;

//namespace Jlion.BrushClient.Client.Helper
//{
//    public class ServiceIniAsyncHelper
//    {
//        #region 定义常量
//        private const string sectionName = "systemAsync";
//        private const string productMarking = "productMarking";
//        #endregion

//        #region 公共方法
//        /// <summary>
//        /// 写入系统配置最后同步时间
//        /// </summary>
//        /// <param name="lastAsyncTime"></param>
//        /// <param name="sectionName"></param>
//        public static void WriteSystemLastTime(DateTime lastAsyncTime, string key, string sectionName = sectionName)
//        {
//            if (string.IsNullOrEmpty(sectionName))
//                throw new ArgumentNullException("sectionName is not null");

//            ServiceIniHelper.Write(sectionName, $"{key}AsyncLastTime", lastAsyncTime.ToString("yyyy-MM-dd HH:mm:ss"), root: Jlion.BrushClient.Application.ConstDefintion.DocumentPath, fileType: Framework.Enums.EnumIniFileType.Sync);
//        }

//        /// <summary>
//        /// 读取系统配置最后一次同步时间
//        /// </summary>
//        /// <param name="lastAsyncTime"></param>
//        /// <param name="sectionName"></param>
//        /// <returns></returns>
//        public static DateTime? ReadSystemLastTime(string key, string sectionName = sectionName)
//        {
//            if (string.IsNullOrEmpty(sectionName) || string.IsNullOrEmpty(key))
//                throw new ArgumentNullException("sectionName is not null or key is not null");

//            var dateTimeStr = ServiceIniHelper.Read(sectionName, $"{key}AsyncLastTime", root: Jlion.BrushClient.Application.ConstDefintion.DocumentPath, fileType: Framework.Enums.EnumIniFileType.Sync);
//            try
//            {
//                if (string.IsNullOrEmpty(dateTimeStr))
//                    return null;

//                return Convert.ToDateTime(dateTimeStr);
//            }
//            catch { }
//            return null;
//        }

//        /// <summary>
//        /// 写入系统配置最后同步时间
//        /// </summary>
//        /// <param name="lastAsyncTime"></param>
//        /// <param name="sectionName"></param>
//        public static void WriteLogUploadLastTime(long lastAsyncTime, string key, string sectionName = sectionName)
//        {
//            if (string.IsNullOrEmpty(sectionName))
//                throw new ArgumentNullException("sectionName is not null");

//            ServiceIniHelper.Write(sectionName, $"{key}AsyncLastTime", lastAsyncTime.ToString(), root: Jlion.BrushClient.Application.ConstDefintion.DocumentPath, fileType: Framework.Enums.EnumIniFileType.Sync);
//        }

//        /// <summary>
//        /// 读取系统配置最后一次同步时间
//        /// </summary>
//        /// <param name="lastAsyncTime"></param>
//        /// <param name="sectionName"></param>
//        /// <returns></returns>
//        public static long ReadLogUploadmLastTime(string key, string sectionName = sectionName)
//        {
//            if (string.IsNullOrEmpty(sectionName) || string.IsNullOrEmpty(key))
//                throw new ArgumentNullException("sectionName is not null or key is not null");

//            var dateTimeStr = ServiceIniHelper.Read(sectionName, $"{key}AsyncLastTime", root: Jlion.BrushClient.Application.ConstDefintion.DocumentPath, fileType: Framework.Enums.EnumIniFileType.Sync);
//            try
//            {
//                if (string.IsNullOrEmpty(dateTimeStr))
//                    return 0;

//                return Convert.ToInt64(dateTimeStr);
//            }
//            catch { }
//            return 0;
//        }

//        /// <summary>
//        /// 写入单品营销配置数据
//        /// </summary>
//        /// <param name="key"></param>
//        /// <param name="sectionName"></param>
//        public static void WriteMarking(string value, string sectionName = productMarking)
//        {
//            if (string.IsNullOrEmpty(sectionName))
//                throw new ArgumentNullException("sectionName is not null");

//            ServiceIniHelper.Write(sectionName, $"markingGoodsCode", value, root: Jlion.BrushClient.Application.ConstDefintion.DocumentPath, fileType: Framework.Enums.EnumIniFileType.Sync);
//        }

//        /// <summary>
//        /// 获得单品营销配置数据
//        /// </summary>
//        /// <param name="sectionName"></param>
//        /// <returns></returns>
//        public static List<string> ReadMarking(string sectionName = productMarking)
//        {
//            if (string.IsNullOrEmpty(sectionName))
//                throw new ArgumentNullException("sectionName is not null or key is not null");

//            var goodsCodes = ServiceIniHelper.Read(sectionName, $"markingGoodsCode", root: Jlion.BrushClient.Application.ConstDefintion.DocumentPath, fileType: Framework.Enums.EnumIniFileType.Sync);
//            try
//            {
//                if (string.IsNullOrEmpty(goodsCodes))
//                    return null;

//                if (goodsCodes.Contains(","))
//                    return goodsCodes.Split(',').ToList();

//                return new List<string>() { goodsCodes };
//            }
//            catch { }
//            return null;
//        }
//        #endregion
//    }
//}
