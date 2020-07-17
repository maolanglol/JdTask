using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Jlion.BrushClient.Client.OnRender;

namespace Jlion.BrushClient.Client
{
    /// <summary>
    /// Transparent.xaml 的交互逻辑
    /// </summary>
    public partial class Transparent : Window
    {
        private OnTransparentRender _onTransparentRender;
        public Transparent(OnTransparentRender onTransparentRender)
        {
            InitializeComponent();
            _onTransparentRender = onTransparentRender;
        }

        public void ShowSuccess(double x, double y, int width, int height,string money)
        {
            _onTransparentRender.RenderShow(this, x, y, width, height);
            _onTransparentRender.RenderSuccess(this, money);
        }

        public void ShowError(double x, double y, int width, int height)
        {
            _onTransparentRender.RenderShow(this, x, y, width, height);

        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
