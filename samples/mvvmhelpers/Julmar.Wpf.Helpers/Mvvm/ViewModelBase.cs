using System;
using System.ComponentModel;
using System.Diagnostics;

namespace JulMar.Windows.Mvvm
{
    /// <summary>
    /// This class is used as the basis for all ModelView objects
    /// </summary>
    public class ViewModelBase : INotifyPropertyChanged, IDisposable
    {
        /// <summary>
        /// This event should be raised to close the UI.  Any view (Window) tied to this
        /// ViewModel should register a handler on this event and close itself when
        /// this event is raised.  If the view is not bound to the lifetime of the
        /// ViewModel then this event can be ignored.
        /// </summary>
        public event EventHandler CloseRequest = delegate { };

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        /// <summary>
        /// Constructor for the ViewModel
        /// </summary>
        public ViewModelBase()
        {
            MessageMediator.Register(this);
        }

        /// <summary>
        /// This raises the CloseRequest event to close the UI.
        /// </summary>
        protected virtual void RaiseCloseRequest()
        {
            CloseRequest(this, EventArgs.Empty);
        }

        /// <summary>
        /// This can be used to indicate that all property values have changed.
        /// </summary>
        protected void OnPropertyChanged() 
        {
            OnPropertyChanged(null);
        }

        /// <summary>
        /// This raises the INotifyPropertyChanged.PropertyChanged event to indicate
        /// a specific property has changed value.
        /// </summary>
        /// <param name="name"></param>
        protected virtual void OnPropertyChanged(string name)
        {
            Debug.Assert(string.IsNullOrEmpty(name) || GetType().GetProperty(name) != null);
            PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        /// <summary>
        /// This sends a message via the MessageMediator.
        /// </summary>
        /// <param name="key">Message key</param>
        /// <param name="message">Message parameter</param>
        /// <returns>True if at least one recipient received the message</returns>
        protected bool SendMessage(string key, object message)
        {
            return MessageMediator.SendMessage(key, message);
        }

        #region IDisposable Members

        /// <summary>
        /// This disposes of the view model.  It unregisters from the message mediator.
        /// </summary>
        /// <param name="isDisposing">True if IDisposable.Dispose was called</param>
        protected virtual void Dispose(bool isDisposing)
        {
            MessageMediator.Unregister(this);
        }

        /// <summary>
        /// Implementation of IDisposable.Dispose.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}