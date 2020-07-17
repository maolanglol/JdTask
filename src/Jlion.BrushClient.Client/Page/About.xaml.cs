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
using Jlion.BrushClient.Client.OnRender;
using Jlion.BrushClient.Framework;
using Jlion.BrushClient.Framework.Helper;

namespace Jlion.BrushClient.Client
{
    /// <summary>
    /// About.xaml 的交互逻辑
    /// </summary>
    public partial class About : Page
    {
        private OnAboutRender _onAboutRender;
        public About()
        {
            InitializeComponent();

            _onAboutRender = AutofacManage.GetService<OnAboutRender>();
            _onAboutRender.Render(this);
        }
    }

}
