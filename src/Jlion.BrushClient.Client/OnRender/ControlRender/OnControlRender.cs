using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Jlion.BrushClient.Framework;
using Jlion.BrushClient.UControl;
using static Jlion.BrushClient.Client.ConstDefintion;
using static Jlion.BrushClient.Client.Enums.OptionEnums;

namespace Jlion.BrushClient.Client.OnRender
{
    /// <summary>
    /// 页面上控件绑定渲染操作
    /// </summary>
    public class OnControlRender : OnSingleRender
    {
        public OnControlRender() { }

        /// <summary>
        /// 绑定ComboBox 组件
        /// </summary>
        /// <param name="comboBox"></param>
        public void BindItemsControl<T>(ItemsControl control, List<T> data)
        {
            if (control == null)
                return;

            if (data != null)
                control.ItemsSource = data;
        }

        /// <summary>
        /// 绑定ItemsControl 控件数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="control"></param>
        /// <param name="data"></param>
        public void BindItemsControl<T>(ItemsControl control, ObservableCollection<T> data)
        {
            if (control == null)
                return;

            if (data != null)
                control.ItemsSource = data;
        }

        /// <summary>
        /// 绑定ComboBox 组件
        /// </summary>
        /// <param name="control"></param>
        /// <param name="data"></param>
        public void BindItemsControl(ItemsControl control, Dictionary<string, int> data)
        {
            if (control == null)
                return;

            if (data != null)
                control.ItemsSource = data;
        }

        /// <summary>
        /// 控件DataContext 绑定
        /// </summary>
        /// <param name="element"></param>
        /// <param name="obj"></param>
        public void BindFrameworkElement(FrameworkElement element, object obj)
        {
            if (element == null)
                return;

            element.DataContext = obj;
        }

        /// <summary>
        /// 渲染内容
        /// </summary>
        /// <param name="control"></param>
        /// <param name="data"></param>
        public void RenderControlContent(ContentControl control, string data)
        {
            if (control == null)
                return;

            control.Content = data;
        }

        /// <summary>
        /// 查找StackPanel 中的所有TextBox
        /// </summary>
        /// <param name="stackPanel"></param>
        /// <returns></returns>
        public List<TextBox> FindChildrenTextBox(StackPanel stackPanel)
        {
            var resp = new List<TextBox>();
            foreach (var control in stackPanel.Children)
            {
                if (control is TextBox)
                {
                    resp.Add(control as TextBox);
                }
                else if (control is StackPanel)
                {
                    resp.AddRange(FindChildrenTextBox(control as StackPanel));
                }
                else
                {
                    continue;
                }
            }
            return resp;
        }

        /// <summary>
        /// 线程操作UI 渲染公共方法
        /// </summary>
        /// <param name="action"></param>
        public void ThreadExecuteUI(Action<object[]> action, params object[] args)
        {
            System.Windows.Application.Current?.Dispatcher?.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new ExecuteUI(() =>
            {
                action(args);
            }));
        }

        /// <summary>
        /// 线程操作UI 渲染公共方法
        /// </summary>
        /// <param name="action"></param>
        public void ThreadExecuteUI(Action action)
        {
            System.Windows.Application.Current?.Dispatcher?.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new ExecuteUI(() =>
            {
                try
                {
                    action();
                }
                catch (Exception ex)
                {
                    TextHelper.ErrorAsync("ThreadExecuteUI 异常", ex);
                }
            }));
        }

        /// <summary>
        /// 线程操作UI 渲染公共方法
        /// </summary>
        /// <param name="action"></param>
        public void ThreadExecuteUI(Action<object> action, object args)
        {
            System.Windows.Application.Current?.Dispatcher?.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new ExecuteUI(() =>
            {
                try
                {
                    action(args);
                }
                catch (Exception ex)
                {
                    TextHelper.ErrorAsync("ThreadExecuteUI(Action<object> action, object args) 异常 args:{args}", ex);
                }
            }));
        }

        private UIElement GetUIElement(DependencyObject dObject, EnumElementType type)
        {
            try
            {
                switch (type)
                {
                    case EnumElementType.TextBox:
                        return dObject as TextBox;
                    case EnumElementType.RadioButton:
                        return dObject as RadioButton;
                    case EnumElementType.ComboBox:
                        return dObject as ComboBox;
                    case EnumElementType.CheckBox:
                        return dObject as CheckBox;
                    case EnumElementType.ButtonEx:
                        return dObject as ButtonEx;
                    default:
                        throw new ArgumentException("不支持的类型");
                }
            }
            catch {
                return null;
            }
            
        }

        /// <summary>
        /// 遍历指定的页面的所有指定的控件
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public List<UIElement> GetChild(DependencyObject parent, EnumElementType type)
        {
            var resp = new List<UIElement>();
            try
            {
                var numVisuals = VisualTreeHelper.GetChildrenCount(parent);
                for (int i = 0; i < numVisuals; i++)
                {
                    var v = (DependencyObject)VisualTreeHelper.GetChild(parent, i);
                    var child = GetUIElement(v, type);
                    if (child != null)
                    {
                        resp.Add(child);
                    }
                    if (child == null)
                    {
                        var item = GetChild(v,type);
                        if ((item?.Count ?? 0) > 0)
                        {
                            resp.AddRange(item);
                        }
                    }
                }
            }
            catch { }
            return resp;
        }

        /// <summary>
        /// textBox 获得焦点清除
        /// </summary>
        /// <param name="text"></param>
        public void RenderTextBoxFocus(TextBox text)
        {
            try
            {
                var content = text.Text.Trim().ToDecimalOrDefault(0);
                if (content == 0)
                {
                    text.Text = "";
                }
            }
            catch (Exception ex)
            {
                TextHelper.Error("RenderTextBox 异常",ex);
            }
        }

        public List<T> ConvertObject<T>(ItemCollection items) where T:class
        {
            var resp = new List<T>();
            foreach (var item in items)
            {
               var obj= item as T;
                resp.Add(obj);
            }
            return resp;
        }
    }

}
