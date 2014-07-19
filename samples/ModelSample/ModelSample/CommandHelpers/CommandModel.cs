using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace ModelSample
{
    /// <summary>
    /// Model for a command
    /// </summary>
    public abstract class CommandModel
    {
        public CommandModel()
        {
            _routedCommand = new RoutedCommand();
        }

        /// <summary>
        /// Routed command associated with the model.
        /// </summary>
        public RoutedCommand Command
        {
            get { return _routedCommand; }
        }

        /// <summary>
        /// Determines if a command is enabled. Override to provide custom behavior. Do not call the
        /// base version when overriding.
        /// </summary>
        public virtual void OnQueryEnabled(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
            e.Handled = true;
        }

        /// <summary>
        /// Function to execute the command.
        /// </summary>
        public abstract void OnExecute(object sender, ExecutedRoutedEventArgs e);

        private RoutedCommand _routedCommand;
    }
}
