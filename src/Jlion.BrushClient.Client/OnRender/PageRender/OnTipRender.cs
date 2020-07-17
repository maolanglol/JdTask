using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Jlion.BrushClient.Application.Plugins;
using Jlion.BrushClient.Framework;
using Jlion.BrushClient.UControl;
using static Jlion.BrushClient.Client.Enums.OptionEnums;

namespace Jlion.BrushClient.Client.OnRender
{
    /// <summary>
    /// 消息提示框
    /// </summary>
    public class OnTipRender : OnSingleRender
    {
        #region 属性定义
        private OnTimerPlugins _onTimerPlugins;
        private OnControlRender _onControlRender;
        private readonly object _loker = new object();
        #endregion

        public OnTipRender(OnTimerPlugins onTimerPlugins, OnControlRender onControlRender)
        {
            _onTimerPlugins = onTimerPlugins;
            _onControlRender = onControlRender;
        }

        /// <summary>
        /// Tip 弹窗  
        /// <UControl:TipBox Source="..\resource\image\error.png" Content="测试"  Margin="0,-500,0,0" Panel.ZIndex="99999" ResultType="ERROR"></UControl:TipBox>
        /// </summary>
        /// <param name="msg"></param>
        public void ExecuteTip(Grid bodyPanel, string msg, EnumResultType resultType = EnumResultType.ERROR)
        {
            try
            {
                _onControlRender.ThreadExecuteUI(() =>
                {
                    if (bodyPanel.Visibility != Visibility.Visible)
                        return;
                    var _tipBox = new TipBox();
                    _tipBox.Source = new Uri(resultType.GetDescription());
                    _tipBox.Content = msg;
                    _tipBox.ResultType = resultType;

                    if (_tipBox.Parent != null)
                        return;

                    var key = TimeKeyStatics.TipKey();
                    bodyPanel.RegisterName(key, _tipBox);
                    if (bodyPanel.Children.Contains(_tipBox))
                        return;

                    bodyPanel.Children.Add(_tipBox);

                    _onTimerPlugins.Add(key, () =>
                    {
                        _onControlRender.ThreadExecuteUI(() =>
                        {
                            bodyPanel.Children.Remove(_tipBox);
                        });
                        return true;
                    }, 800, DateTime.Now,1);
                });
            }
            catch (Exception ex)
            {
                TextHelper.Error("ExecuteTip 异常", ex);
            }
        }

        public void ExecuteConfim(Grid bodyPanel, string msg, RoutedEventHandler closeAction, RoutedEventHandler submitAction, string title = "提示")
        {
            try
            {
                _onControlRender.ThreadExecuteUI(() =>
                {
                    var TipConfirm = new TipConfirmBox();
                    TipConfirm.Margin = new Thickness(0, 0, 0, 0);
                    TipConfirm.BoxType = BoxType.NoIconSmall;
                    TipConfirm.Title = title;
                    TipConfirm.Content = msg;
                    TipConfirm.IconVisibility = Visibility.Collapsed;
                    TipConfirm.SubmitName = "确认";
                    TipConfirm.CloseName = "关闭";
                    if (closeAction != null)
                    {
                        TipConfirm.OnClose += closeAction;
                    }
                    if (submitAction != null)
                    {
                        TipConfirm.OnSubmit += submitAction;
                    }

                    bodyPanel.RegisterName("tipConfirm" + Guid.NewGuid().ToString().RemoveChar('-'), TipConfirm);
                    bodyPanel.Children.Add(TipConfirm);
                });
            }
            catch (Exception ex)
            {
                TextHelper.Error("ExecuteConfim 异常", ex);
            }
        }

        public void ExecuteConfimIcon(Grid bodyPanel, string msg, IconEnumType icon, RoutedEventHandler closeAction, RoutedEventHandler submitAction,
            string title = "提示", string closeName = "关闭", string submitName = "确定")
        {
            try
            {
                _onControlRender.ThreadExecuteUI(() =>
                {
                    var TipConfirm = new TipConfirmBox();
                    TipConfirm.Margin = new Thickness(0, 0, 0, 0);

                    TipConfirm.Title = title;
                    TipConfirm.WidthIcon = 90;
                    TipConfirm.Content = msg;
                    TipConfirm.BoxType = BoxType.Small;
                    TipConfirm.Icon = new BitmapImage(new Uri(icon.GetDescription(), UriKind.RelativeOrAbsolute));
                    TipConfirm.CloseName = closeName;
                    TipConfirm.SubmitName = submitName;
                    TipConfirm.OnClose += closeAction;
                    TipConfirm.OnSubmit += submitAction;

                    bodyPanel.RegisterName("tipConfirm" + Guid.NewGuid().ToString().RemoveChar('-'), TipConfirm);
                    bodyPanel.Children.Add(TipConfirm);
                });
            }
            catch (Exception ex)
            {
                TextHelper.Error("ExecuteConfim 异常", ex);
            }
        }

        public void ExecuteConfimBigIcon(Grid bodyPanel, string msg, IconEnumType icon, RoutedEventHandler closeAction, RoutedEventHandler submitAction,
           string title = "提示", string closeName = "关闭", string submitName = "确定", int IconWidth = 90)
        {
            try
            {
                _onControlRender.ThreadExecuteUI(() =>
                {
                    #region 判断是否存在TipConfirmBox
                    if (IsExistsTipConfirmBox(bodyPanel))
                    {
                        return;
                    }
                    #endregion

                    var TipConfirm = new TipConfirmBox();
                    var height = bodyPanel.RenderSize.Height;
                    TipConfirm.Margin = new Thickness(0, 0, 0, 0);

                    TipConfirm.BoxHeight = Convert.ToInt32(bodyPanel.RenderSize.Height);
                    TipConfirm.BoxWidth = Convert.ToInt32(bodyPanel.RenderSize.Width);
                    TipConfirm.Title = title;
                    TipConfirm.TitleSize = 20;
                    TipConfirm.WidthIcon = IconWidth;
                    TipConfirm.Content = msg;
                    TipConfirm.Icon = new BitmapImage(new Uri(icon.GetDescription(), UriKind.RelativeOrAbsolute));
                    TipConfirm.CloseName = closeName;
                    TipConfirm.SubmitName = submitName;
                    TipConfirm.OnClose += closeAction;
                    if (string.IsNullOrEmpty(closeName))
                    {
                        TipConfirm.ButtonWidth = 310;
                    }
                    else
                    {
                        TipConfirm.ButtonWidth = 145;
                    }
                    TipConfirm.BoxType = BoxType.Big;
                    TipConfirm.OnSubmit += submitAction;


                    bodyPanel.RegisterName("tipConfirm" + Guid.NewGuid().ToString().RemoveChar('-'), TipConfirm);
                    bodyPanel.Children.Add(TipConfirm);
                });
            }
            catch (Exception ex)
            {
                TextHelper.Error("ExecuteConfim 异常", ex);
            }
        }

        public void ExecuteConfim(Grid bodyPanel, string msg, RoutedEventHandler submitAction)
        {
            try
            {
                _onControlRender.ThreadExecuteUI(() =>
                {
                    var TipConfirm = new TipConfirmBox();
                    TipConfirm.Margin = new Thickness(0, 0, 0, 0);
                    TipConfirm.BoxType = BoxType.NoIconSmall;
                    TipConfirm.IconVisibility = Visibility.Collapsed;
                    TipConfirm.Title = "";
                    TipConfirm.Content = msg;
                    TipConfirm.CloseName = "";
                    TipConfirm.SubmitName = "确认";
                    TipConfirm.OnSubmit += submitAction;

                    bodyPanel.RegisterName("tipConfirm" + Guid.NewGuid().ToString().RemoveChar('-'), TipConfirm);
                    bodyPanel.Children.Add(TipConfirm);
                });
            }
            catch (Exception ex)
            {
                TextHelper.Error("ExecuteConfim 异常", ex);
            }
        }

        public void ExecuteConfimTextBox(Grid bodyPanel, string title, string content, string value, RoutedEventHandler submitAction, RoutedEventHandler closeAction)
        {
            try
            {
                _onControlRender.ThreadExecuteUI(() =>
                {
                    var TipConfirm = new TipConfirmBox();
                    TipConfirm.Margin = new Thickness(0, 0, 0, 0);
                    TipConfirm.BoxType = BoxType.TextBox;
                    TipConfirm.IconVisibility = Visibility.Collapsed;
                    TipConfirm.Title = title;
                    TipConfirm.Content = content;
                    TipConfirm.CloseName = "取消";
                    TipConfirm.Value = value;
                    TipConfirm.SubmitName = "确认";
                    TipConfirm.OnSubmit += submitAction;
                    TipConfirm.OnClose += closeAction;
                    TipConfirm.TextInputName = "txbInputAmount";

                    bodyPanel.RegisterName("tipConfirm" + Guid.NewGuid().ToString().RemoveChar('-'), TipConfirm);
                    bodyPanel.Children.Add(TipConfirm);
                });
            }
            catch (Exception ex)
            {
                TextHelper.Error("ExecuteConfim 异常", ex);
            }
        }

        public void ExecuteRefundPwdConfirm(Grid bodyPanel, string title, string content, string value, RoutedEventHandler submitAction, RoutedEventHandler closeAction)
        {
            try
            {
                _onControlRender.ThreadExecuteUI(() =>
                {
                    var TipConfirm = new TipConfirmBox();
                    TipConfirm.Margin = new Thickness(0, 0, 0, 0);
                    TipConfirm.BoxType = BoxType.RefundPwdConfirm;
                    TipConfirm.IconVisibility = Visibility.Collapsed;
                    TipConfirm.Title = title;
                    TipConfirm.Content = content;
                    TipConfirm.CloseName = "取消";
                    TipConfirm.Value = value;
                    TipConfirm.SubmitName = "确认";
                    TipConfirm.OnSubmit += submitAction;
                    TipConfirm.OnClose += closeAction;
                    TipConfirm.TextInputName = "txbInputAmount";
                    TipConfirm.LabelPasswordValue = "密码：";
                    TipConfirm.TextInputPassword = "txbInputPassword";

                    bodyPanel.RegisterName("tipRefoudConfirm" + Guid.NewGuid().ToString().RemoveChar('-'), TipConfirm);
                    bodyPanel.Children.Add(TipConfirm);
                });
            }
            catch (Exception ex)
            {
                TextHelper.Error("ExecuteRefundPwdConfirm 异常", ex);
            }
        }

        public void ExecuteConfimTextBoxCombine(Grid bodyPanel, string title, string content, string value, RoutedEventHandler submitAction, RoutedEventHandler closeAction, bool isPassword)
        {
            if (isPassword)
                ExecuteRefundPwdConfirm(bodyPanel, title, content, value, submitAction, closeAction);
            else
                ExecuteConfimTextBox(bodyPanel, title, content, value, submitAction, closeAction);
        }
        private bool IsExistsTipConfirmBox(Grid grid)
        {
            if ((grid?.Children?.Count ?? 0) <= 0)
                return false;

            foreach (var item in grid.Children)
            {
                if (item is TipConfirmBox)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
