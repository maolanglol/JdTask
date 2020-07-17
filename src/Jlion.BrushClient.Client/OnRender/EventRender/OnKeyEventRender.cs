using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;
using WP.Device.Plugins.Kernel;

namespace Jlion.BrushClient.Client.OnRender
{
    /// <summary>
    /// 键盘快捷键渲染
    /// </summary>
    public class OnKeyEventRender : OnSingleRender
    {
        private Queue<string> keyQueue = new Queue<string>();
        private Setting _setting;

        public OnKeyEventRender()
        {
        }

        public void SetSettingPage(Setting setting)
        {
            _setting = setting;
        }


        #region 多个组合键
        public void TextBox_KeyPress(object sender, System.Windows.Input.KeyEventArgs e)
        {
            System.Windows.Forms.SendKeys.SendWait("{tab}");
        }

        public void TextBox_PreviewKeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            var textBox = sender as TextBox;
            var tabIndex = textBox.TabIndex;

            if (keyQueue.Count > 0)
            {
                var str = keyQueue.Dequeue();
                if (string.IsNullOrEmpty(textBox.Text))
                    textBox.Text += str;
                else
                    textBox.Text += "+" + str;
            }

            if (e.Key == Key.Delete)
            {
                return;
            }
            #region 自动切换到下一个TextBox
            if (keyQueue.Count <= 0)
            {
                var selectTextBox = _setting.accountEntityBox.Where(item => item.accountMode == Enums.OptionEnums.AccountMode.Keyboard && item.textBox.TabIndex == tabIndex + 1).FirstOrDefault();
                selectTextBox?.textBox?.Focus();
            }
            #endregion
        }

        public void TextBox_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            var textBox = sender as TextBox;
            if (e.Key == Key.ImeProcessed)
            {
                e.Handled = true;
                return;
            }
            if (e.Key == Key.Delete)
            {
                e.Handled = true;
                textBox.Clear();
                keyQueue?.Clear();
                return;
            }
            if (keyQueue.Count <= 0)
            {
                textBox.Text = "";
            }
            e.Handled = true;
            if (!keyQueue.Contains(e.Key.ToString()))
            {
                keyQueue.Enqueue(e.Key.ToString());
            }
        }
        #endregion

        public void TextBox_PreviewOneKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            var textBox = sender as TextBox;
            if (e.Key == Key.ImeProcessed)
            {
                return;
            }
            if (e.Key == Key.LeftCtrl || e.Key == Key.LeftShift || e.Key == Key.LWin || e.Key == Key.LeftAlt
                || e.Key == Key.RightCtrl || e.Key == Key.RightShift || e.Key == Key.RWin || e.Key == Key.RightAlt
                || (e.Key == Key.System && e.SystemKey != Key.F10))
            {
                return;
            }
            // f10 Key值为 system
            if (e.Key == Key.System && e.SystemKey == Key.F10)
                textBox.Text = e.SystemKey.ToString();
            else
                textBox.Text = e.Key.ToString();
            e.Handled = true;
        }

        #region 坐标textBox
        /// <summary>
        /// 只能输入Delete 并且清空
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void TextBox_PreviewPointKeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            var textBox = sender as TextBox;
            if (e.Key != Key.Delete)
            {
                e.Handled = true;
                return;
            }
            textBox.Text = "0";
        }

        /// <summary>
        /// 只能输入Delete 并且清空
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void TextBox_PreviewPointKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            var textBox = sender as TextBox;
            if (e.Key != Key.Delete)
            {
                e.Handled = true;
                return;
            }
        }
        #endregion
    }
}
