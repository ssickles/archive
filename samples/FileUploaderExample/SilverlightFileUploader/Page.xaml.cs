/*
Copyright 2003-2009 Virtual Chemistry, Inc. (VCI)
http://www.virtualchemistry.com
Using .Net, Silverlight, SharePoint and more to solve your tough problems in web-based data management.

Author: Peter Coley
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace SilverlightFileUploader
{
    public partial class Page : UserControl
    {
        public Page()
        {
            InitializeComponent();
        }

        public Page(IDictionary<string, string> InitParams)
        {
            InitializeComponent();

            fileUploader.InitializeFromInitParams(InitParams);
        }

        private void LayoutRoot_LostMouseCapture(object sender, MouseEventArgs e)
        {
            FixScrollView();
        }

        private void LayoutRoot_MouseLeave(object sender, MouseEventArgs e)
        {
            FixScrollView();
        }

        private void FixScrollView()
        {
            // the file uploader is used in windowless mode (to avoid overlap issues, and make it display correctly in firefox
            // when initially hiding it in a display:none div), so need to do some code to fix mouse capture issues

            FrameworkElement fe = VisualTreeHelper.GetChild(fileUploader.ScrollViewer, 0) as FrameworkElement;
            if (fe != null)
            {
                //find the vertical scrollbar
                ScrollBar sb = fe.FindName("VerticalScrollBar") as ScrollBar; if (sb != null)
                {
                    //Get the scrollbar's thumbnail. This is the sucker that remains stuck in the "drag" state
                    Thumb thumb = ((FrameworkElement)VisualTreeHelper.GetChild(sb, 0)).FindName("VerticalThumb") as Thumb;

                    //cancel the operation
                    thumb.CancelDrag();
                }
            }
        }
    }
}
