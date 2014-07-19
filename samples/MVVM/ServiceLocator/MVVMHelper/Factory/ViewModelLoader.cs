using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace MVVMHelper.Factory
{
    public class ViewModelLoader
    {
        #region FactoryType

        /// <summary>
        /// FactoryType Attached Dependency Property
        /// </summary>
        public static readonly DependencyProperty FactoryTypeProperty =
            DependencyProperty.RegisterAttached("FactoryType", typeof(Type), typeof(ViewModelLoader),
                new FrameworkPropertyMetadata((Type)null,
                    new PropertyChangedCallback(OnFactoryTypeChanged)));

        /// <summary>
        /// Gets the FactoryType property.  This dependency property 
        /// indicates ....
        /// </summary>
        public static Type GetFactoryType(DependencyObject d)
        {
            return (Type)d.GetValue(FactoryTypeProperty);
        }

        /// <summary>
        /// Sets the FactoryType property.  This dependency property 
        /// indicates ....
        /// </summary>
        public static void SetFactoryType(DependencyObject d, Type value)
        {
            d.SetValue(FactoryTypeProperty, value);
        }

        /// <summary>
        /// Handles changes to the FactoryType property.
        /// </summary>
        private static void OnFactoryTypeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            FrameworkElement element = (FrameworkElement)d;
            IFactory factory = Activator.CreateInstance( GetFactoryType(d) ) as IFactory;
            if (factory == null)
                throw new InvalidOperationException("You have to specify a type that inherits from IFactory");
            element.DataContext = factory.CreateViewModel(d);
        }

        #endregion

    }
}
