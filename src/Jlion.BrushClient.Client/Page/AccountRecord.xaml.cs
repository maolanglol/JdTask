using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Jlion.BrushClient.Client.LoginPersist;
using Jlion.BrushClient.Client.OnRender;
using Jlion.BrushClient.Framework;

namespace Jlion.BrushClient.Client
{
    /// <summary>
    /// Order.xaml 的交互逻辑
    /// </summary>
    public partial class AccountRecord : Page
    {
        OnAcccountRecordRender _onAcccountRecordRender;
        public AccountRecord()
        {
            InitializeComponent();
            _onAcccountRecordRender = AutofacManage.GetService<OnAcccountRecordRender>();
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            this.DpStartTime.SelectedDateTime = DateTime.Parse(DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd 00:00"));
            this.DpEndTime.SelectedDateTime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd 23:59"));

            this.DoPage();
        }

        private void DoPage(int pageIndex = 1)
        {
            var _ = _onAcccountRecordRender.RenderListAsync(this, pageIndex);
        }

        #region 事件

        #region 查询
        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            this.DoPage();
        }
        #endregion

        #region 翻页
        private void LabPage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var label = sender as Label;
            var page = Convert.ToInt32(label.Tag);
            this.DoPage(page);
        }

        private void TextBox_KeyUp(object sender, KeyEventArgs e)
        {
            var textBox = sender as TextBox;
            var tag = textBox.Tag;
            try
            {
                var pageIndex = Convert.ToInt32(textBox.Text);
                var totalPage = Convert.ToInt32(tag);
                if (pageIndex > totalPage)
                {
                    return;
                }
                if (e.Key == Key.Enter)
                {
                    this.DoPage(pageIndex);
                }
            }
            catch { }
        }
        #endregion
        #endregion
    }
}
