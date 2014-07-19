using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WPFDisciples.Backend;
using System.Collections.ObjectModel;
using System.Windows.Threading;
using WPFDisciples.Common;
using AttachedCommandBehavior;

namespace WPFDisciples.ViewModels
{
    /// <summary>
    /// View Model for the Developers View
    /// </summary>
    public class DevelopersViewModel : BaseViewModel
    {
        /// <summary>
        /// Gets the list of Developers
        /// </summary>
        public ObservableCollection<WPFDisciples.Backend.Developers> Developers { get; private set; }

        /// <summary>
        /// Command to select a Developer
        /// </summary>
        public SimpleCommand SelectDeveloper { get; private set; }

        int selectedDeveloperIndex;

        /// <summary>
        /// Gets or sets the Selected Developer index
        /// </summary>
        public int SelectedDeveloperIndex
        {
            get { return selectedDeveloperIndex; }
            set
            {
                selectedDeveloperIndex = value;
                //Set the selected Developer
                SelectedDeveloper = Developers[Math.Min(Math.Max(0, selectedDeveloperIndex), Developers.Count)];
                OnPropertyChanged("SelectedDeveloperIndex");
            }
        }

        private WPFDisciples.Backend.Developers selectedDeveloper;

        public WPFDisciples.Backend.Developers SelectedDeveloper
        {
            get { return selectedDeveloper; }
            set
            {
                selectedDeveloper = value;
                OnPropertyChanged("SelectedDeveloper");
            }
        }

        Dispatcher dispatcher;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dispatcher">The dispatcher of the View</param>
        public DevelopersViewModel(Dispatcher dispatcher)
        {
            this.dispatcher = dispatcher;
            Developers = new ObservableCollection<WPFDisciples.Backend.Developers>();

            //Register to the 
            Mediator.Register(x => LoadData(), ViewModelMessages.ShowDevelopers);

            //Create the SelectDeveloper handlers
            SelectDeveloper = new SimpleCommand
            {
                //Notify subscibers that a developer has been selected and pass on the selected developer as parameter
                ExecuteDelegate = x => Mediator.NotifyColleagues(ViewModelMessages.SelectDeveloper, SelectedDeveloper)
            };
        }

        /// <summary>
        /// Loads the Developer data and add it to the Developers List
        /// </summary>
        public void LoadData()
        {
            //use dispatcher to do this when the dispatcher is not busy loading UI. In a real world app this should be done in a sepearte thread.
            dispatcher.BeginInvoke((Action)delegate
            {
                Developers.Clear();
                var data = DevelopersDataService.GetDevelopers();
                if (data != null)
                {
                    foreach (var item in data)
                        Developers.Add(item);
                    SelectedDeveloperIndex = 0;
                }
                else
                {
                    //Notify Mediator there was an error
                    Mediator.NotifyColleagues(ViewModelMessages.ShowError, "Error getting list of developers from DB");
                }
            }, DispatcherPriority.Background, null);

        }
    }
}
