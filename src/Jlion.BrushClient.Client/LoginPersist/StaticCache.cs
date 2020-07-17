using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Jlion.BrushClient.Client.Enums.OptionEnums;

namespace Jlion.BrushClient.Client.LoginPersist
{
    public class StaticCache
    {
        public List<string> authCodeList = new List<string>();

        /// <summary>
        /// 增加付款码到本地内存
        /// </summary>
        /// <param name="authCode"></param>
        public static void AddAuthCode(string authCode)
        {

            if (Static.authCodeList.Contains(authCode))
                return;

            Static.authCodeList.Add(authCode);
        }

        public static bool IsContains(string authCode)
        {
            return Static.authCodeList.Contains(authCode);
        }

        public static bool RemoveAuthCode(string authCode)
        {
            if (Static.authCodeList.Contains(authCode))
            {
                return Static.authCodeList.Remove(authCode);
            }
            return false;
        }

        #region 单例
        private static StaticCache _Static;
        private static readonly object _lock = new object();
        public static StaticCache Static
        {
            get
            {
                if (_Static != null)
                    return _Static;
                lock (_lock)
                {
                    if (_Static != null)
                        return _Static;

                    _Static = new StaticCache();
                    return _Static;
                }
            }
        }
        #endregion
    }
}
