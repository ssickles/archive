using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;

namespace System.Windows.Controls
{
    [TemplateVisualState(Name = "Active", GroupName = "Activity")]
    [TemplateVisualState(Name = "Inactive", GroupName = "Activity")]
    [TemplateVisualState(Name = "Displaying", GroupName = "ActivityDisplay")]
    [TemplateVisualState(Name = "Hidden", GroupName = "ActivityDisplay")]
    [StyleTypedProperty(Property = "OverlayStyle", StyleTargetType = typeof(Rectangle))]
    [StyleTypedProperty(Property = "ProgressBarStyle", StyleTargetType = typeof(ProgressBar))]
    public class Activity : ContentControl, IDisposable
    {
        private Timer activeTimer;
        private Timer inactiveTimer;
        private DateTime displayStart;
        private List<DependencyProperty> props;
        private static Stack<DependencyProperty> cachedProps;
        private static int propNumber;

        public Activity()
        {
            this.DefaultStyleKey = typeof(Activity);
            activeTimer = new Timer(ActiveDelayCallback);
            inactiveTimer = new Timer(InactiveDelayCallback);
            VisualStateManager.GoToState(this, "Inactive", true);
            VisualStateManager.GoToState(this, "Hidden", true);
            cachedProps = new Stack<DependencyProperty>();
            props = new List<DependencyProperty>();
            this.LayoutUpdated += new EventHandler(Activity_LayoutUpdated);
        }

        private void Activity_LayoutUpdated(object sender, EventArgs e)
        {
            Register();
        }

        public bool IsActivityVisible
        {
            get { return (bool)GetValue(IsActivityVisibleProperty); }
            private set { SetValue(IsActivityVisibleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsActivityVisible.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsActivityVisibleProperty =
            DependencyProperty.Register("IsActivityVisible", typeof(bool), typeof(Activity), new PropertyMetadata(false));

        public bool IsActive
        {
            get { return (bool)GetValue(IsActiveProperty); }
            set { SetValue(IsActiveProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsActive.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsActiveProperty =
            DependencyProperty.Register("IsActive", typeof(bool), typeof(Activity), new PropertyMetadata(false, new PropertyChangedCallback(IsActiveChanged)));

        private void IsActiveChanged()
        {
            if (this.IsActive)
            {
                VisualStateManager.GoToState(this, "Active", true);
                if (!DisplayAfter.Equals(TimeSpan.Zero))
                {
                    activeTimer.Change(this.DisplayAfter, TimeSpan.FromMilliseconds(-1));
                }
                else
                {
                    ActiveDelayCallback(null);
                }
            }
            else
            {
                VisualStateManager.GoToState(this, "Inactive", true);
                if (IsActivityVisible)
                {
                    TimeSpan duration = DateTime.Now - displayStart;
                    if (MinDisplayTime > duration)
                    {
                        inactiveTimer.Change(this.MinDisplayTime - duration, TimeSpan.FromMilliseconds(-1));
                    }
                    else
                    {
                        InactiveDelayCallback(null);
                    }
                }
            }
        }

        public void ResetActivity()
        {
            if (this.IsActive)
            {
                IsActivityVisible = true;
                VisualStateManager.GoToState(this, "Active", true);
                VisualStateManager.GoToState(this, "Displaying", true);
            }
            else
            {
                VisualStateManager.GoToState(this, "Inactive", true);
                VisualStateManager.GoToState(this, "Hidden", true);
                IsActivityVisible = false;
            }
        }

        private void ActiveDelayCallback(object state)
        {
            this.Dispatcher.BeginInvoke(
                () =>
                {
                    if (this.IsActive)
                    {
                        if (!IsActivityVisible)
                            displayStart = DateTime.Now;
                        IsActivityVisible = true;
                        VisualStateManager.GoToState(this, "Displaying", true);
                    }
                });
        }

        private void InactiveDelayCallback(object state)
        {
            this.Dispatcher.BeginInvoke(
                () =>
                {
                    if (!this.IsActive)
                    {
                        VisualStateManager.GoToState(this, "Hidden", true);
                        IsActivityVisible = false;
                    }
                });
        }

        private static void IsActiveChanged(DependencyObject dep, DependencyPropertyChangedEventArgs e)
        {
            (dep as Activity).IsActiveChanged();
        }


        public string ActivityPropertyName
        {
            get { return (string)GetValue(ActivityPropertyNameProperty); }
            set { SetValue(ActivityPropertyNameProperty, value); }
        }

        private void ActivePropertyNameChanged()
        {
            Register();
        }

        // Using a DependencyProperty as the backing store for ActivityPropertyName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ActivityPropertyNameProperty =
            DependencyProperty.Register("ActivityPropertyName", typeof(string), typeof(Activity), new PropertyMetadata("IsBusy", new PropertyChangedCallback(ActivityPropertyNameChanged)));

        private static void ActivityPropertyNameChanged(DependencyObject dep, DependencyPropertyChangedEventArgs e)
        {
            (dep as Activity).ActivePropertyNameChanged();
        }

        public TimeSpan DisplayAfter
        {
            get { return (TimeSpan)GetValue(DisplayAfterProperty); }
            set { SetValue(DisplayAfterProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DisplayAfter.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DisplayAfterProperty =
            DependencyProperty.Register("DisplayAfter", typeof(TimeSpan), typeof(Activity), new PropertyMetadata(TimeSpan.FromSeconds(.25)));

        public TimeSpan MinDisplayTime
        {
            get { return (TimeSpan)GetValue(MinDisplayTimeProperty); }
            set { SetValue(MinDisplayTimeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DisplayAfter.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MinDisplayTimeProperty =
            DependencyProperty.Register("MinDisplayTime", typeof(TimeSpan), typeof(Activity), new PropertyMetadata(TimeSpan.FromSeconds(.25)));

        public Style OverlayStyle
        {
            get { return (Style)GetValue(OverlayStyleProperty); }
            set { SetValue(OverlayStyleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for OverlayStyle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OverlayStyleProperty =
            DependencyProperty.Register("OverlayStyle", typeof(Style), typeof(Activity), new PropertyMetadata(null));


        public Style ProgressBarStyle
        {
            get { return (Style)GetValue(ProgressBarStyleProperty); }
            set { SetValue(ProgressBarStyleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ProgressBarStyle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ProgressBarStyleProperty =
            DependencyProperty.Register("ProgressBarStyle", typeof(Style), typeof(Activity), new PropertyMetadata(null));

        public object ActiveContent
        {
            get { return (object)GetValue(ActiveContentProperty); }
            set { SetValue(ActiveContentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ActiveContent.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ActiveContentProperty =
            DependencyProperty.Register("ActiveContent", typeof(object), typeof(Activity), new PropertyMetadata(null));



        public DataTemplate ActiveContentTemplate
        {
            get { return (DataTemplate)GetValue(ActiveContentTemplateProperty); }
            set { SetValue(ActiveContentTemplateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ActiveContentTemplate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ActiveContentTemplateProperty =
            DependencyProperty.Register("ActiveContentTemplate", typeof(DataTemplate), typeof(Activity), new PropertyMetadata(null));



        public bool AutoBind
        {
            get { return (bool)GetValue(AutoBindProperty); }
            set { SetValue(AutoBindProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AutoBind.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AutoBindProperty =
            DependencyProperty.Register("AutoBind", typeof(bool), typeof(Activity), new PropertyMetadata(false, new PropertyChangedCallback(AutoBindChanged)));

        private static void AutoBindChanged(DependencyObject dep, DependencyPropertyChangedEventArgs e)
        {
            (dep as Activity).AutoBindChanged();
        }

        private void AutoBindChanged()
        {
            if (AutoBind)
            {
                Register();
            }
            else
            {
                Unregister();
            }
        }

        private void Register()
        {
            if (AutoBind)
            {
                Unregister();
                Register(this);
                RefreshValues();
            }
        }

        private DependencyProperty GetDependencyProperty()
        {
            if (cachedProps.Count > 0)
                return cachedProps.Pop();
            return DependencyProperty.Register("Activity" + propNumber++, typeof(bool), GetType(), new PropertyMetadata(new PropertyChangedCallback(ChildPropertyChanged)));
        }

        private void FreeDependencyProperty(DependencyProperty dp)
        {
            ClearValue(dp);
            cachedProps.Push(dp);
        }

        private void Register(DependencyObject dep)
        {
            if (dep == null)
                return;
            int childrenCount = VisualTreeHelper.GetChildrenCount(dep);
            for (int x = 0; x < childrenCount; x++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(dep, x);
                PropertyInfo pi = child.GetType().GetProperty(ActivityPropertyName, typeof(bool)) ?? child.GetType().GetProperty(ActivityPropertyName, typeof(bool?));
                if (pi != null && pi.CanRead)
                {
                    DependencyProperty dp = GetDependencyProperty();
                    Binding b = new Binding(this.ActivityPropertyName);
                    b.Source = child;
                    SetBinding(dp, b);
                    props.Add(dp);
                }
                Register(child);
            }
        }

        private void RefreshValues()
        {
            if (props.Count == 0)
                return;
            bool result = false;
            int activeCount = 0;
            for (int x = 0; x < props.Count; x++)
            {
                DependencyProperty dp = props[x];
                bool b = (bool)GetValue(dp);
                if (b)
                {
                    result = true;
                    activeCount++;
                }
            }
            this.IsActive = result;
        }

        private void ChildPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            RefreshValues();
        }

        private void Unregister()
        {
            List<DependencyProperty> copy = new List<DependencyProperty>(props);
            props.Clear();
            foreach (DependencyProperty dp in copy)
                FreeDependencyProperty(dp);
        }

        protected virtual void Dispose(bool disposeManaged)
        {
            if (disposeManaged)
            {
                if (activeTimer != null)
                    activeTimer.Dispose();
                if (inactiveTimer != null)
                    inactiveTimer.Dispose();
            }
        }

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
