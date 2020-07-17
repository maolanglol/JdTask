using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using Jlion.BrushClient.Client.Helper;
using Jlion.BrushClient.Client.OnRender;
using Jlion.BrushClient.Framework;

namespace Jlion.BrushClient.Client
{
    /// <summary>
    /// Login.xaml 的交互逻辑
    /// </summary>
    public partial class Login : Window
    {
        #region 构造定义
        private OnLoginRender _onLoginRender;
        public bool IsAuto = true;


        public Login()
        {
            InitializeComponent();
            _onLoginRender = AutofacManage.GetService<OnLoginRender>();
        }
        #endregion

        #region 登录
        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            var _ = _onLoginRender.RenderLoginAsync(this);
        }
        #endregion

        #region 注册
        private void btnReg_Click(object sender, RoutedEventArgs e)
        {
            var _ = _onLoginRender.RenderRegAsync(this);
        }
        #endregion

        #region 登陆UI显示
        public void ShowLogin()
        {
            this.Show();
            var main = AutofacManage.GetService<Main>();
            main.HideWindow();
        }
        #endregion

        #region 事件
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            _onLoginRender.DragMove(this);
        }


        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            CloseWindow();
        }
        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter)
                return;

            this.BtnLogin_Click(null, null);
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var deleteBtn = sender as Button;
        }
        #endregion

        #region public Method
        public void CloseWindow()
        {
            System.Windows.Application.Current.Shutdown();
        }
        #endregion

        private void MerchantId_GotFocus(object sender, RoutedEventArgs e)
        {
            var content = this.MerchantId.Text.Trim();
            if (content.Equals("注册时输入邀请码,登录无需输入"))
                this.MerchantId.Text = "";
        }

        private void MerchantId_LostFocus(object sender, RoutedEventArgs e)
        {
            var content = this.MerchantId.Text.Trim();
            if (content.Equals(""))
                this.MerchantId.Text = "注册时输入邀请码,登录无需输入";
        }
    }
}
