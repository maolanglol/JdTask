using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jlion.BrushClient.Client.Model.Response
{
    public class UpgradeResponse
    {
        /// <summary>
        /// 是否强制更新
        /// </summary>
        public bool IsForce { set; get; }

        /// <summary>
        /// 是否需要更新
        /// </summary>
        public bool IsUpgrade { set; get; }

        /// <summary>
        /// 是否需要解压
        /// </summary>
        public bool IsZip { set; get; }

        /// <summary>
        /// 描述信息
        /// </summary>
        public string Mark { set; get; }
    }
}
