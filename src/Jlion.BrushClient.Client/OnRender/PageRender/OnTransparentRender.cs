using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using Jlion.BrushClient.Framework;

namespace Jlion.BrushClient.Client.OnRender
{
    public class OnTransparentRender : OnSingleRender
    {
        private OnControlRender _onControlRender;
        public OnTransparentRender(OnControlRender onControlRender)
        {
            _onControlRender = onControlRender;
        }

        /// <summary>
        /// 渲染显示
        /// </summary>
        /// <param name="page"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public void RenderShow(Transparent page, double x, double y, int width, int height)
        {
            _onControlRender.ThreadExecuteUI(() =>
            {
                try
                {
                    page.Cursor = Cursors.Cross;
                    page.mainBroder.BorderBrush = System.Windows.Media.Brushes.Red;
                    page.mainBroder.BorderThickness = new Thickness(2);
                    page.mainBroder.Width = width;
                    page.mainBroder.Height = height;

                    page.mainPanel.Width = 0;
                    page.mainPanel.Height = 0;

                    page.labContent.Content = "无法识别到金额";

                    page.Left = x;
                    page.Top = y-25;
                    page.Width = width;
                    page.Height = height+25;
                    page.imgSuccess.Visibility = Visibility.Hidden;
                    if (page.Visibility == Visibility.Hidden || page.Visibility == Visibility.Collapsed)
                    {
                        page.Show();
                    }
                }
                catch (Exception ex)
                {
                    TextHelper.Error("RenderShow 异常", ex);
                }
            });
        }

        public void RenderSuccess(Transparent transparent, string money)
        {
            _onControlRender.ThreadExecuteUI(() =>
            {
                transparent.mainPanel.Width = transparent.mainBroder.Width;
                transparent.mainPanel.Height = transparent.mainBroder.Height;
                transparent.labContent.Content = $"金额{money} 元";
                transparent.imgSuccess.Visibility = Visibility.Visible;
            });
        }

        //public void RenderError(Transparent transparent)
        //{
        //    _onControlRender.ThreadExecuteUI(() =>
        //    {
        //        transparent.labContent.Content = $"无法识别金额";
        //        transparent.imgSuccess.Visibility = Visibility.Visible;
        //    });
        //}
    }
}
