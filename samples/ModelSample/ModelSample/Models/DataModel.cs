using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Threading;

namespace ModelSample
{
    /// <summary>
    /// Base class for data models. All public methods must be called on the UI thread only.
    /// </summary>
    public class DataModel : INotifyPropertyChanged
    {
        public DataModel()
        {
            _dispatcher = Dispatcher.CurrentDispatcher;
            this.State = ModelState.Invalid;
        }

        /// <summary>
        /// Possible states for a DataModel.
        /// </summary>
        public enum ModelState
        {
            Invalid,    // The model is in an invalid state
            Fetching,   // The model is being fetched
            Valid       // The model has fetched its data
        }

        /// <summary>
        /// Is the model active?
        /// </summary>
        public bool IsActive
        {
            get
            {
                VerifyCalledOnUIThread();
                return _isActive;
            }

            private set
            {
                VerifyCalledOnUIThread();
                if (value != _isActive)
                {
                    _isActive = value;
                    SendPropertyChanged("IsActive");
                }
            }
        }

        /// <summary>
        /// Activate the model.
        /// </summary>
        public void Activate()
        {
            VerifyCalledOnUIThread();

            if (!_isActive)
            {
                this.IsActive = true;
                OnActivated();
            }
        }

        /// <summary>
        /// Override to provide behavior on activate.
        /// </summary>
        protected virtual void OnActivated()
        {
        }

        /// <summary>
        /// Deactivate the model.
        /// </summary>
        public void Deactivate()
        {
            VerifyCalledOnUIThread();

            if (_isActive)
            {
                this.IsActive = false;
                OnDeactivated();
            }
        }

        /// <summary>
        /// Override to provide behavior on deactivate.
        /// </summary>
        protected virtual void OnDeactivated()
        {
        }

        /// <summary>
        /// PropertyChanged event for INotifyPropertyChanged implementation.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged
        {
            add
            {
                VerifyCalledOnUIThread();
                _propertyChangedEvent += value;
            }
            remove
            {
                VerifyCalledOnUIThread();
                _propertyChangedEvent -= value;
            }
        }

        /// <summary>
        /// Gets or sets current state of the model.
        /// </summary>
        public ModelState State
        {
            get
            {
                VerifyCalledOnUIThread();
                return _state;
            }

            set
            {
                VerifyCalledOnUIThread();
                if (value != _state)
                {
                    _state = value;
                    SendPropertyChanged("State");
                }
            }
        }

        /// <summary>
        /// The Dispatcher associated with the model.
        /// </summary>
        public Dispatcher Dispatcher
        {
            get { return _dispatcher; }
        }

        /// <summary>
        /// Utility function for use by subclasses to notify that a property has changed.
        /// </summary>
        /// <param name="propertyName">The name of the property.</param>
        protected void SendPropertyChanged(string propertyName)
        {
            VerifyCalledOnUIThread();
            if (_propertyChangedEvent != null)
            {
                _propertyChangedEvent(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        /// <summary>
        /// Debugging utility to make sure functions are called on the UI thread.
        /// </summary>
        [Conditional("Debug")]
        protected void VerifyCalledOnUIThread()
        {
            Debug.Assert(Dispatcher.CurrentDispatcher == this.Dispatcher, "Call must be made on UI thread.");
        }

        private ModelState _state;
        private Dispatcher _dispatcher;
        private PropertyChangedEventHandler _propertyChangedEvent;
        private bool _isActive;
    }
}
