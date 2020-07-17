using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// IpAddressControl.xaml 的交互逻辑
    /// </summary>
    public partial class IpAddressControl : UserControl
    {
        #region Fields

        /// <summary>
        /// IP地址的依赖属性
        /// </summary>
        public static readonly DependencyProperty IPProperty = DependencyProperty.Register("IP", typeof(string), typeof(IpAddressControl),
                new FrameworkPropertyMetadata(DefaultIP, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, IPChangedCallback));

        /// <summary>
        /// IP地址的正则表达式
        /// </summary>
        public static readonly Regex IpRegex = new Regex(@"^((2[0-4]\d|25[0-5]|(1\d{2})|([1-9]?[0-9]))\.){3}(2[0-4]\d|25[0-4]|(1\d{2})|([1-9][0-9])|([1-9]))$");

        /// <summary>
        /// 默认IP地址
        /// </summary>
        private const string DefaultIP = "127.0.0.1";

        private static readonly Regex PartIprRegex = new Regex(@"^(\.?(2[0-4]\d|25[0-5]|(1\d{2})|([1-9]?[0-9]))\.?)+$");

        /// <summary>
        /// 输入框的集合
        /// </summary>
        private readonly List<TextBox> numbericTextBoxs = new List<TextBox>();

        /// <summary>
        /// 当前活动的输入框
        /// </summary>
        private TextBox currentNumbericTextBox;

        #endregion Fields

        public IpAddressControl()
        {
            InitializeComponent();
            this.numbericTextBoxs.Add(this.IpPartItem1);
            this.numbericTextBoxs.Add(this.IpPartItem2);
            this.numbericTextBoxs.Add(this.IpPartItem3);
            this.numbericTextBoxs.Add(this.IpPartItem4);
            this.KeyUp += this.IpAddressControlKeyUp;

            this.UpdateParts(this);

            //foreach (var numbericTextBox in this.numbericTextBoxs)
            //{
            //    numbericTextBox.PreviewTextChanged += this.NumbericTextBox_OnPreviewTextChanged;
            //}

            foreach (var numbericTextBox in this.numbericTextBoxs)
            {
                numbericTextBox.TextChanged += this.TextBoxBase_OnTextChanged;
            }
        }
        

        #region Constructors


        #endregion Constructors

        #region Properties

        public string IP
        {
            get
            {
                return (string)GetValue(IPProperty);
            }

            set
            {
                SetValue(IPProperty, value);
            }
        }

        #endregion Properties

        #region Private Methods

        /// <summary>
        /// IP值改变的响应
        /// </summary>
        /// <param name="dependencyObject"></param>
        /// <param name="dependencyPropertyChangedEventArgs"></param>
        private static void IPChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            if (dependencyPropertyChangedEventArgs.NewValue == null)
            {
                throw new Exception("IP can not be null");
            }

            var control = dependencyObject as IpAddressControl;
            if (control != null)
            {
                control.UpdateParts(control);
            }
        }

        private void UpdateParts(IpAddressControl control)
        {
            string[] parts = control.IP.Split(new[] { '.' });
            control.IpPartItem1.Text = parts[0];
            control.IpPartItem2.Text = parts[1];
            control.IpPartItem3.Text = parts[2];
            control.IpPartItem4.Text = parts[3];
        }

        #endregion Private Methods

        #region Event Handling

        /// <summary>
        /// 按键松开的响应
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void IpAddressControlKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.OemPeriod || e.Key == Key.Decimal)
            {
                if (this.currentNumbericTextBox != null)
                {
                    int index = this.numbericTextBoxs.IndexOf(this.currentNumbericTextBox);
                    int next = (index + 1) % this.numbericTextBoxs.Count;
                    this.numbericTextBoxs[next].Focus();
                    this.numbericTextBoxs[next].SelectionStart = this.numbericTextBoxs[next].Text.Length;
                }
            }
        }

        /// <summary>
        /// 获得焦点的响应
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBox_OnGotFocus(object sender, RoutedEventArgs e)
        {
            this.currentNumbericTextBox = e.OriginalSource as TextBox;
        }

        private void NumbericTextBox_OnPreviewTextChanged(object sender, TextChangedEventArgs e)
        {
            var numbericTextBox = sender as TextBox;
            Contract.Assert(numbericTextBox != null);

            if (PartIprRegex.IsMatch(numbericTextBox.Text))
            {
                var ips = numbericTextBox.Text.Split('.');

                if (ips.Length == 1)
                {
                    return;
                }

                int index = this.numbericTextBoxs.IndexOf(numbericTextBox);
                int pointer2Ips = 0;
                for (int i = index; i < this.numbericTextBoxs.Count; i++)
                {
                    while (pointer2Ips < ips.Length && string.IsNullOrEmpty(ips[pointer2Ips]))
                    {
                        pointer2Ips++;
                    }

                    if (pointer2Ips >= ips.Length)
                    {
                        return;
                    }

                    this.numbericTextBoxs[i].Text = ips[pointer2Ips];
                    pointer2Ips++;
                }
            }
        }

        private void TextBoxBase_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            var ip = string.Format(
                "{0}.{1}.{2}.{3}",
                this.IpPartItem1.Text,
                this.IpPartItem2.Text,
                this.IpPartItem3.Text,
                this.IpPartItem4.Text);
            if (IpRegex.IsMatch(ip))
            {
                this.IP = ip;
            }
        }
        #endregion     Event Handling
    }
}
