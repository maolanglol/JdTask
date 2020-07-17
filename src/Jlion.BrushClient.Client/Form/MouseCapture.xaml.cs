using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Jlion.BrushClient.Application.Model;
using Jlion.BrushClient.Application.Plugins;
using Jlion.BrushClient.Client.OnRender;
using WP.Device.Plugins.Kernel;
using static Jlion.BrushClient.Client.ConstDefintion;
using static WP.Device.Plugins.Kernel.BaseWin32Api;

namespace Jlion.BrushClient.Client
{
    /// <summary>
    /// MouseCapture.xaml 的交互逻辑 
    /// </summary>
    public partial class MouseCapture : Window
    {
        private OnMouseCaptureRender _onMouseCaptureRender;
        public MouseCapture(OnMouseCaptureRender mouseCaptureRender)
        {
            InitializeComponent();
            _onMouseCaptureRender = mouseCaptureRender;
        }

        /// <summary>
        /// 显示
        /// </summary>
        /// <param name="settingPage"></param>
        public void Show(MouseDownHandler mouseDownHandler)
        {
            _onMouseCaptureRender.RenderShow(this, mouseDownHandler);
        }

        #region 鼠标事件
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            _onMouseCaptureRender.RenderClose(this);
        }

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            this.Cursor = Cursors.Cross;
        }
        #endregion
    }
}
