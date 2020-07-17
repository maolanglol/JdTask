using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Jlion.BrushClient.Client.LoginPersist;
using Jlion.BrushClient.Client.OnRender;
using Jlion.BrushClient.Framework;
using Jlion.BrushClient.UControl;
using static Jlion.BrushClient.Client.Enums.OptionEnums;

namespace Jlion.BrushClient.Client
{
    /// <summary>
    /// Main.xaml 的交互逻辑
    /// </summary>
    public partial class Main : Window
    {
        #region 属性定义
        System.Windows.Forms.NotifyIcon notifyIcon;

        //public SuspensionMain _suspensionMain;
        //private Payment _payment;
        //private Refund _refund;

        private OnMainRender _onMainRender;
        private List<ButtonEx> buttonExList;
        #endregion

        public Main()
        {
            InitializeComponent();
            _onMainRender = AutofacManage.GetService<OnMainRender>();

            InitializeNotify();
            Init();
        }

        #region 重写
        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            var _ = _onMainRender.RenderAccountAsync(this);
        }
        #endregion


        #region 事件

        #region 菜单导航

        public void BtnAccount_Click(object sender, RoutedEventArgs e)
        {
            _onMainRender.RenderMenus(this, "../Page/Account.xaml", this.BtnAccount);
        }

        public void BtnAbout_Click(object sender, RoutedEventArgs e)
        {
            _onMainRender.RenderMenus(this, "../Page/About.xaml", this.BtnAbout);
        }

        private void BtnProduct_Click(object sender, RoutedEventArgs e)
        {
            _onMainRender.RenderMenus(this, "../Page/Product.xaml", this.BtnProduct);
        }

        public void BtnAccountRecord_Click(object sender, RoutedEventArgs e)
        {
            _onMainRender.RenderMenus(this, "../Page/AccountRecord.xaml", this.BtnAccountRecord);
        }
        public void BtnWithdrawRecord_Click(object sender, RoutedEventArgs e)
        {
            _onMainRender.RenderMenus(this, "../Page/WithdrawRecord.xaml", this.BtnWithdrawRecord);
        }
        #endregion

        #region 启动事件
        private void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            var _ = _onMainRender.Start(this);
        }
        #endregion

        #region 门店基本信息
        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {

                if (this.StoreBox.Visibility == Visibility.Collapsed || this.StoreBox.Visibility == Visibility.Hidden)
                    this.StoreBox.Visibility = Visibility.Visible;
                else
                    this.StoreBox.Visibility = Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                TextHelper.ErrorAsync("Image_MouseLeftButtonDown 异常", ex);
            }
        }
        #endregion

        #region 窗口关闭
        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            HideWindow();
        }
        #endregion

        #region 窗口拖动
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _onMainRender.DragMove(this);
        }
        #endregion

        #region 点击隐藏店铺信息悬浮窗
        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                this.StoreBox.Visibility = Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                TextHelper.ErrorAsync("Border_MouseLeftButtonDown 异常", ex);
            }
        }
        #endregion

        #endregion

        #region 对外公共方法
        /// <summary>
        /// 启动服务
        /// </summary>
        public void StartService()
        {
            var _ = _onMainRender.Start(this);
        }

        /// <summary>
        /// 停止服务
        /// </summary>
        public void StopService()
        {
            var _ = _onMainRender.Stop(this);
        }

        /// <summary>
        /// 显示窗口
        /// </summary>
        public void ShowWindow(bool isShowMain = true)
        {
            _onMainRender.RenderMainWindows(isShowMain);
        }

        public void ShowAmount()
        {
            var _ = _onMainRender.RenderAccountAsync(this);
        }

        /// <summary>
        /// 注销
        /// </summary>
        public void Logout()
        {
            this.notifyIcon?.Dispose();
            this.notifyIcon = null;
            this.Hide();
            _onMainRender.RenderLogout(this);
        }

        /// <summary>
        /// 退出
        /// </summary>
        public void Exit()
        {
            _onMainRender.ExecuteExit(this);
        }
        #endregion

        #region 私有方法
        private void Init()
        {
            buttonExList = new List<ButtonEx>();
            buttonExList.Add(this.BtnAccount);
            buttonExList.Add(this.BtnAccountRecord);
            buttonExList.Add(this.BtnWithdrawRecord);
            buttonExList.Add(this.BtnProduct);
            buttonExList.Add(this.BtnAbout);
        }

        public Style GetActiveStyle(string name)
        {
            return (Style)this.FindResource(name);
        }

        public void ClearStyle()
        {
            buttonExList?.ForEach(item =>
            {
                item.Style = GetActiveStyle("MenuButtonEx");
            });
        }
        #endregion

        #region 系统托盘
        /// <summary>
        /// 添加基础信息
        /// </summary>
        public void InitializeNotify()
        {
            try
            {
                if (notifyIcon != null)
                {
                    return;
                }
                var path = AppDomain.CurrentDomain.BaseDirectory + "resource//image//main.ico";
                notifyIcon = new System.Windows.Forms.NotifyIcon
                {
                    Icon = new Icon(path),//图标图片
                    Text = "刷任务助手",
                    Visible = true,
                };
                notifyIcon.MouseDoubleClick += OnNotifyIconDoubleClick;

                #region 添加右键菜单内容
                //实例化右键菜单
                var menu = new System.Windows.Forms.ContextMenu();
                menu.MenuItems.Add("打开", OnMenuOpenClick);
                menu.MenuItems.Add("注销", OnMenuLogoutClick);
                menu.MenuItems.Add("退出", OnMenuExitClick);
                notifyIcon.ContextMenu = menu;//设置右键弹出菜单          
                #endregion
            }
            catch (Exception ex)
            {
                TextHelper.Error("InitializeNotify 异常", ex);
            }
        }

        #region 系统托盘菜单
        private void OnMenuOpenClick(object sender, EventArgs e)
        {
            this.Show();
        }

        private void OnMenuSettingClick(object sender, EventArgs e)
        {
            this.ShowWindow();
        }

        private void OnMenuExitClick(object sender, EventArgs e)
        {
            //_onMainRender.ExecuteExit(this);
        }

        private void OnMenuLogoutClick(object sender, EventArgs e)
        {
            this.Logout();
        }
        #endregion

        /// <summary>
        /// 双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnNotifyIconDoubleClick(object sender, EventArgs e)
        {
            ShowWindow();
        }

        /// <summary>
        /// 隐藏窗口
        /// </summary>
        public void HideWindow()
        {
            this.Hide();
            this.Topmost = false;
        }
        #endregion

        #region 快捷键

        private void CommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void CommandBinding_CanExecute_1(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        #endregion
    }
}
