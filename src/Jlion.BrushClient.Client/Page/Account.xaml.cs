using Jlion.BrushClient.Client.OnRender;
using Jlion.BrushClient.Framework;
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

namespace Jlion.BrushClient.Client
{
    /// <summary>
    /// Account.xaml 的交互逻辑
    /// </summary>
    public partial class Account : Page
    {
        OnAccountRender _onAccountRender;
        public Account()
        {
            InitializeComponent();
            _onAccountRender = AutofacManage.GetService<OnAccountRender>();
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            var _ = _onAccountRender.RenderAccountAsync(this);
        }

        #region 计算服务费
        private void tbAmount_KeyDown(object sender, KeyEventArgs e)
        {
            _onAccountRender.RenderHandler(this);
        }

        #endregion

        #region 申请提现
        private void btnWithdraw_Click(object sender, RoutedEventArgs e)
        {
            var _ = _onAccountRender.ExecuteWithdrawAsync(this);
        }
        #endregion
    }
}
