using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Diagnostics.Eventing;
using System.Diagnostics;
using System.Windows.Documents;
using Controls;

namespace WickedSick.DiagramDesigner
{
    public class DragCanvas: Canvas
    {
        private UIElement _dragElement;
        private Point _originalOffset;

        public DragCanvas()
            : base()
        {
            this.Background = Brushes.White;
            this.MouseLeftButtonUp += new MouseButtonEventHandler(DragCanvas_MouseLeftButtonUp);
            this.MouseMove += new MouseEventHandler(DragCanvas_MouseMove);
        }

        public void ShrinkToContents()
        {
            double minWidth = 0;
            double minHeight = 0;
            foreach (UIElement element in Children)
            {
                if (!double.IsNaN(GetTop(element)))
                {
                    minWidth = Math.Max(minWidth, GetLeft(element) + element.DesiredSize.Width);
                }
                if (!double.IsNaN(GetLeft(element)))
                {
                    minHeight = Math.Max(minHeight, GetTop(element) + element.DesiredSize.Height);
                }
            }
            MinHeight = minHeight;
            MinWidth = minWidth;
        }

        private void DragCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && _dragElement != null)
            {
                Point currentPoint = e.GetPosition(this);
                currentPoint.Offset(-_originalOffset.X, -_originalOffset.Y);
                if (currentPoint.X < 0) currentPoint.X = 0;
                if (currentPoint.Y < 0) currentPoint.Y = 0;
                SetLeft(_dragElement, currentPoint.X);
                SetTop(_dragElement, currentPoint.Y);
                ResetMinWidthHeight(_dragElement);
                if (_dragElement is FrameworkElement)
                {
                    ((FrameworkElement)_dragElement).BringIntoView();
                }
            }
        }

        private void DragCanvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            System.Windows.Input.Mouse.Capture(null);
            _dragElement = null;
        }

        private void element_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _dragElement = (UIElement)sender;
            _originalOffset = e.GetPosition(_dragElement);
            this.CaptureMouse();
        }

        private void ResetMinWidthHeight(UIElement element)
        {
            if (!double.IsNaN(GetTop(element)))
            {
                MinHeight = Math.Max(MinHeight, GetTop(element) + element.RenderSize.Height);
            }
            if (!double.IsNaN(GetLeft(element)))
            {
                MinWidth = Math.Max(MinWidth, GetLeft(element) + element.RenderSize.Width);
            }
        }

        protected override void OnVisualChildrenChanged(System.Windows.DependencyObject visualAdded, System.Windows.DependencyObject visualRemoved)
        {
            base.OnVisualChildrenChanged(visualAdded, visualRemoved);

            if (visualAdded != null)
            {
                UIElement element = (UIElement)visualAdded;
                element.MouseLeftButtonDown += new MouseButtonEventHandler(element_MouseLeftButtonDown);
                element.LayoutUpdated += new EventHandler(element_LayoutUpdated);
            }
        }

        void element_LayoutUpdated(object sender, EventArgs e)
        {
            foreach (UIElement element in Children)
            {
                if (!double.IsNaN(GetTop(element)))
                {
                    MinHeight = Math.Max(MinHeight, GetTop(element) + element.RenderSize.Height);
                }
                if (!double.IsNaN(GetLeft(element)))
                {
                    MinWidth = Math.Max(MinWidth, GetLeft(element) + element.RenderSize.Width);
                }
            }
        }
    }
}
