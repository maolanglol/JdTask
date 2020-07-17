using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jlion.BrushClient.Application.Model
{
    public class AuthResponse
    {
        /// <summary>
        /// 登录AccessToken
        /// </summary>
        public string AccessToken { set; get; }

        /// <summary>
        /// 刷新Token
        /// </summary>
        public string RefreshToken { set; get; }

        public UserItemResponse User { set; get; }
    }
}
