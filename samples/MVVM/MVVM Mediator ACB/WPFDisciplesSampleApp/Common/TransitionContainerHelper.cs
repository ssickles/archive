using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using FluidKit.Controls;

namespace WPFDisciples.Common
{
    public class TransitionPresenterHelper
    {
        #region CurrentView

        /// <summary>
        /// CurrentView Attached Dependency Property
        /// </summary>
        public static readonly DependencyProperty CurrentViewProperty =
            DependencyProperty.RegisterAttached("CurrentView", typeof(string), typeof(TransitionPresenterHelper),
                new FrameworkPropertyMetadata((string)"",
                    new PropertyChangedCallback(OnCurrentViewChanged)));

        /// <summary>
        /// Gets the CurrentView property.  This dependency property 
        /// indicates ....
        /// </summary>
        public static string GetCurrentView(DependencyObject d)
        {
            return (string)d.GetValue(CurrentViewProperty);
        }

        /// <summary>
        /// Sets the CurrentView property.  This dependency property 
        /// indicates ....
        /// </summary>
        public static void SetCurrentView(DependencyObject d, string value)
        {
            d.SetValue(CurrentViewProperty, value);
        }

        /// <summary>
        /// Handles changes to the CurrentView property.
        /// </summary>
        private static void OnCurrentViewChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue != null && e.NewValue != null)
            {
                TransitionPresenter container = (TransitionPresenter)d;
                container.ApplyTransition((string)e.OldValue, (string)e.NewValue);
            }
        }

        #endregion
    }
}
