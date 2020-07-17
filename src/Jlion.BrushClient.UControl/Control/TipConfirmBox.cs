using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
    ///     <MyNamespace:TipConfirmBox/>
    ///
    /// </summary>
    [TemplatePart(Name = "PATH_CloseButtonHost", Type = typeof(ButtonBase))]
    [TemplatePart(Name = "PATH_SubmitButtonHost", Type = typeof(ButtonBase))]
    public class TipConfirmBox : Button
    {
        private ButtonEx submitButton;
        private TextBox inputAmountBox;
        private PasswordBox inuputPasswordBox;
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            var closeButton = GetChild(this, this.CloseName);
            if (closeButton != null)
            {
                if (string.IsNullOrEmpty(this.CloseName))
                {
                    closeButton.Visibility = Visibility.Collapsed;
                }
                else
                {
                    closeButton.Click += HandleOnCloseClick;
                }
            }

            submitButton = GetChild(this, this.SubmitName);
            if (submitButton != null)
            {
                submitButton.Focus();
                submitButton.Click += HandleOnSubmitClick;
            }

            inputAmountBox = GetChildTextBox(this, this.TextInputName);
            inuputPasswordBox = GetChildPasswordBox(this, this.TextInputPassword);
            if (inputAmountBox != null)
            {
                inputAmountBox.Focus(); 
                inputAmountBox.PreviewKeyDown += InputAmountBox_PreviewKeyDown;
            }
            if (inuputPasswordBox != null)
            {
                inuputPasswordBox.PreviewKeyDown += InuputPasswordBox_PreviewKeyDown;
                inuputPasswordBox.PasswordChanged += InuputPasswordBox_PasswordChanged;
            }
        }

        private void InuputPasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            TextInputPasswordValue = inuputPasswordBox.Password;
        }

        private void InputAmountBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                this.Value = inputAmountBox.Text.ToString();
                if (inuputPasswordBox == null)
                {
                    this.HandleOnSubmitClick(null, null);
                }
                else
                {
                    inuputPasswordBox.Focus();
                }
            }
        }

        private void InuputPasswordBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                this.HandleOnSubmitClick(null, null);
            } 
        }
        private ButtonEx GetChild(DependencyObject parent, string name)
        {
            try
            {
                var numVisuals = VisualTreeHelper.GetChildrenCount(parent);
                for (int i = 0; i < numVisuals; i++)
                {
                    var v = (DependencyObject)VisualTreeHelper.GetChild(parent, i);
                    var child = v as ButtonEx;

                    if (child == null)
                    {
                        var item = GetChild(v, name);
                        if (item != null)
                            return item;
                    }
                    else
                    {
                        if (child.Content.Equals(name))
                        {
                            return child;
                        }
                    }
                }
            }
            catch { }
            return null;
        }

        private TextBox GetChildTextBox(DependencyObject parent, string name)
        {
            try
            {
                var numVisuals = VisualTreeHelper.GetChildrenCount(parent);
                for (int i = 0; i < numVisuals; i++)
                {
                    var v = (DependencyObject)VisualTreeHelper.GetChild(parent, i);
                    var child = v as TextBox;

                    if (child == null)
                    {
                        var item = GetChildTextBox(v, name);
                        if (item != null)
                            return item;
                    }
                    else
                    {
                        if (child.Tag.Equals(name))
                        {
                            return child;
                        }
                    }
                }
            }
            catch { }
            return null;
        }

        private PasswordBox GetChildPasswordBox(DependencyObject parent, string name)
        {
            try
            {
                var numVisuals = VisualTreeHelper.GetChildrenCount(parent);
                for (int i = 0; i < numVisuals; i++)
                {
                    var v = (DependencyObject)VisualTreeHelper.GetChild(parent, i);
                    var child = v as PasswordBox;

                    if (child == null)
                    {
                        var item = GetChildPasswordBox(v, name);
                        if (item != null)
                            return item;
                    }
                    else
                    {
                        if (child.Tag.Equals(name))
                        {
                            return child;
                        }
                    }
                }
            }
            catch { }
            return null;
        }

        #region 事件定义

        [Category("Behavior")]
        public static readonly RoutedEvent OnCloseEvent = EventManager.RegisterRoutedEvent("OnClose", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(TipConfirmBox));
        public event RoutedEventHandler OnClose
        {
            add { AddHandler(OnCloseEvent, value); }
            remove { RemoveHandler(OnCloseEvent, value); }
        }

        [Category("Behavior")]
        public static readonly RoutedEvent OnSubmitEvent = EventManager.RegisterRoutedEvent("OnSubmit", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(TipConfirmBox));
        public event RoutedEventHandler OnSubmit
        {
            add { AddHandler(OnSubmitEvent, value); }
            remove { RemoveHandler(OnSubmitEvent, value); }
        }

        //[Category("Behavior")]
        //public static readonly KeyEventHandler OnPreviewKeyUpEvent = EventManager.RegisterRoutedEvent("OnPreviewKeyUp", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(TipConfirmBox));
        //public event RoutedEventHandler OnPreviewKeyUp
        //{
        //    add { AddHandler(OnPreviewKeyUpEvent, value); }
        //    remove { RemoveHandler(OnPreviewKeyUpEvent, value); }
        //}

        public void HandleOnCloseClick(object sender, RoutedEventArgs e)
        {
            this.RaiseEvent(new RoutedEventArgs(TipConfirmBox.OnCloseEvent, this));
        }

        public void HandleOnSubmitClick(object sender, RoutedEventArgs e)
        {
            this.RaiseEvent(new RoutedEventArgs(TipConfirmBox.OnSubmitEvent, this));
        }

        //public void HandleOnPreviewKeyUp(object sender, KeyEventArgs e)
        //{
        //    this.RaiseEvent(new RoutedEventArgs(TipConfirmBox.OnPreviewKeyUpEvent, this));
        //}


        //触发点击事件，这里注册你控件上的那个点击按钮事件
        void Close_Click(object sender, RoutedEventArgs e)
        {
            e.RoutedEvent = OnCloseEvent;
            e.Source = this;
            this.RaiseEvent(e);
        }

        void Submit_Click(object sender, RoutedEventArgs e)
        {

            e.RoutedEvent = OnSubmitEvent;
            e.Source = this;
            this.RaiseEvent(e);
        }
        #endregion

        static TipConfirmBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TipConfirmBox), new FrameworkPropertyMetadata(typeof(TipConfirmBox)));
        }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(string), typeof(TipConfirmBox), new PropertyMetadata(null));

        /// <summary>
        /// 标题
        /// </summary>
        public string Value
        {
            get { return (string)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(string), typeof(TipConfirmBox), new PropertyMetadata(null));

        /// <summary>
        /// 关闭按钮名称
        /// </summary>
        public string CloseName
        {
            get { return (string)GetValue(CloseNameProperty); }
            set { SetValue(CloseNameProperty, value); }
        }
        public static readonly DependencyProperty CloseNameProperty = DependencyProperty.Register("CloseName", typeof(string), typeof(TipConfirmBox), new PropertyMetadata(null));

        /// <summary>
        /// 确定按钮名称
        /// </summary>
        public string SubmitName
        {
            get { return (string)GetValue(SubmitNameProperty); }
            set { SetValue(SubmitNameProperty, value); }
        }
        public static readonly DependencyProperty SubmitNameProperty = DependencyProperty.Register("SubmitName", typeof(string), typeof(TipConfirmBox), new PropertyMetadata(null));

        /// <summary>
        /// 金额输入框按钮名称
        /// </summary>
        public string TextInputName
        {
            get { return (string)GetValue(TextInputNameProperty); }
            set { SetValue(TextInputNameProperty, value); }
        }
        public static readonly DependencyProperty TextInputNameProperty = DependencyProperty.Register("TextInputName", typeof(string), typeof(TipConfirmBox), new PropertyMetadata(null));

        /// <summary>
        /// 密码输入框显示名称
        /// </summary>
        public string LabelPasswordValue
        {
            get { return (string)GetValue(LabelPasswordValueProperty); }
            set { SetValue(LabelPasswordValueProperty, value); }
        }
        public static readonly DependencyProperty LabelPasswordValueProperty = DependencyProperty.Register("LabelPasswordValue", typeof(string), typeof(TipConfirmBox), new PropertyMetadata(null));

        /// <summary>
        /// 密码输入框id
        /// </summary>
        public string TextInputPassword
        {
            get { return (string)GetValue(TextInputPasswordProperty); }
            set { SetValue(TextInputPasswordProperty, value); }
        }
        public static readonly DependencyProperty TextInputPasswordProperty = DependencyProperty.Register("TextInputPassword", typeof(string), typeof(TipConfirmBox), new PropertyMetadata(null));

        /// <summary>
        /// 密码输入框值
        /// </summary>
        public string TextInputPasswordValue
        {
            get { return (string)GetValue(TextInputPasswordValueProperty); }
            set { SetValue(TextInputPasswordValueProperty, value); }
        }
        public static readonly DependencyProperty TextInputPasswordValueProperty = DependencyProperty.Register("TextInputPasswordValue", typeof(string), typeof(TipConfirmBox), new PropertyMetadata(null));


        /// <summary>
        /// 图标
        /// </summary>
        public ImageSource Icon
        {
            get { return (ImageSource)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        public static readonly DependencyProperty IconProperty = DependencyProperty.Register("Icon", typeof(ImageSource), typeof(TipConfirmBox), new PropertyMetadata(null));

        /// <summary>
        /// 图标宽度
        /// </summary>
        public int WidthIcon
        {
            get { return (int)GetValue(WidthIconProperty); }
            set { SetValue(WidthIconProperty, value); }
        }
        public static readonly DependencyProperty WidthIconProperty = DependencyProperty.Register("WidthIcon", typeof(int), typeof(TipConfirmBox), new PropertyMetadata(null));


        public Visibility IconVisibility
        {
            get { return (Visibility)GetValue(IconVisibilityProperty); }
            set { SetValue(IconVisibilityProperty, value); }
        }
        public static readonly DependencyProperty IconVisibilityProperty = DependencyProperty.Register("IconVisibility", typeof(Visibility), typeof(TipConfirmBox), new PropertyMetadata(Visibility.Visible));

        /// <summary>
        /// 宽度
        /// </summary>
        public int BoxWidth
        {
            get { return (int)GetValue(BoxWidthProperty); }
            set { SetValue(BoxWidthProperty, value); }
        }
        public static readonly DependencyProperty BoxWidthProperty = DependencyProperty.Register("BoxWidth", typeof(int), typeof(TipConfirmBox), new PropertyMetadata());

        /// <summary>
        /// 宽度
        /// </summary>
        public int BoxHeight
        {
            get { return (int)GetValue(BoxHeightProperty); }
            set { SetValue(BoxHeightProperty, value); }
        }
        public static readonly DependencyProperty BoxHeightProperty = DependencyProperty.Register("BoxHeight", typeof(int), typeof(TipConfirmBox), new PropertyMetadata());

        /// <summary>
        /// 宽度
        /// </summary>
        public int TitleSize
        {
            get { return (int)GetValue(TitleSizeProperty); }
            set { SetValue(TitleSizeProperty, value); }
        }
        public static readonly DependencyProperty TitleSizeProperty = DependencyProperty.Register("TitleSize", typeof(int), typeof(TipConfirmBox), new PropertyMetadata());


        /// <summary>
        /// 宽度
        /// </summary>
        public int ButtonWidth
        {
            get { return (int)GetValue(ButtonWidthProperty); }
            set { SetValue(ButtonWidthProperty, value); }
        }
        public static readonly DependencyProperty ButtonWidthProperty = DependencyProperty.Register("ButtonWidth", typeof(int), typeof(TipConfirmBox), new PropertyMetadata());

        /// <summary>
        /// 宽度
        /// </summary>
        public BoxType BoxType
        {
            get { return (BoxType)GetValue(BoxTypeProperty); }
            set { SetValue(BoxTypeProperty, value); }
        }
        public static readonly DependencyProperty BoxTypeProperty = DependencyProperty.Register("BoxType", typeof(BoxType), typeof(TipConfirmBox), new PropertyMetadata(BoxType.Small));
    }

    public enum BoxType
    {
        Big,
        Small,
        NoIconSmall,
        TextBox,
        /// <summary>
        /// 退款含密码确认
        /// </summary>
        RefundPwdConfirm
    }
}
