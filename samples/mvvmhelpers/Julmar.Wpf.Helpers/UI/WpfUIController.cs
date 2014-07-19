using System;
using System.Collections.Generic;
using System.Windows;
using JulMar.Windows.Interfaces;
using JulMar.Windows.Mvvm;

namespace JulMar.Windows.UI
{
    /// <summary>
    /// This class implements the IUIController for WPF.
    /// </summary>
    public class WpfUIController : IUIController
    {
        private readonly Dictionary<string, Type> _registeredWindows;

        /// <summary>
        /// Constructor
        /// </summary>
        public WpfUIController()
        {
            _registeredWindows = new Dictionary<string, Type>();
        }

        /// <summary>
        /// Constructor that takes a pre-built Dictionary to map types to keys.
        /// </summary>
        /// <param name="startupData"></param>
        public WpfUIController(Dictionary<string, Type> startupData)
        {
            _registeredWindows = startupData;
        }

        /// <summary>
        /// Registers a type through a key.
        /// </summary>
        /// <param name="key">Key for the UI dialog</param>
        /// <param name="winType">Type which implements dialog</param>
        public void Register(string key, Type winType)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException("key");
            if (winType == null)
                throw new ArgumentNullException("winType");
            if (!typeof(Window).IsAssignableFrom(winType))
                throw new ArgumentException("winType must be of type Window");

            lock(_registeredWindows)
            {
                _registeredWindows.Add(key, winType);
            }
        }

        /// <summary>
        /// This unregisters a type and removes it from the mapping
        /// </summary>
        /// <param name="key">Key to remove</param>
        /// <returns>True/False success</returns>
        public bool Unregister(string key)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException("key");

            lock (_registeredWindows)
            {
                return _registeredWindows.Remove(key);
            }
        }

        /// <summary>
        /// This method displays a modaless dialog associated with the given key.
        /// </summary>
        /// <param name="key">Key previously registered with the UI controller.</param>
        /// <param name="state">Object state to associate with the dialog</param>
        /// <param name="completedProc">Callback used when UI closes (may be null)</param>
        /// <returns>True/False if UI is displayed</returns>
        public bool Show(string key, object state, EventHandler<UICompletedEventArgs> completedProc)
        {
            Window win = CreateWindow(key, state, completedProc);
            if (win != null)
            {
                win.Show();
                return true;
            }
            return false;
        }

        /// <summary>
        /// This method displays a modal dialog associated with the given key.
        /// </summary>
        /// <param name="key">Key previously registered with the UI controller.</param>
        /// <param name="state">Object state to associate with the dialog</param>
        /// <returns>True/False if UI is displayed.</returns>
        public bool? ShowDialog(string key, object state)
        {
            Window win = CreateWindow(key, state, null);
            if (win != null)
                return win.ShowDialog();
            
            return false;
        }

        /// <summary>
        /// This creates the WPF window from a key.
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="dataContext">DataContext (state) object</param>
        /// <param name="completedProc">Callback</param>
        /// <returns>Success code</returns>
        private Window CreateWindow(string key, object dataContext, EventHandler<UICompletedEventArgs> completedProc)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException("key");

            Type winType;
            lock (_registeredWindows)
            {
                if (!_registeredWindows.TryGetValue(key, out winType))
                    return null;
            }

            var win = (Window) Activator.CreateInstance(winType);
            win.Owner = Application.Current.MainWindow;
            win.DataContext = dataContext;

            if (dataContext != null)
            {
                var bmv = dataContext as ViewModelBase;
                if (bmv != null)
                    bmv.CloseRequest += delegate { win.Close(); };
            }

            if (completedProc != null)
            {
                win.Closed +=
                    (s, e) =>
                    completedProc(this,
                                  new UICompletedEventArgs { State = dataContext, Result = win.DialogResult });

            }

            return win;
        }
    }
}