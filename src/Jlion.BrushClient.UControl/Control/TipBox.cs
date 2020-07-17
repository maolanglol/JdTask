using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace Jlion.BrushClient.UControl
{
    /// <summary>
    /// 按照步骤 1a 或 1b 操作，然后执行步骤 2 以在 XAML 文件中使用此自定义控件。
    ///
    /// 步骤 1a) 在当前项目中存在的 XAML 文件中使用该自定义控件。
    /// 将此 XmlNamespace 特性添加到要使用该特性的标记文件的根 
    /// 元素中: 
    ///
    ///     xmlns:MyNamespace="clr-namespace:Jlion.BrushClient.UControl"
    ///
    ///
    /// 步骤 1b) 在其他项目中存在的 XAML 文件中使用该自定义控件。
    /// 将此 XmlNamespace 特性添加到要使用该特性的标记文件的根 
    /// 元素中: 
    ///
    ///     xmlns:MyNamespace="clr-namespace:Jlion.BrushClient.UControl;assembly=Jlion.BrushClient.UControl"
    ///
    /// 您还需要添加一个从 XAML 文件所在的项目到此项目的项目引用，
    /// 并重新生成以避免编译错误: 
    ///
    ///     在解决方案资源管理器中右击目标项目，然后依次单击
    ///     “添加引用”->“项目”->[浏览查找并选择此项目]
    ///
    ///
    /// 步骤 2)
    /// 继续操作并在 XAML 文件中使用控件。
    ///
    ///     <MyNamespace:TipBox/>
    ///
    /// </summary>
    public class TipBox : Label
    {
        static TipBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TipBox), new FrameworkPropertyMetadata(typeof(TipBox)));
        }

        public Uri Source
        {
            get { return (Uri)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }
        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register("Source", typeof(Uri), typeof(TipBox), new PropertyMetadata(null));


        /// <summary>
        /// 是否成功
        /// </summary>
        public EnumResultType ResultType
        {
            get { return (EnumResultType)GetValue(ResultTypeProperty); }
            set { SetValue(ResultTypeProperty, value); }
        }

        public static readonly DependencyProperty ResultTypeProperty = DependencyProperty.Register("ResultType", typeof(EnumResultType), typeof(TipBox), new PropertyMetadata(EnumResultType.OK));
    }

    /// <summary>
    /// 类型枚举
    /// </summary>
    public enum EnumResultType
    {
        [Description("pack://application:,,,/Jlion.BrushClient.Client;component/resource/image/success.png")]
        OK,
        [Description("pack://application:,,,/Jlion.BrushClient.Client;component/resource/image/error.png")]
        ERROR
    }
}
