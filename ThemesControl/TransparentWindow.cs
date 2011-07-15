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
using System.Windows.Controls.Primitives;

namespace ThemesControl
{
    public class TransparentWindow : Window
    {
        static TransparentWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TransparentWindow), new FrameworkPropertyMetadata(typeof(TransparentWindow)));
        }

        private ResizeType _resizeType;
        private Border _titleBar;
        private Button _minimize;
        private ToggleButton _restore;
        private Button _close;
        private Rectangle _windowResizeWE;
        private Rectangle _windowResizeNS;
        private ResizeGrip _resize;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _titleBar = (Border)GetTemplateChild("PART_TitleBar");
            _minimize = (Button)GetTemplateChild("PART_Minimize");
            _restore = (ToggleButton)GetTemplateChild("PART_Restore");
            _close = (Button)GetTemplateChild("PART_Close");
            _windowResizeWE = (Rectangle)GetTemplateChild("PART_WindowResizeWE");
            _windowResizeNS = (Rectangle)GetTemplateChild("PART_WindowResizeNS");
            _resize = (ResizeGrip)GetTemplateChild("PART_WindowResizeGrip");

            _titleBar.MouseLeftButtonDown += new MouseButtonEventHandler(_titleBar_MouseLeftButtonDown);
            _minimize.Click += new RoutedEventHandler(_minimize_Click);
            _restore.Click += new RoutedEventHandler(_restore_Click);
            _close.Click += new RoutedEventHandler(_close_Click);
            _windowResizeWE.MouseLeftButtonDown += new MouseButtonEventHandler(_windowResizeWE_MouseLeftButtonDown);
            _windowResizeWE.MouseLeftButtonUp += new MouseButtonEventHandler(window_endResize);
            _windowResizeWE.MouseMove += new MouseEventHandler(window_Resize);
            _windowResizeNS.MouseLeftButtonDown += new MouseButtonEventHandler(_windowResizeNS_MouseLeftButtonDown);
            _windowResizeNS.MouseLeftButtonUp += new MouseButtonEventHandler(window_endResize);
            _windowResizeNS.MouseMove += new MouseEventHandler(window_Resize);
        }

        private void _windowResizeNS_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _resizeType = ResizeType.Height;
        }

        private void _windowResizeWE_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _resizeType = ResizeType.Width;
        }

        private void _close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void _restore_Click(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Normal)
            {
                WindowState = WindowState.Maximized;
            }
            else
            {
                WindowState = WindowState.Normal;
            }
        }

        private void _minimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void _titleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
                if (WindowState == WindowState.Maximized)
                    WindowState = WindowState.Normal;
                else
                    WindowState = WindowState.Maximized;
            else
                DragMove();
        }

        private void window_endResize(object sender, MouseButtonEventArgs e)
        {
            FrameworkElement element = (FrameworkElement)sender;
            element.ReleaseMouseCapture();
        }

        private void window_Resize(object sender, MouseEventArgs e)
        {
            FrameworkElement element = (FrameworkElement)sender;

            if (e.LeftButton == MouseButtonState.Pressed)
            {
                element.CaptureMouse();
                if (_resizeType == ResizeType.Width)
                {
                    double newWidth = e.GetPosition(this).X + 5;
                    if (newWidth >= 0)
                    {
                        Width = newWidth;
                    }
                }
                if (_resizeType == ResizeType.Height)
                {
                    double newHeight = e.GetPosition(this).Y + 5;
                    if (newHeight >= 0)
                    {
                        Height = newHeight;
                    }
                }
            }
        }
    }

    [Flags()]
    enum ResizeType
    {
        Width, Height
    }
}
