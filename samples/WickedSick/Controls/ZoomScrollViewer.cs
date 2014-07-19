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
using System.ComponentModel;
using System.Windows.Markup;
using System.Windows.Controls.Primitives;
using Controls;

namespace WickedSick.DiagramDesigner
{
    public class ZoomScrollViewer : ScrollViewer
    {
        public static readonly DependencyProperty MaxZoomProperty;
        public static readonly DependencyProperty MinZoomProperty;
        public static readonly DependencyProperty ZoomProperty;
        public static readonly DependencyProperty FillProperty;

        public static readonly RoutedEvent MaxZoomChangedEvent;
        public static readonly RoutedEvent MinZoomChangedEvent;
        public static readonly RoutedEvent ZoomChangedEvent;

        static ZoomScrollViewer()
        {
            MaxZoomProperty = DependencyProperty.Register("MaxZoom", typeof(double), typeof(ZoomScrollViewer),
                new FrameworkPropertyMetadata((double)2, new PropertyChangedCallback(OnMaxZoomChanged), new CoerceValueCallback(CoerceMaxZoom)));
            MinZoomProperty = DependencyProperty.Register("MinZoom", typeof(double), typeof(ZoomScrollViewer),
                new FrameworkPropertyMetadata((double).5, new PropertyChangedCallback(OnMinZoomChanged), new CoerceValueCallback(CoerceMinZoom)));
            ZoomProperty = DependencyProperty.Register("Zoom", typeof(double), typeof(ZoomScrollViewer),
                new FrameworkPropertyMetadata((double)1, new PropertyChangedCallback(OnZoomChanged), new CoerceValueCallback(ConstrainZoom)));
            FillProperty = DependencyProperty.RegisterAttached("Fill", typeof(bool), typeof(ZoomScrollViewer));

            MaxZoomChangedEvent = EventManager.RegisterRoutedEvent("MaxZoomChanged", RoutingStrategy.Bubble,
                typeof(RoutedPropertyChangedEventHandler<double>), typeof(ZoomScrollViewer));
            MinZoomChangedEvent = EventManager.RegisterRoutedEvent("MinZoomChanged", RoutingStrategy.Bubble,
                typeof(RoutedPropertyChangedEventHandler<double>), typeof(ZoomScrollViewer));
            ZoomChangedEvent = EventManager.RegisterRoutedEvent("ZoomChanged", RoutingStrategy.Bubble,
                typeof(RoutedPropertyChangedEventHandler<double>), typeof(ZoomScrollViewer));

            DefaultStyleKeyProperty.OverrideMetadata(typeof(ZoomScrollViewer), new FrameworkPropertyMetadata(typeof(ZoomScrollViewer)));
        }

        public static bool GetFill(FrameworkElement element)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }
            return (bool)element.GetValue(ZoomScrollViewer.FillProperty);
        }

        public static void SetFill(FrameworkElement element, bool value)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }
            element.SetValue(ZoomScrollViewer.FillProperty, value);
        }

        private double? _startHorizontalOffset = null;
        private double? _startVerticalOffset = null;
        private Point? _startScroll = null;

        public ZoomScrollViewer()
            : base()
        {
            this.ScrollChanged += new ScrollChangedEventHandler(ZoomScrollViewer_ScrollChanged);
            this.MouseMove += new MouseEventHandler(ZoomScrollViewer_MouseMove);
            this.MouseDown += new MouseButtonEventHandler(ZoomScrollViewer_MouseDown);
            this.MouseUp += new MouseButtonEventHandler(ZoomScrollViewer_MouseUp);
        }

        void ZoomScrollViewer_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Right)
            {
                _startScroll = null;
                Cursor = Cursors.Arrow;
                Mouse.Capture(null);
            }
        }

        void ZoomScrollViewer_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Right)
            {
                _startHorizontalOffset = this.HorizontalOffset;
                _startVerticalOffset = this.VerticalOffset;
                _startScroll = e.GetPosition(this);
                Cursor = Cursors.ScrollAll;
                this.CaptureMouse();
            }
        }

        void ZoomScrollViewer_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.RightButton == MouseButtonState.Pressed)
            {
                if (_startScroll.HasValue)
                {
                    Point newPoint = e.GetPosition(this);
                    double? newHorizontalOffset = _startHorizontalOffset - (newPoint.X - _startScroll.Value.X);
                    double? newVerticalOffset = _startVerticalOffset - (newPoint.Y - _startScroll.Value.Y);
                    ScrollToHorizontalOffset(newHorizontalOffset.Value);
                    ScrollToVerticalOffset(newVerticalOffset.Value);
                }
            }
        }

        private void ZoomScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (Content != null)
            {
                FrameworkElement content = (FrameworkElement)Content;
                if (GetFill(content))
                {
                    if (e.ViewportWidth > content.Width)
                        content.Width = e.ViewportWidth;
                    if (e.ViewportHeight > content.Height)
                        content.Height = e.ViewportHeight;
                }
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            ScaleTransform scale = GetTemplateChild("PART_Transform") as ScaleTransform;
            if (scale != null)
            {
                Binding binding = new Binding("Zoom");
                binding.Source = this;
                binding.Mode = BindingMode.OneWay;
                BindingOperations.SetBinding(scale, ScaleTransform.ScaleXProperty, binding);
                BindingOperations.SetBinding(scale, ScaleTransform.ScaleYProperty, binding);
            }

            RangeBase slider = GetTemplateChild("PART_ZoomSlider") as RangeBase;
            if (slider != null)
            {
                Binding maxBinding = new Binding("MaxZoom");
                maxBinding.Source = this;
                maxBinding.Mode = BindingMode.OneWay;
                slider.SetBinding(RangeBase.MaximumProperty, maxBinding);

                Binding minBinding = new Binding("MinZoom");
                minBinding.Source = this;
                minBinding.Mode = BindingMode.OneWay;
                slider.SetBinding(RangeBase.MinimumProperty, minBinding);

                Binding binding = new Binding("Zoom");
                binding.Source = this;
                binding.Mode = BindingMode.TwoWay;
                BindingOperations.SetBinding(slider, RangeBase.ValueProperty, binding);
            }
        }

        public double MinZoom
        {
            get { return (double)GetValue(MinZoomProperty); }
            set { SetValue(MinZoomProperty, value); }
        }

        public double MaxZoom
        {
            get { return (double)GetValue(MaxZoomProperty); }
            set { SetValue(MaxZoomProperty, value); }
        }

        public double Zoom
        {
            get { return (double)GetValue(ZoomProperty); }
            set { SetValue(ZoomProperty, value); }
        }

        public event RoutedPropertyChangedEventHandler<double> MaxZoomChanged
        {
            add { AddHandler(MaxZoomChangedEvent, value); }
            remove { RemoveHandler(MaxZoomChangedEvent, value); }
        }

        public event RoutedPropertyChangedEventHandler<double> MinZoomChanged
        {
            add { AddHandler(MinZoomChangedEvent, value); }
            remove { RemoveHandler(MinZoomChangedEvent, value); }
        }

        public event RoutedPropertyChangedEventHandler<double> ZoomChanged
        {
            add { AddHandler(ZoomChangedEvent, value); }
            remove { RemoveHandler(ZoomChangedEvent, value); }
        }

        private static void OnMaxZoomChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ZoomScrollViewer canvas = (ZoomScrollViewer)sender;

            double oldMaxZoom = (double)e.OldValue;
            double newMaxZoom = (double)e.NewValue;
            RoutedPropertyChangedEventArgs<double> args = new RoutedPropertyChangedEventArgs<double>(oldMaxZoom, newMaxZoom);
            args.RoutedEvent = ZoomScrollViewer.MaxZoomChangedEvent;

            canvas.RaiseEvent(args);
        }

        private static void OnMinZoomChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ZoomScrollViewer canvas = (ZoomScrollViewer)sender;

            double oldMinZoom = (double)e.OldValue;
            double newMinZoom = (double)e.NewValue;
            RoutedPropertyChangedEventArgs<double> args = new RoutedPropertyChangedEventArgs<double>(oldMinZoom, newMinZoom);
            args.RoutedEvent = ZoomScrollViewer.MinZoomChangedEvent;

            canvas.RaiseEvent(args);
        }

        private static void OnZoomChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ZoomScrollViewer canvas = (ZoomScrollViewer)sender;
            double oldZoom = (double)e.OldValue;
            double newZoom = (double)e.NewValue;

            RoutedPropertyChangedEventArgs<double> args = new RoutedPropertyChangedEventArgs<double>(oldZoom, newZoom);
            args.RoutedEvent = ZoomScrollViewer.ZoomChangedEvent;

            canvas.RaiseEvent(args);
        }

        private static object CoerceMaxZoom(DependencyObject sender, object value)
        {
            ZoomScrollViewer canvas = (ZoomScrollViewer)sender;
            if ((double)value < canvas.MinZoom)
            {
                return canvas.MinZoom;
            }
            else
            {
                return value;
            }
        }

        private static object CoerceMinZoom(DependencyObject sender, object value)
        {
            ZoomScrollViewer canvas = (ZoomScrollViewer)sender;
            if ((double)value > canvas.MaxZoom)
            {
                return canvas.MaxZoom;
            }
            else
            {
                return value;
            }
        }

        private static object ConstrainZoom(DependencyObject sender, object value)
        {
            ZoomScrollViewer canvas = (ZoomScrollViewer)sender;
            if ((double)value < canvas.MinZoom)
            {
                return canvas.MinZoom;
            }

            if ((double)value > canvas.MaxZoom)
            {
                return canvas.MaxZoom;
            }

            return value;
        }
    }
}
