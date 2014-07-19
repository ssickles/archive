using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using WPFDisciples.Backend;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Threading;
using System.Threading;
using WPFDisciples.Common;
using AttachedCommandBehavior;

namespace WPFDisciples.ViewModels
{
    /// <summary>
    /// View Model for the Project List
    /// </summary>
    public class ProjectListViewModel : BaseViewModel
    {
        #region Properties
        ObservableCollection<WPFDisciples.Backend.ProjectDetails> projects = new ObservableCollection<WPFDisciples.Backend.ProjectDetails>();

        /// <summary>
        /// Gets the list of projects
        /// </summary>
        public ObservableCollection<WPFDisciples.Backend.ProjectDetails> Projects
        {
            get { return projects; }
        }

        /// <summary>
        /// Gets an action that opens the browser page for WPF Disciples
        /// </summary>
        public Action<object> OpenUrl { get; private set; }

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
        public Action<object> SelectView { get; private set; }

        /// <summary>
        /// Command to delete a single project
        /// </summary>
        public SimpleCommand DeleteProject { get; private set; }

        #endregion

        Dispatcher dispatcher;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dispatcher">The dispatcher object of the view</param>
        public ProjectListViewModel(Dispatcher dispatcher)
        {
            this.dispatcher = dispatcher;

            //Register to the ShowListOfProjects message because this means that we added a new project and that we should add it to the list
            Mediator.Register(x =>
                //We could have got the data from the database, but the mediator will give us the new object and we can simple add it to the list and let ListBinding work it's magic :)
                    projects.Add((WPFDisciples.Backend.ProjectDetails)x),
                    ViewModelMessages.NewProjectAdded);


            OpenUrl = x => dispatcher.BeginInvoke(
                (Action)delegate
                {
                    try
                    {
                        System.Diagnostics.Process.Start((string)x);
                    }
                    catch
                    {
                        Mediator.NotifyColleagues(ViewModelMessages.ShowError, "Error opening URL");
                    }
                }, DispatcherPriority.Background, null);

            DeleteProject = new SimpleCommand
            {
                ExecuteDelegate = x =>
                {
                    var proj = (WPFDisciples.Backend.ProjectDetails)x;
                    if (ProjectsDataService.DeleteProject(proj))
                    {
                        Mediator.NotifyColleagues(ViewModelMessages.NotificationMessage, "Project deleted!");
                        Projects.Remove(proj);
                    }
                    else
                        Mediator.NotifyColleagues(ViewModelMessages.ShowError, "Cannot delete project!");

                }
            };

            //Switch the view name. This will be picked up by binding and the view will change accordingly
            SelectView = x => CurrentView = (string)x;
        }

        /// <summary>
        /// Loads the data for the View
        /// </summary>
        public void LoadData()
        {
            //use dispatcher to do this when the dispatcher is not busy loading UI. In a real world app this should be done in a sepearte thread.
            dispatcher.BeginInvoke((Action)delegate
            {
                var data = ProjectsDataService.GetProjects();
                if (data != null)
                {
                    foreach (var item in data)
                        projects.Add(item);
                }
                else
                {
                    //Notify mediator there was an error
                    Mediator.NotifyColleagues(ViewModelMessages.ShowError, "Cannot get the list of projects from DB");
                }
            }, DispatcherPriority.Background, null);
        }
    }
}
