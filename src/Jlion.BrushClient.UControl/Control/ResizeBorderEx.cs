using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
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
    ///     <MyNamespace:ResizeBorderEx/>
    ///
    /// </summary>
    public class ResizeBorderEx : Label
    {
        public const int WM_SYSCOMMAND = 0x112;
        public HwndSource HwndSource;

        public Dictionary<ResizeDirection, Cursor> cursors = new Dictionary<ResizeDirection, Cursor>
        {
            {ResizeDirection.Top, Cursors.SizeNS},
            {ResizeDirection.Bottom, Cursors.SizeNS},
            {ResizeDirection.Left, Cursors.SizeWE},
            {ResizeDirection.Right, Cursors.SizeWE},
            {ResizeDirection.TopLeft, Cursors.SizeNWSE},
            {ResizeDirection.BottomRight, Cursors.SizeNWSE},
            {ResizeDirection.TopRight, Cursors.SizeNESW},
            {ResizeDirection.BottomLeft, Cursors.SizeNESW}
        };

        public enum ResizeDirection
        {
            Left = 1,
            Right = 2,
            Top = 3,
            TopLeft = 4,
            TopRight = 5,
            Bottom = 6,
            BottomLeft = 7,
            BottomRight = 8,
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.VcreditWindowBehindCode_SourceInitialized();
            this.VcreditWindowBehindCode_Loaded();
        }

        static ResizeBorderEx()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ResizeBorderEx), new FrameworkPropertyMetadata(typeof(ResizeBorderEx)));
        }

        private void VcreditWindowBehindCode_SourceInitialized()
        {
            this.HwndSource = PresentationSource.FromVisual(this) as HwndSource;
        }

        private Rectangle GetChild(DependencyObject parent, string name)
        {
            try
            {
                var numVisuals = VisualTreeHelper.GetChildrenCount(parent);
                for (int i = 0; i < numVisuals; i++)
                {
                    var v = (DependencyObject)VisualTreeHelper.GetChild(parent, i);
                    var child = v as Rectangle;

                    if (child == null)
                    {
                        var item = GetChild(v, name);
                        if (item != null)
                            return item;
                    }
                    else
                    {
                        if (child.Tag.ToString().Equals(name))
                        {
                            return child;
                        }
                    }
                }
            }
            catch { }
            return null;
        }

        private void VcreditWindowBehindCode_Loaded()
        {
            var TopLeft = GetChild(this, "ResizeTopLeft");
            if (TopLeft != null)
            {
                TopLeft.MouseMove += ResizePressed;
                TopLeft.MouseDown += ResizePressed;
            }
            var Top = GetChild(this, "ResizeTop");
            if (Top != null)
            {
                Top.MouseMove += ResizePressed;
                Top.MouseDown += ResizePressed;
            }
            var TopRight = GetChild(this, "ResizeTopRight");
            if (TopRight != null)
            {
                TopRight.MouseMove += ResizePressed;
                TopRight.MouseDown += ResizePressed;
            }

            var Left = GetChild(this, "ResizeLeft");
            if (Left != null)
            {
                Left.MouseMove += ResizePressed;
                Left.MouseDown += ResizePressed;
            }

            var Right = GetChild(this, "ResizeRight");
            if (Right != null)
            {
                Right.MouseMove += ResizePressed;
                Right.MouseDown += ResizePressed;
            }

            var BottomLeft = GetChild(this, "ResizeBottomLeft");
            if (BottomLeft != null)
            {
                BottomLeft.MouseMove += ResizePressed;
                BottomLeft.MouseDown += ResizePressed;
            }

            var Bottom = GetChild(this, "ResizeBottom");
            if (Bottom != null)
            {
                Bottom.MouseMove += ResizePressed;
                Bottom.MouseDown += ResizePressed;
            }
            var BottomRight = GetChild(this, "ResizeBottomRight");
            if (BottomRight != null)
            {
                BottomRight.MouseMove += ResizePressed;
                BottomRight.MouseDown += ResizePressed;
            }
        }

        public void ResizePressed(object sender, MouseEventArgs e)
        {
            FrameworkElement element = sender as FrameworkElement;
            ResizeDirection direction = (ResizeDirection)Enum.Parse(typeof(ResizeDirection), element.Tag.ToString().Replace("Resize", ""));
            this.Cursor = cursors[direction];
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                ResizeWindow(direction);
            }
        }

        public void ResizeWindow(ResizeDirection direction)
        {
            SendMessage(HwndSource.Handle, WM_SYSCOMMAND, (IntPtr)(61440 + direction), IntPtr.Zero);
        }


        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);
    }

}
