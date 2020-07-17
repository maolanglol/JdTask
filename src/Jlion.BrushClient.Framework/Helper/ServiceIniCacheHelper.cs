using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jlion.BrushClient.Framework;
using Jlion.BrushClient.Framework.Helper;
using System.IO;

namespace Jlion.BrushClient.Framework.Helper
{
    /// <summary>
    /// 缓存配置文件帮助类
    /// </summary>
    public class ServiceIniCacheHelper
    {
       
        #region 定义常量
        private const string sectionName = "baseInfo";
        #endregion

        #region 公共方法
        /// <summary>
        /// 写入登录信息
        /// </summary>
        /// <param name="sectionName"></param>
        /// <param name="value">账号信息(数字json)</param>
        /// <param name="appKey">加密key->软件唯一编码</param>
        /// <param name="root"></param>
        public static void WriteLoginInfo(List<ServiceIniLoginResponse> list, string appKey, string sectionName = sectionName)
        {
            var json = JsonConvert.SerializeObject(list);
            var value = AESHelper.Encrypt(json, appKey);

            ServiceIniHelper.Write(sectionName, "loginInfo", value, root: DocumentDefintion.DocumentPath, fileType: Framework.Enums.EnumIniFileType.Cache);
        }

        /// <summary>
        /// 缓存中添加登录信息
        /// </summary>
        /// <param name="serviceIniLoginResponse"></param>
        /// <param name="appKey"></param>
        /// <param name="root"></param>
        /// <param name="sectionName"></param>
        public static void AddLoginInfo(ServiceIniLoginResponse serviceIniLoginResponse, string appKey, string sectionName = sectionName)
        {
            var list = new List<ServiceIniLoginResponse>();
            var resp = ReadLoginInfo(appKey, sectionName);
            if (resp == null)
            {
                list.Add(serviceIniLoginResponse);
                WriteLoginInfo(list, appKey, sectionName);
                return;
            }

            #region 清理历史的自动登录
            if (serviceIniLoginResponse.IsAutoLogin)
            {
                ClearAutoLogin(resp);
            }
            #endregion

            #region 如果以及存在则修改
            var cacheLoginExists = resp.Exists((match) =>
            {
                return match.UserName.Equals(serviceIniLoginResponse.UserName);
            });
            if (cacheLoginExists)
            {
                resp.ForEach(item =>
                {
                    if (item.UserName.Equals(serviceIniLoginResponse.UserName))
                    {
                        item.Password = serviceIniLoginResponse.Password;
                        item.IsAutoLogin = serviceIniLoginResponse.IsAutoLogin;
                    }
                });
                WriteLoginInfo(resp, appKey, sectionName);
                return;
            }
            #endregion

            resp.Add(serviceIniLoginResponse);
            WriteLoginInfo(resp, appKey, sectionName);
        }

        /// <summary>
        /// 写入登录信息
        /// </summary>
        /// <param name="sectionName"></param>
        /// <param name="value">账号信息(数字json)</param>
        /// <param name="appKey">加密key->软件唯一编码</param>
        /// <param name="root"></param>
        public static List<ServiceIniLoginResponse> ReadLoginInfo(string appKey, string sectionName = sectionName)
        {
            var encryptInfo = ServiceIniHelper.Read(sectionName, "loginInfo", root: DocumentDefintion.DocumentPath, fileType: Framework.Enums.EnumIniFileType.Cache, length: 5000);
            if (string.IsNullOrEmpty(encryptInfo))
                return null;
            var value = AESHelper.Decrypt(encryptInfo, appKey);
            if (string.IsNullOrEmpty(value))
                return null;
            if (value.Equals(encryptInfo))
                return null;

            return JsonConvert.DeserializeObject<List<ServiceIniLoginResponse>>(value);
        }

        ///// <summary>
        ///// 写入accessToken
        ///// </summary>
        ///// <param name="sectionName"></param>
        ///// <param name="merchantId"></param>
        ///// <param name="root"></param>
        //public static void WriteAccessToken(int userId,string accessToken, string sectionName = sectionName)
        //{
        //    ServiceIniHelper.Write(sectionName, $"accessToken_{userId}", accessToken.ToString(), root: DocumentDefintion.DocumentPath, fileType: Framework.Enums.EnumIniFileType.Cache);
        //}

        ///// <summary>
        ///// 获得accessToken
        ///// </summary>
        ///// <param name="sectionName"></param>
        ///// <returns></returns>
        //public static string ReadAccessToken(int userId,string sectionName = sectionName)
        //{
        //    try
        //    {
        //        return ServiceIniHelper.Read(sectionName, $"accessToken_{userId}", root: DocumentDefintion.DocumentPath, fileType: Framework.Enums.EnumIniFileType.Cache);
        //        //var result = 0;
        //        //int.TryParse(merchantIdStr, out result);
        //        //return result;
        //    }
        //    catch
        //    {
        //        return "";
        //    }
        //}


        ///// <summary>
        ///// 写入refreshToken
        ///// </summary>
        ///// <param name="sectionName"></param>
        ///// <param name="merchantId"></param>
        ///// <param name="root"></param>
        //public static void WriteRefreshToken(string refreshToken, string sectionName = sectionName)
        //{
        //    ServiceIniHelper.Write(sectionName, "refreshToken", refreshToken.ToString(), root: DocumentDefintion.DocumentPath, fileType: Framework.Enums.EnumIniFileType.Cache);
        //}

        ///// <summary>
        ///// 获得refreshToken
        ///// </summary>
        ///// <param name="sectionName"></param>
        ///// <returns></returns>
        //public static string ReadRefreshToken(string sectionName = sectionName)
        //{
        //    try
        //    {
        //        return ServiceIniHelper.Read(sectionName, "refreshToken", root: DocumentDefintion.DocumentPath, fileType: Framework.Enums.EnumIniFileType.Cache);
        //        //return merchantIdStr.ToLower().Equals("true");
        //    }
        //    catch
        //    {
        //        return "";
        //    }
        //}


        #endregion

        #region 私有方法
        private static void ClearAutoLogin(List<ServiceIniLoginResponse> list)
        {
            if ((list?.Count ?? 0) <= 0)
                return;

            list.ForEach(item =>
            {
                item.IsAutoLogin = false;
            });
        }
        #endregion
    }

    /// <summary>
    /// 登录用户缓存信息
    /// </summary>
    public class ServiceIniLoginResponse
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { set; get; }

        /// <summary>
        /// 用户密码
        /// </summary>
        public string Password { set; get; }

        /// <summary>
        /// 是否自动登录
        /// </summary>
        public bool IsAutoLogin { set; get; }
    }
}
