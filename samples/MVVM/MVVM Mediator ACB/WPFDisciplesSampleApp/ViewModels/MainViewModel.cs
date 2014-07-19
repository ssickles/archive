using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WPFDisciples.Common;
using AttachedCommandBehavior;
using System.Windows.Threading;

namespace WPFDisciples.ViewModels
{
    /// <summary>
    /// Main View ViewModel
    /// </summary>
    public class MainViewModel : BaseViewModel
    {
        private bool showDevelopers = false;
        /// <summary>
        /// Gets or sets a bool to check if the Developers View should be Visible
        /// </summary>
        public bool ShowDevelopers
        {
            get { return showDevelopers; }
            set
            {
                showDevelopers = value;
                OnPropertyChanged("ShowDevelopers");
            }
        }

        bool showRichText;
        /// <summary>
        /// Gets or sets a bool to check if the Rich Text View should be Visible
        /// </summary>
        public bool ShowRichText
        {
            get { return showRichText; }
            set
            {
                showRichText = value;
                OnPropertyChanged("ShowRichText");
            }
        }

        bool showError;
        /// <summary>
        /// Gets or sets a bool to check if the Error View should be Visible
        /// </summary>
        public bool ShowError
        {
            get { return showError; }
            private set
            {
                showError = value;
                OnPropertyChanged("ShowError");
            }
        }

        string error;
        /// <summary>
        /// Gets or sets the error Text
        /// </summary>
        public string Error
        {
            get { return error; }
            private set
            {
                error = value;
                ShowError = !String.IsNullOrEmpty(error);
                OnPropertyChanged("Error");
            }
        }

        bool showNotification;
        /// <summary>
        /// Gets or sets a flag to show the notification view
        /// </summary>
        public bool ShowNotification
        {
            get { return showNotification; }
            set
            {
                showNotification = value;
                OnPropertyChanged("ShowNotification");
            }
        }

        string notificationMessage;
        /// <summary>
        /// Gets or sets the notification message to show
        /// </summary>
        public string NotificationMessage
        {
            get { return notificationMessage; }
            set
            {
                notificationMessage = value;
                ShowNotification = !String.IsNullOrEmpty(notificationMessage);
                OnPropertyChanged("NotificationMessage");
            }
        }

        private string currentView;
        /// <summary>
        /// Gets or sets the Current view
        /// </summary>
        public string CurrentView
        {
            get { return currentView; }
            set
            {
                currentView = value;
                OnPropertyChanged("CurrentView");
            }
        }

        /// <summary>
        /// Selects the View to display
        /// </summary>
        public SimpleCommand SelectView { get; private set; }

        /// <summary>
        /// Gets an action to hide the Error messages
        /// </summary>
        public Action<object> HideError { get; private set; }

        /// <summary>
        /// Gets an action to hide the Notification messages
        /// </summary>
        public Action<object> HideNotification { get; private set; }

        /// <summary>
        /// Gets an action that opens the browser page for WPF Disciples
        /// </summary>
        public Action<object> VisitTheDisciples { get; private set; }

        /// <summary>
        /// Guess what.... this is the Constructor... LOL sorry about this comment, I guess I have nothing better to do :D
        /// </summary>
        public MainViewModel()
        {
            //Register for notification to show or Hide the Developers View
            Mediator.Register(x => ShowDevelopers = true, ViewModelMessages.ShowDevelopers);
            Mediator.Register(x => ShowDevelopers = false, ViewModelMessages.SelectDeveloper);

            //Register for notification to show or Hide the RichText View
            Mediator.Register(x => ShowRichText = true, ViewModelMessages.ShowRichText);
            Mediator.Register(x => ShowRichText = false, ViewModelMessages.HideRichText);

            //Register for notification to show the Errors View
            Mediator.Register(x => Error = x.ToString(), ViewModelMessages.ShowError);

            //Initialize the action to Hide the errors View
            HideError = x => Error = null;

            Mediator.Register(x => NotificationMessage = x.ToString(), ViewModelMessages.NotificationMessage);

            //Initialize the action to Hide the notification View
            HideNotification = x => NotificationMessage = null;

            VisitTheDisciples = x => Dispatcher.CurrentDispatcher.BeginInvoke(
                (Action)delegate { System.Diagnostics.Process.Start("http://groups.google.com/group/wpf-disciples"); }, DispatcherPriority.Background, null);

            //Switch the view name. This will be picked up by binding and the view will change accordingly
            SelectView = new SimpleCommand
            {
                ExecuteDelegate = x => CurrentView = (string)x
            };
        }
    }
}
