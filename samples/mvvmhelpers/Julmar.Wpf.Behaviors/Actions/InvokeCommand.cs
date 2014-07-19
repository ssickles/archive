﻿using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows.Controls.Primitives;

namespace JulMar.Windows.Actions
{
    /// <summary>
    /// This trigger action binds a command/command parameter for MVVM usage with 
    /// a Blend based trigger.  This is used in place of the one in the Blend samples - 
    /// it has a problem in it as of the current (first) release.  Once it is fixed, this
    /// command can go away.
    /// </summary>
    [DefaultTrigger(typeof(ButtonBase), typeof(System.Windows.Interactivity.EventTrigger), "Click"), DefaultTrigger(typeof(UIElement), typeof(System.Windows.Interactivity.EventTrigger), "MouseLeftButtonDown")]
    public class InvokeCommand : TriggerAction<FrameworkElement>
    {
        /// <summary>
        /// ICommand to execute
        /// </summary>
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register("Command", typeof(ICommand), typeof(InvokeCommand),
                                        new PropertyMetadata(null));

        /// <summary>
        /// Command parameter to pass to command execution
        /// </summary>
        public static readonly DependencyProperty CommandParameterProperty = 
            DependencyProperty.Register("CommandParameter", typeof(object), typeof(InvokeCommand), 
                                        new PropertyMetadata(null));

        /// <summary>
        /// Command to execute
        /// </summary>
        public ICommand Command
        {
            get { return (ICommand)base.GetValue(CommandProperty); }
            set { base.SetValue(CommandProperty, value); }
        }

        /// <summary>
        /// Command parameter
        /// </summary>
        public object CommandParameter
        {
            get { return base.GetValue(CommandParameterProperty); }    
            set { base.SetValue(CommandParameterProperty, value);}
        }

        /// <summary>
        /// This is called to execute the command when the trigger conditions are satisified.
        /// </summary>
        /// <param name="parameter">parameter (not used)</param>
        protected override void Invoke(object parameter)
        {
            ICommand command = this.Command;
            if ((command != null) && command.CanExecute(this.CommandParameter))
                command.Execute(this.CommandParameter);
        }
    }
}
