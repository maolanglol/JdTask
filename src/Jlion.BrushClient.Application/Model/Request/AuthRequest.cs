using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jlion.BrushClient.Application.Model
{
    public class AuthRequest
    {
        /// <summary>
        /// 手机号码
        /// </summary>
        public string Mobile { set; get; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { set; get; }

    }
}
