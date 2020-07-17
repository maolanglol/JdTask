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

namespace Jlion.BrushClient.UControl
{
    /// <summary>
    /// 按照步骤 1a 或 1b 操作，然后执行步骤 2 以在 XAML 文件中使用此自定义控件。
    ///
    /// 步骤 1a) 在当前项目中存在的 XAML 文件中使用该自定义控件。
    /// 将此 XmlNamespace 特性添加到要使用该特性的标记文件的根 
    /// 元素中: 
    ///
    ///     xmlns:MyNamespace="clr-namespace:Jlion.BrushClient.UControl.Control"
    ///
    ///
    /// 步骤 1b) 在其他项目中存在的 XAML 文件中使用该自定义控件。
    /// 将此 XmlNamespace 特性添加到要使用该特性的标记文件的根 
    /// 元素中: 
    ///
    ///     xmlns:MyNamespace="clr-namespace:Jlion.BrushClient.UControl.Control;assembly=Jlion.BrushClient.UControl.Control"
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
    ///     <MyNamespace:RadioButtonIconEx/>
    ///
    /// </summary>
    public class RadioButtonIconEx : RadioButton
    {
        static RadioButtonIconEx()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RadioButtonIconEx), new FrameworkPropertyMetadata(typeof(RadioButtonIconEx)));
        }

        /// <summary>
        /// 控件按钮类型
        /// </summary>
        public RadioButtonType RadioButtonType
        {
            get { return (RadioButtonType)GetValue(RadioButtonTypeProperty); }
            set { SetValue(RadioButtonTypeProperty, value); }
        }

        public static readonly DependencyProperty RadioButtonTypeProperty = DependencyProperty.Register("RadioButtonType", typeof(RadioButtonType), typeof(RadioButtonIconEx), new PropertyMetadata(RadioButtonType.Normal));

        /// <summary>
        /// 选中状态前景样式
        /// </summary>
        public Brush TextForeground
        {
            get { return (Brush)GetValue(TextForegroundProperty); }
            set { SetValue(TextForegroundProperty, value); }
        }
        public static readonly DependencyProperty TextForegroundProperty = DependencyProperty.Register("TextForeground", typeof(Brush), typeof(RadioButtonIconEx), new PropertyMetadata(Brushes.LimeGreen));



        /// <summary>
        /// 选中状态前景样式
        /// </summary>
        public Brush CheckedForeground
        {
            get { return (Brush)GetValue(CheckedBackgroundProperty); }
            set { SetValue(CheckedBackgroundProperty, value); }
        }
        public static readonly DependencyProperty CheckedBackgroundProperty = DependencyProperty.Register("CheckedBackground", typeof(Brush), typeof(RadioButtonIconEx), new PropertyMetadata(Brushes.LimeGreen));
        ///// <summary>
        ///// 宽度
        ///// </summary>
        //public int Width
        //{
        //    get { return (int)GetValue(WidthProperty); }
        //    set { SetValue(WidthProperty, value); }
        //}
        //public static readonly DependencyProperty WidthProperty = DependencyProperty.Register("Width", typeof(int), typeof(RadioButtonIconEx), new PropertyMetadata());


        #region 图标相关属性
        /// <summary>
        /// 图标大小
        /// </summary>
        public int IconSize
        {
            get { return (int)GetValue(IconSizeProperty); }
            set { SetValue(IconSizeProperty, value); }
        }
        public static readonly DependencyProperty IconSizeProperty = DependencyProperty.Register("IconSize", typeof(int), typeof(RadioButtonIconEx), new PropertyMetadata());

        /// <summary>
        /// 是否选中(代替IsChecked)//TODO 主要是由于IsChecked 自定义控件后台代码获取不到问题(待研究)
        /// </summary>
        public bool IsSelected
        {
            get { return (bool)GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }
        public static readonly DependencyProperty IsSelectedProperty = DependencyProperty.Register("IsSelected", typeof(bool), typeof(RadioButtonIconEx), new PropertyMetadata());

        ///// <summary>
        ///// 图标Margin
        ///// </summary>
        //public Thickness IconMargin
        //{
        //    get { return (Thickness)GetValue(IconMarginProperty); }
        //    set { SetValue(IconMarginProperty, value); }
        //}
        //public static readonly DependencyProperty IconMarginProperty = DependencyProperty.Register("IconMargin", typeof(Thickness), typeof(RadioButtonIconEx), new PropertyMetadata());

        /// <summary>
        /// 图标
        /// </summary>
        public ImageSource Icon
        {
            get { return (ImageSource)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        public static readonly DependencyProperty IconProperty = DependencyProperty.Register("Icon", typeof(ImageSource), typeof(RadioButtonIconEx), new PropertyMetadata(null));
        #endregion
    }

    public enum RadioButtonType
    {
        Normal,
        //Icon,
        Button,
        IconText
    }
}
