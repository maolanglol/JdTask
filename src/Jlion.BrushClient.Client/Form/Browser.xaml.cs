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
    /// Browser.xaml 的交互逻辑
    /// </summary>
    public partial class Browser : Window
    {
        OnBrowserRender _onBrowserRender;
        OnTipRender _onTipRender;
        public Browser()
        {
            InitializeComponent();
            _onBrowserRender = AutofacManage.GetService<OnBrowserRender>();
            _onTipRender = AutofacManage.GetService<OnTipRender>();

            _onBrowserRender.RenderInit(this);
            clientBrowser.Navigating += WebBrowserMain_Navigating;
        }

        public void StopService()
        {
            try
            {
                var main = AutofacManage.GetService<Main>();
                var susman = AutofacManage.GetService<SuspensionMain>();
                _onBrowserRender?.Stop();
                main.btnStart.Content = "启动";
                susman.BtnStart.Content = "启动";
            }
            catch (Exception ex)
            {
                TextHelper.ErrorAsync($"StopService 异常:{ex.Message}", ex);
            }
        }

        public void StartService()
        {
            var main = AutofacManage.GetService<Main>();
            var susman = AutofacManage.GetService<SuspensionMain>();
            _onBrowserRender.Start();
            main.btnStart.Content = "停止";
            susman.BtnStart.Content = "停止";
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
            try
            {
                FieldInfo fi = typeof(WebBrowser).GetField("_axIWebBrowser2", BindingFlags.Instance | BindingFlags.NonPublic);
                if (fi != null)
                {
                    object browser = fi.GetValue(webBrowser);
                    if (browser != null)
                        browser.GetType().InvokeMember("Silent", BindingFlags.SetProperty, null, browser, new object[] { silent });
                }
            }
            catch (Exception ex)
            { 
            
            }
            
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _onBrowserRender.DragMove(this);
        }

    }
}
