using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jlion.BrushClient.Framework.Model
{
    public class UpgradeModel
    {
        /// <summary>
        /// 更新地址
        /// </summary>
        public string Url { set; get; }

        /// <summary>
        /// 更新描述
        /// </summary>
        public string Description { set; get; }

        /// <summary>
        /// 退换目标文件名称
        /// </summary>
        public string FileName { set; get; }

        /// <summary>
        /// 是否是主程序
        /// </summary>
        public bool IsMainProcess { set; get; }
    }
}
