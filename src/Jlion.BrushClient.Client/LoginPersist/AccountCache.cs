using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Jlion.BrushClient.Application.Model;
using Jlion.BrushClient.Client.Helper;
using Jlion.BrushClient.Framework;

namespace Jlion.BrushClient.Client.LoginPersist
{
    public class AccountCache
    {

        public string UserName { set; get; }

        public string Password { set; get; }

        public long UserId { set; get; }

        public bool IsMember { set; get; }

        public string AccessToken { set; get; }

        public string RefreshToken { set; get; }

        /// <summary>
        /// 系统设置
        /// </summary>
        public SystemSettingsResponse SystemSettings { set; get; }

        #region 存储
        #region 单例
        private static AccountCache _Persist;
        private static readonly object _lock = new object();
        public static AccountCache Persist
        {
            get
            {
                if (_Persist != null)
                    return _Persist;
                lock (_lock)
                {
                    if (_Persist != null)
                        return _Persist;

                    _Persist = new AccountCache();
                    return _Persist;
                }
            }
        }
        #endregion

        public static void SetLogin(AccountCache accountCache)
        {
            _Persist = accountCache;
        }


        public static void Clear()
        {
            _Persist = null;
        }

     
        #endregion
    }
}
