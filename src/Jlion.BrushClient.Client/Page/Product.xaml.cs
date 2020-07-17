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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Jlion.BrushClient.Client.OnRender;
using Jlion.BrushClient.Framework;
using Jlion.BrushClient.UControl;

namespace Jlion.BrushClient.Client
{
    /// <summary>
    /// Tool.xaml 的交互逻辑
    /// </summary>
    public partial class Product : Page
    {
        OnProductRender _onProductRender;
        public Product()
        {
            InitializeComponent();
            _onProductRender = AutofacManage.GetService<OnProductRender>();
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            var _ = _onProductRender.RenderListAsync(this);
        }


        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as ButtonEx;
            var tagId = btn.Tag.ToInt32OrDefault(0);
            var _ = _onProductRender.RenderPayAsync(this, tagId);
        }

        #region 激活
        private void btnActive_Click(object sender, RoutedEventArgs e)
        {
            var _ = _onProductRender.RenderPayActiveAsync(this);
        }
        #endregion
    }
}
