using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace Jlion.BrushClient.Client
{
    public partial class ConstDefintion
    {
        #region 回调定义
        /// <summary>
        /// 修改数据回调
        /// </summary>
        /// <param name="money"></param>
        /// <param name="bitmap"></param>
        public delegate void UpdateHandler<T>(List<T> data);

        /// <summary>
        /// 修改金额回调
        /// </summary>
        /// <param name="money"></param>
        /// <param name="bitmap"></param>
        public delegate void UpdateMoneyHandler(decimal money, Bitmap bitmap = null);

        /// <summary>
        /// 鼠标按下回调
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public delegate void MouseDownHandler(int x, int y);

        /// <summary>
        /// UI线程回调
        /// </summary>
        public delegate void ExecuteUI();

        #endregion

        /// <summary>
        /// 订单查询最大超时时间
        /// </summary>
        public const int MaxSeconds = 60;

        #region 特殊键盘码
        ///// <summary>
        ///// 数字点的键盘虚拟码
        ///// </summary>
        //public static readonly int DECIMAL_VirKey = 110;
        #endregion

        #region 配置
        /// <summary>
        /// 升级组件路径
        /// </summary>
        public static string UpgradePath
        {

            get
            {
                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Wp.Device.UpgradeClient.exe");
            }
        }
        #endregion

        #region OEM 相关图片

        /// <summary>
        /// OEM 默认的名称
        /// </summary>
        public const string OemDefaultName = "收银小能手";
        
        /// <summary>
        /// OEM 默认的联系电话
        /// </summary>
        public const string OemDefaultContactTell = "400-8987-098";
        #endregion

        #region jd登陆
        public class JdUrl
        {
            /// <summary>
            /// 京东搜索地址
            /// </summary>
            public const string SearchUrl = "https://search.jd.com/Search?keyword={0}&enc=utf-8&wq={0}";

            /// <summary>
            /// 京东首页
            /// </summary>
            public const string HomeUrl = "https://jd.com";

            /// <summary>
            /// 搜索到的商品域名
            /// </summary>
            public const string ItemUrl = "https://item.jd.com/";

            public const string Parter = "(item.jd.com)/[0-9]+.html";
        }
        #endregion
    }
}
