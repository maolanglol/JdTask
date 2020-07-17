using Jlion.BrushClient.Client.OnRender;
using Jlion.BrushClient.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Jlion.BrushClient.Client
{
    /// <summary>
    /// PageBrowser.xaml 的交互逻辑
    /// </summary>
    public partial class PageBrowser : Page
    {
        OnBrowserRender _onBrowserRender;
        OnTipRender _onTipRender;
        public PageBrowser()
        {
            InitializeComponent();
            _onBrowserRender = AutofacManage.GetService<OnBrowserRender>();
            _onTipRender = AutofacManage.GetService<OnTipRender>();
            //_onTipRender.ExecuteTip(this.BodyPanel, "任务完成奖励10元");

            _onBrowserRender.RenderInit(this);
            clientBrowser.Navigating += WebBrowserMain_Navigating;
        }



        void WebBrowserMain_Navigating(object sender, NavigatingCancelEventArgs e)
        {
            SetWebBrowserSilent(sender as WebBrowser, true);
        }

        /// <summary>
        /// 设置浏览器静默，不弹错误提示框
        /// </summary>
        /// <param name="webBrowser">要设置的WebBrowser控件浏览器</param>
        /// <param name="silent">是否静默</param>
        private void SetWebBrowserSilent(WebBrowser webBrowser, bool silent)
        {
            FieldInfo fi = typeof(WebBrowser).GetField("_axIWebBrowser2", BindingFlags.Instance | BindingFlags.NonPublic);
            if (fi != null)
            {
                object browser = fi.GetValue(webBrowser);
                if (browser != null)
                    browser.GetType().InvokeMember("Silent", BindingFlags.SetProperty, null, browser, new object[] { silent });
            }
        }
    }
}
