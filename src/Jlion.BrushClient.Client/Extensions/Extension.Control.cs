//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;
//using System.Windows;
//using System.Windows.Controls;
//using System.Windows.Data;
//using System.Windows.Input;
//using Jlion.BrushClient.Application.Model;
//using Jlion.BrushClient.Client.LoginPersist;
//using Jlion.BrushClient.Client.OnRender;
//using Jlion.BrushClient.Framework;
//using Jlion.BrushClient.UControl;
//using static Jlion.BrushClient.Client.Enums.OptionEnums;

//namespace Jlion.BrushClient.Client
//{
//    public static partial class Extensions
//    {

//        /// <summary>
//        /// 判断是否选中
//        /// </summary>
//        /// <param name="radioButtonIconEx"></param>
//        /// <returns></returns>
//        public static bool IsChecked(this RadioButtonIconEx radioButtonIconEx)
//        {
//            if (radioButtonIconEx == null)
//                return false;
//            return (radioButtonIconEx.IsChecked ?? false) || radioButtonIconEx.IsSelected;
//        }


//        public static EnumPayType GetPayType(this string data)
//        {
//            if (string.IsNullOrEmpty(data))
//            {
//                return EnumPayType.NONE;
//            }
//            if (data.Contains(EnumPayType.ALIPAY.ToString()))
//            {
//                return EnumPayType.ALIPAY;
//            }
//            if (data.Contains(EnumPayType.BESTPAY.ToString()))
//            {
//                return EnumPayType.BESTPAY;
//            }
//            if (data.Contains(EnumPayType.WXPAY.ToString()))
//            {
//                return EnumPayType.WXPAY;
//            }
//            return EnumPayType.ALIPAY;
//        }

//        public static void FocusLast(this TextBox textBox)
//        {
//            textBox.Focus();
//            textBox.SelectionStart = textBox.Text.Length;
//        }

//        #region 窗体扩展

//        #region Setting
//        /// <summary>
//        /// 获得支付类型
//        /// </summary>
//        /// <param name="page"></param>
//        /// <returns></returns>
//        public static EnumPayType GetPayType(this Setting page)
//        {
//            if (page.cmbPayType.SelectedValue == null)
//                return EnumPayType.NONE;
//            var cmbPayType = page.cmbPayType.SelectedValue as ComboBoxItem;
//            return (EnumPayType)cmbPayType.Tag.ToInt32OrDefault(0);
//            //if (page.RbAccountAlipay.IsChecked())
//            //    return EnumPayType.ALIPAY;
//            //if (page.RbAccountWeiXin.IsChecked())
//            //    return EnumPayType.WXPAY;
//            //if (page.RbAccountOther.IsChecked())
//            //    return EnumPayType.Other;

//            //return EnumPayType.NONE;
//        }

//        /// <summary>
//        /// 获得入账模式
//        /// </summary>
//        /// <param name="page"></param>
//        /// <returns></returns>
//        public static AccountMode GetAccountMode(this Setting page)
//        {
//            if (page.RbAccountKeyBoardMode.IsChecked == true)
//                return AccountMode.Keyboard;
//            if (page.RbAccountMouseMode.IsChecked == true)
//                return AccountMode.Mouse;
//            return AccountMode.Keyboard;
//        }

//        /// <summary>
//        /// 获得回填方式
//        /// </summary>
//        /// <param name="page"></param>
//        /// <returns></returns>
//        public static AccountBackMoneyType GetAccountBackMoneyType(this Setting page)
//        {
//            if (page.RbAccountPointBackType.IsChecked == true)
//                return AccountBackMoneyType.Point;
//            else
//                return AccountBackMoneyType.Keyboard;
//        }
//        #endregion

//        #region PayResult

//        /// <summary>
//        /// 支付成功
//        /// </summary>
//        /// <param name="page"></param>
//        /// <param name="payType"></param>
//        /// <param name="money"></param>
//        public static void PaySuccess(this PayResult page, EnumGatewayPayType payType, decimal money)
//        {
//            page.payType = payType;
//            page.Money = money;
//            page.IsPaySuccess = true;
//        }

//        #endregion

//        #endregion

//        #region 控件扩展
//        ///// <summary>
//        ///// 动态添加入账快捷键TextBox
//        ///// </summary>
//        ///// <param name="boxes"></param>
//        ///// <param name="data"></param>
//        ///// <param name="width"></param>
//        ///// <param name="onKeyEventRender"></param>
//        ///// <param name="step"></param>
//        //public static void AddTextBox(this Setting page, StackPanel stackPanel, InputAccountHotKeyCollection data,
//        //    int textWidth, int labelWidth, Thickness labelThickness, OnKeyEventRender onKeyEventRender, int step)
//        //{
//        //    var label = new Label() { Content = $"第{step}步：", Width = labelWidth, FontSize = 12, Margin = labelThickness };

//        //    var textBox = new TextBox() { Width = textWidth, TabIndex = step, Name = $"txbBox_{step}" };
//        //    textBox.CharacterCasing = CharacterCasing.Upper;
//        //    InputMethod.SetIsInputMethodEnabled(textBox, false);//关掉输入法
//        //    if (data.Count > (step - 1))
//        //    {
//        //        textBox.SetBinding(TextBox.TextProperty, new Binding("Key") { Source = data[step - 1] });
//        //        textBox.PreviewKeyDown += onKeyEventRender.TextBox_PreviewKeyDown;
//        //        textBox.PreviewKeyUp += onKeyEventRender.TextBox_PreviewKeyUp;
//        //    }

//        //    stackPanel.Children.Add(label);
//        //    stackPanel.Children.Add(textBox);

//        //    page.accountEntityBox.Add(new Model.AccountEntityTextBoxModel() { accountMode = AccountMode.Keyboard, textBox = textBox });
//        //}

//        ///// <summary>
//        ///// 添加X，Y 坐标定位TextBox
//        ///// </summary>
//        ///// <param name="page"></param>
//        ///// <param name="stackPanel"></param>
//        ///// <param name="step"></param>
//        ///// <param name="data"></param>
//        ///// <param name="labelWidth"></param>
//        ///// <param name="textBoxWidth"></param>
//        ///// <param name="action"></param>
//        //public static void AddTextBoxPoint(this Setting page, StackPanel stackPanel, int step,
//        //    InputAccountHotKeyCollection data, int labelWidth, int textBoxWidth,
//        //    RoutedEventHandler action, OnKeyEventRender onKeyEventRender)
//        //{
//        //    var label = new Label() { Content = $"第{step}步:", FontSize = 12, Width = labelWidth };

//        //    #region X坐标
//        //    var labelX = new Label() { Content = "X" };
//        //    var textBoxX = new TextBox() { Width = textBoxWidth, Name = $"txbAccountPointX_{step}" };
//        //    textBoxX.CharacterCasing = CharacterCasing.Upper;
//        //    InputMethod.SetIsInputMethodEnabled(textBoxX, false);//关掉输入法
//        //    if ((data?.Count ?? 0) > (step - 1))
//        //    {
//        //        textBoxX.SetBinding(TextBox.TextProperty, new Binding("X") { Source = data[step - 1] });
//        //        textBoxX.PreviewKeyUp += onKeyEventRender.TextBox_PreviewPointKeyUp;
//        //        textBoxX.PreviewKeyDown += onKeyEventRender.TextBox_PreviewPointKeyDown;
//        //    }
//        //    page.accountEntityBox.Add(new Model.AccountEntityTextBoxModel() { accountMode = AccountMode.Mouse, textBox = textBoxX });
//        //    #endregion

//        //    #region Y坐标
//        //    var labelY = new Label() { Content = "Y" };
//        //    var textBoxY = new TextBox() { Width = textBoxWidth, Name = $"txbAccountPointY_{step}" };
//        //    textBoxY.CharacterCasing = CharacterCasing.Upper;
//        //    InputMethod.SetIsInputMethodEnabled(textBoxY, false);//关掉输入法
//        //    if ((data?.Count ?? 0) > (step - 1))
//        //    {
//        //        textBoxY.SetBinding(TextBox.TextProperty, new Binding("Y") { Source = data[step - 1] });
//        //        textBoxY.PreviewKeyDown += onKeyEventRender.TextBox_PreviewPointKeyDown;
//        //        textBoxY.PreviewKeyUp += onKeyEventRender.TextBox_PreviewPointKeyUp;
//        //    }
//        //    page.accountEntityBox.Add(new Model.AccountEntityTextBoxModel() { accountMode = AccountMode.Mouse, textBox = textBoxY });
//        //    #endregion

//        //    var buttonSettings = new ButtonEx() { Width = 40, Content = "设置", Tag = $"{step}", Margin = new Thickness(5, 0, 0, 0) };
//        //    buttonSettings.Click += action;

//        //    stackPanel.Children.Add(label);
//        //    stackPanel.Children.Add(labelX);

//        //    stackPanel.RegisterName(textBoxX.Name, textBoxX);
//        //    stackPanel.Children.Add(textBoxX);

//        //    stackPanel.Children.Add(labelY);
//        //    stackPanel.RegisterName(textBoxY.Name, textBoxY);
//        //    stackPanel.Children.Add(textBoxY);
//        //    stackPanel.Children.Add(buttonSettings);
//        //}
//        #endregion
//    }
//}