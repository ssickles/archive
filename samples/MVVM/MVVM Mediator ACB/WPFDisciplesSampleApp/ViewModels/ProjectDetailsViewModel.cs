using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WPFDisciples.Common;
using WPFDisciples.Backend;
using AttachedCommandBehavior;

namespace WPFDisciples.ViewModels
{
    /// <summary>
    /// ViewModel for the ProjectDetails
    /// </summary>
    public class ProjectDetailsViewModel : BaseViewModel
    {
        /// <summary>
        /// Gets a Command To show the developers View
        /// </summary>
        public SimpleCommand ShowDevelopers { get; private set; }

        /// <summary>
        /// Gets a Command To show the rich text View
        /// </summary>
        public SimpleCommand ShowRichText { get; private set; }

        /// <summary>
        /// Gets a Command to save the current project
        /// </summary>
        public SimpleCommand SaveProject { get; private set; }

        WPFDisciples.Backend.ProjectDetails project;

        /// <summary>
        /// Gets the current project being viewed
        /// </summary>
        public WPFDisciples.Backend.ProjectDetails Project
        {
            get { return project; }
            set
            {
                project = value;
                OnPropertyChanged("Project");
            }
        }

        /// <summary>
        /// Gets or sets a flag to check if the view is in edit mode
        /// </summary>
        public bool IsEditMode { get; set; }

        public ProjectDetailsViewModel()
        {
            Project = new WPFDisciples.Backend.ProjectDetails();

            //initialize the ShowDevelopers Command
            ShowDevelopers = new SimpleCommand
            {
                //Notify listeners to show the developers View
                ExecuteDelegate = x => Mediator.NotifyColleagues(ViewModelMessages.ShowDevelopers, null)
            };

            ShowRichText = new SimpleCommand
            {
                //Notify listeners to show the rich text View
                ExecuteDelegate = x => Mediator.NotifyColleagues(ViewModelMessages.ShowRichText, x)
            };

            SaveProject = new SimpleCommand
            {
                ExecuteDelegate = Save,
                //Can execute only if there are no errors
                CanExecuteDelegate = x => String.IsNullOrEmpty(Project.Error)
            };

            Mediator.Register(
                x =>
                {
                    WPFDisciples.Backend.Developers dev = x as WPFDisciples.Backend.Developers;
                    if (dev != null)
                    {
                        Project.Developer = dev.Name;
                        Project.DeveloperId = dev.id;
                        Project.DeveloperPic = dev.Avatar;
                    }
                },
                ViewModelMessages.SelectDeveloper);

            //Receive a notification with the new Text that was entered in the RichText view for the Description
            Mediator.Register(x => Project.Description = x.ToString(), ViewModelMessages.HideRichText);
        }

        private void Save(object x)
        {
            if (IsEditMode)
            {
                if (ProjectsDataService.UpdateProject(Project))
                    Mediator.NotifyColleagues(ViewModelMessages.NotificationMessage, "Project details updated!");
                else
                    Mediator.NotifyColleagues(ViewModelMessages.ShowError, "Error saving project in database");
            }
            else
            {
                if (ProjectsDataService.AddNewProject(Project))
                {
                    //Notify Mediator that the project was added
                    Mediator.NotifyColleagues(ViewModelMessages.NewProjectAdded, Project);
                    //also send a notification to user
                    Mediator.NotifyColleagues(ViewModelMessages.NotificationMessage, "New project added!");

                    //clear the fields in the View
                    Project = new WPFDisciples.Backend.ProjectDetails();
                }
                else
                    //Notify Mediator there was an error
                    Mediator.NotifyColleagues(ViewModelMessages.ShowError, "Error saving project in database");
            }
        }
    }
}
