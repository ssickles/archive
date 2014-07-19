using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;

namespace ControlTemplates
{
    public partial class CustomWindowChrome : ResourceDictionary
    {
        public CustomWindowChrome()
        {
            InitializeComponent();
        }

        private bool isResizing = false;
        [Flags()]
        private enum ResizeType
        {
            Width, Height
        }
        private ResizeType resizeType;

        
        private void window_initiateResizeWE(object sender, System.Windows.Input.MouseEventArgs e)
        {
            isResizing = true;
            resizeType = ResizeType.Width;
        }
        private void window_initiateResizeNS(object sender, System.Windows.Input.MouseEventArgs e)
        {
            isResizing = true;
            resizeType = ResizeType.Height;
        }

        private void window_endResize(object sender, System.Windows.Input.MouseEventArgs e)
        {
            isResizing = false;

            // Make sure capture is released.
            Rectangle rect = (Rectangle)sender;
            rect.ReleaseMouseCapture();
        }

        private void window_Resize(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Rectangle rect = (Rectangle)sender;
            Window win = (Window)rect.TemplatedParent;

            if (isResizing)
            {                
                rect.CaptureMouse();
                if (resizeType == ResizeType.Width) win.Width = e.GetPosition(win).X + 5;
                if (resizeType == ResizeType.Height) win.Height = e.GetPosition(win).Y + 5;                 
            }            
        }

        private void titleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Window win = (Window)
                ((FrameworkElement)sender).TemplatedParent;
            win.DragMove();
        }

        private void cmdClose_Click(object sender, RoutedEventArgs e)
        {
            Window win = (Window)
                ((FrameworkElement)sender).TemplatedParent;
            win.Close();
        }
    }
}
