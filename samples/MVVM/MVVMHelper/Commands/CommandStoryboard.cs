using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Animation;
using System.Windows;
using System.Windows.Input;

namespace MVVMHelper.Commands
{
    /// <summary>
    /// Just like a regular storyboard, except it calls a command on its completed event
    /// </summary>
    public class CommandStoryboard
    {
        #region CommandParameter

        /// <summary>
        /// CommandParameter Attached Dependency Property
        /// </summary>
        public static readonly DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached("CommandParameter", typeof(object), typeof(CommandStoryboard),
                new FrameworkPropertyMetadata((object)null));

        /// <summary>
        /// Gets the CommandParameter property.  This dependency property 
        /// indicates ....
        /// </summary>
        public static object GetCommandParameter(DependencyObject d)
        {
            return (object)d.GetValue(CommandParameterProperty);
        }

        /// <summary>
        /// Sets the CommandParameter property.  This dependency property 
        /// indicates ....
        /// </summary>
        public static void SetCommandParameter(DependencyObject d, object value)
        {
            d.SetValue(CommandParameterProperty, value);
        }

        #endregion

        #region Command

        /// <summary>
        /// Command Attached Dependency Property
        /// </summary>
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command", typeof(ICommand), typeof(CommandStoryboard),
                new FrameworkPropertyMetadata((ICommand)null,
                    new PropertyChangedCallback(OnCommandChanged)));

        /// <summary>
        /// Gets the Command property.  This dependency property 
        /// indicates ....
        /// </summary>
        public static ICommand GetCommand(DependencyObject d)
        {
            return (ICommand)d.GetValue(CommandProperty);
        }

        /// <summary>
        /// Sets the Command property.  This dependency property 
        /// indicates ....
        /// </summary>
        public static void SetCommand(DependencyObject d, ICommand value)
        {
            d.SetValue(CommandProperty, value);
        }

        /// <summary>
        /// Handles changes to the Command property.
        /// </summary>
        private static void OnCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var timeline = d as Timeline;
            if (timeline != null && !timeline.IsFrozen)
            {
                timeline.Completed += delegate
                {
                    ICommand command = GetCommand(d);
                    object param = GetCommandParameter(d);

                    if(command != null && command.CanExecute( param ))
                        GetCommand(d).Execute(GetCommandParameter(d));
                };
            }
        }

        #endregion
    }
}
