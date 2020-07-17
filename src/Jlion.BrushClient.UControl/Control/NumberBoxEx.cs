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
using Jlion.BrushClient.Framework;

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
    ///     <MyNamespace:NumberBoxEx/>
    ///
    /// </summary>
    public class NumberBoxEx : TextBox
    {

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            this.LostFocus += new RoutedEventHandler(NumericBox_LostFocus);
            this.KeyDown += new KeyEventHandler(NumberBox_KeyDown);
        }
        //public NumberBoxEx()
        //{
        //    this.LostFocus += new RoutedEventHandler(NumericBox_LostFocus);
        //    this.KeyDown += new KeyEventHandler(NumberBox_KeyDown);
        //    //this.Text = "0";
        //}

        static NumberBoxEx()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NumberBoxEx), new FrameworkPropertyMetadata(typeof(NumberBoxEx)));
        }

        #region 属性

        public static readonly DependencyProperty DecimalProperty = DependencyProperty.Register("DecimalPlaces", typeof(ushort), typeof(NumberBoxEx));
        /// <summary>
        /// 允许输入的小数点个数
        /// </summary>
        [Description("小数位数,0表示不能输入小数"), Category("输入设置")]
        public ushort DecimalPlaces
        {
            get
            {
                return (ushort)GetValue(DecimalProperty);
            }
            set
            {
                SetValue(DecimalProperty, value);
            }
        }

        public static readonly DependencyProperty MaximumProperty = DependencyProperty.Register("MaxNum", typeof(decimal), typeof(NumberBoxEx));
        /// <summary>
        /// 可输入的最大值
        /// </summary>
        [Description("可输入的最大值"), Category("输入设置"), DefaultValue(0)]
        public decimal MaxNum
        {
            get
            {
                return (decimal)GetValue(MaximumProperty);
            }
            set
            {
                SetValue(MaximumProperty, value);
            }
        }

        public static readonly DependencyProperty MinimumProperty = DependencyProperty.Register("MinNum", typeof(decimal), typeof(NumberBoxEx));
        /// <summary>
        /// 可输入的最小值
        /// </summary>
        [Description("可输入的最小值"), Category("输入设置"), DefaultValue(0)]
        public decimal MinNum
        {
            get
            {
                return (decimal)GetValue(MinimumProperty);
            }
            set
            {
                SetValue(MinimumProperty, value);
            }
        }

        #endregion

        #region 内部事件
        /// <summary>
        /// 丢失焦点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NumericBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextCheck();
        }

        private void NumberBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.ImeProcessed)
            {
                e.Handled = true;
                return;
            }
            if (DecimalPlaces == 0 && (e.Key == Key.Decimal || e.Key == Key.OemPeriod))
            {
                e.Handled = true;
                return;
            }
            if (e.Key == Key.Decimal || e.Key == Key.OemPeriod)
            {
                e.Handled = false;
                return;
            }
            int __ppos = this.Text.IndexOf('.');
            if (DecimalPlaces > 0 && (e.Key == Key.Decimal || e.Key == Key.OemPeriod))
            {
                if (__ppos > 0)
                {
                    e.Handled = true;
                    return;
                }
                if (__ppos == -1 || this.Text.Length - __ppos - 1 < DecimalPlaces)
                {
                    e.Handled = false;
                    return;
                }
                e.Handled = true;
                return;
            }
            if ((e.Key >= Key.D0 && e.Key <= Key.D9) || (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9))
            {
                e.Handled = false;
                return;
            }
            e.Handled = true;
            return;
        }

        #endregion

        #region 输入限制

        /// <summary>
        /// 输入合法性检查
        /// </summary>
        private bool TextCheck()
        {
            try
            {
                decimal d = Convert.ToDecimal(this.Text.Trim());//转换为数字
                if (this.DecimalPlaces > 0)
                {
                    d = Math.Round(d, this.DecimalPlaces);//舍入小数位数
                }
                if (!(this.MinNum == 0 && this.MaxNum == 0))
                {
                    if (d < this.MinNum)//输入的值小于最小值
                    {
                        d = this.MinNum;
                    }
                    else if (d > this.MaxNum)//输入的值大于最大值
                    {
                        d = this.MaxNum;
                    }
                }
                this.Text = d.ToString();
            }
            catch
            {
                this.Text = this.MinNum.ToString();
                return false;
            }
            return true;
        }
        #endregion
    }
}
