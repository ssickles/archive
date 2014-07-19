using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MediatorSample.Behaviours
{
    public class TreeViewBehaviours
    {
        #region SelectedItem

        /// <summary>
        /// SelectedItem Attached Dependency Property
        /// </summary>
        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.RegisterAttached("SelectedItem", typeof(object), typeof(TreeViewBehaviours),
                new FrameworkPropertyMetadata((object)null));

        /// <summary>
        /// Gets the SelectedItem property.  This dependency property 
        /// indicates ....
        /// </summary>
        public static object GetSelectedItem(DependencyObject d)
        {
            return (object)d.GetValue(SelectedItemProperty);
        }

        /// <summary>
        /// Sets the SelectedItem property.  This dependency property 
        /// indicates ....
        /// </summary>
        public static void SetSelectedItem(DependencyObject d, object value)
        {
            d.SetValue(SelectedItemProperty, value);
        }

        #endregion

        #region HandleSelectionChanges

        /// <summary>
        /// HandleSelectionChanges Attached Dependency Property
        /// </summary>
        public static readonly DependencyProperty HandleSelectionChangesProperty =
            DependencyProperty.RegisterAttached("HandleSelectionChanges", typeof(bool), typeof(TreeViewBehaviours),
                new FrameworkPropertyMetadata((bool)false,
                    new PropertyChangedCallback(OnHandleSelectionChangesChanged)));

        /// <summary>
        /// Gets the HandleSelectionChanges property.  This dependency property 
        /// indicates ....
        /// </summary>
        public static bool GetHandleSelectionChanges(DependencyObject d)
        {
            return (bool)d.GetValue(HandleSelectionChangesProperty);
        }

        /// <summary>
        /// Sets the HandleSelectionChanges property.  This dependency property 
        /// indicates ....
        /// </summary>
        public static void SetHandleSelectionChanges(DependencyObject d, bool value)
        {
            d.SetValue(HandleSelectionChangesProperty, value);
        }

        /// <summary>
        /// Handles changes to the HandleSelectionChanges property.
        /// </summary>
        private static void OnHandleSelectionChangesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var source = (TreeView)d;
            if ((bool)e.NewValue)
                source.SelectedItemChanged += SelectedItemChanged;
            else
                source.SelectedItemChanged -= SelectedItemChanged;
        }

        static void SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            SetSelectedItem((DependencyObject)sender, e.NewValue);
        }

        #endregion

        #region ExpandedCommand

        /// <summary>
        /// ExpandedCommand Attached Dependency Property
        /// </summary>
        public static readonly DependencyProperty ExpandedCommandProperty =
            DependencyProperty.RegisterAttached("ExpandedCommand", typeof(ICommand), typeof(TreeViewBehaviours),
                new FrameworkPropertyMetadata((ICommand)null,
                    new PropertyChangedCallback(OnExpandedCommandChanged)));

        /// <summary>
        /// Gets the ExpandedCommand property.  This dependency property 
        /// indicates ....
        /// </summary>
        public static ICommand GetExpandedCommand(DependencyObject d)
        {
            return (ICommand)d.GetValue(ExpandedCommandProperty);
        }

        /// <summary>
        /// Sets the ExpandedCommand property.  This dependency property 
        /// indicates ....
        /// </summary>
        public static void SetExpandedCommand(DependencyObject d, ICommand value)
        {
            d.SetValue(ExpandedCommandProperty, value);
        }

        /// <summary>
        /// Handles changes to the ExpandedCommand property.
        /// </summary>
        private static void OnExpandedCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var source = (TreeView)d;
            source.RemoveHandler(TreeViewItem.ExpandedEvent, (RoutedEventHandler)ItemExpanded);//unregister in case this is not the first time the property is set
            source.AddHandler(TreeViewItem.ExpandedEvent, (RoutedEventHandler)ItemExpanded);
        }

        static void ItemExpanded(object sender, RoutedEventArgs e)
        {
            var source = (TreeView)sender;
            var command = GetExpandedCommand(source);
            var item = (TreeViewItem)e.OriginalSource;
            if (command != null && command.CanExecute(item.Header))
                command.Execute(item.Header);
        }

        #endregion


    }
}
