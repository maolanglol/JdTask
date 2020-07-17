using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Jlion.BrushClient.Application.Plugins;
using Jlion.BrushClient.Client.OnRender;
using Jlion.BrushClient.Framework;

namespace Jlion.BrushClient.Client
{
    /// <summary>
    /// 悬浮窗口
    /// </summary>
    public partial class SuspensionMain : Window
    {
        private OnSuspensionMainRender _onSuspensionMainRender;
        private OnAppSettingPlugins _onAppSettingPlugins;

        private Main main;

        public SuspensionMain()
        {
            InitializeComponent();
            _onSuspensionMainRender = AutofacManage.GetService<OnSuspensionMainRender>();
            _onAppSettingPlugins = AutofacManage.GetService<OnAppSettingPlugins>();
            this.Opacity =Convert.ToDouble( _onAppSettingPlugins?.Opacity ?? 1);
            this.Topmost = this.Opacity >= 1;
            main = AutofacManage.GetService<Main>();
            //_onSuspensionMainRender.RenderTopMost(this);
            this.ShowInTaskbar = false;
        }

        public void SetMain()
        {
            this.main = AutofacManage.GetService<Main>();
        }

        public void InitAccount()
        {
            this.main.ShowAmount();
        }

        #region 窗口其他事件
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _onSuspensionMainRender.DragMove(this);
        }

        private void Window_LocationChanged(object sender, EventArgs e)
        {
            _onSuspensionMainRender.RenderHidden(this);
        }

        private void Window_MouseUpClick(object sender, MouseButtonEventArgs e)
        {
            _onSuspensionMainRender.ExecuteMenu(this);
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            _onSuspensionMainRender.DragMove(this);
        }
        #endregion

        #region 菜单
        private void BtnAbount_Click(object sender, RoutedEventArgs e)
        {
            this.main.ShowWindow();
            this.main.BtnAbout_Click(null, null);
            _onSuspensionMainRender.RenderHidden(this);
        }

        #region 退出程序
        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            this.main.Exit();
        }
        #endregion

        private void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            var content = this.BtnStart.Content;
            if (content.Equals("启动"))
            {
                this.main.ShowWindow();
                this.main.StartService();
                return;
            }
            this.main.StopService();
        }

        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            this.main.Logout();
        }
        #endregion

        private void BtnAccount_Click(object sender, RoutedEventArgs e)
        {
            this.main.ShowWindow();
            this.main.BtnAccount_Click(null, null);
            _onSuspensionMainRender.RenderHidden(this);
        }

        private void BtnWithdraw_Click(object sender, RoutedEventArgs e)
        {
            this.main.ShowWindow();
            this.main.BtnWithdrawRecord_Click(null, null);
            _onSuspensionMainRender.RenderHidden(this);
        }

        private void BtnAccountRecord_Click(object sender, RoutedEventArgs e)
        {
            this.main.ShowWindow();
            this.main.BtnAccountRecord_Click(null, null);
            _onSuspensionMainRender.RenderHidden(this);
        }
    }
}
