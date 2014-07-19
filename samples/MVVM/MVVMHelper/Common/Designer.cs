using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.ComponentModel;

namespace MVVMHelper.Common
{
    /// <summary>
    /// Can be used to resolve whether the application is currently in design
    /// mode.
    /// </summary>
    public static class Designer
    {
        #region IsDesignMode

        private static readonly bool isDesignMode;

        /// <summary>
        /// Checks whether the application is currently in design mode.
        /// </summary>
        public static bool IsDesignMode
        {
            get { return isDesignMode; }
        }

        #endregion

        static Designer()
        {
            DependencyProperty prop = DesignerProperties.IsInDesignModeProperty;
            isDesignMode = (bool)DependencyPropertyDescriptor.FromProperty(prop, typeof(FrameworkElement))
                                                             .Metadata.DefaultValue;

        }
    }
}
