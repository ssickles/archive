using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace WPFDisciples.Backend
{
    /// <summary>
    /// Represents a project
    /// </summary>
    public class ProjectDetails : INotifyPropertyChanged, IDataErrorInfo
    {
        int projectId;

        /// <summary>
        /// Gets or sets the project id
        /// </summary>
        public int ProjectId
        {
            get { return projectId; }
            set
            {
                OnPropertyChanged("ProjectId");
                projectId = value;
            }
        }

        string link;
        const string LinkProperty = "Link";
        /// <summary>
        /// Gets or sets a link to the project
        /// </summary>
        public string Link
        {
            get { return link; }
            set
            {
                link = value;
                OnPropertyChanged(LinkProperty);
            }
        }

        string name;
        const string NameProperty = "Name";
        /// <summary>
        /// Gets or sets the project name.
        /// </summary>
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged(NameProperty);
            }
        }

        string description;
        const string DescriptionProperty = "Description";
        /// <summary>
        /// Gets or sets the project description.
        /// </summary>
        public string Description
        {
            get { return description; }
            set
            {
                description = value;
                OnPropertyChanged(DescriptionProperty);
            }
        }

        string developer;
        const string DeveloperProperty = "Developer";
        /// <summary>
        /// Gets or sets the project developer name.
        /// </summary>
        public string Developer
        {
            get { return developer; }
            set
            {
                developer = value;
                OnPropertyChanged(DeveloperProperty);
            }
        }

        string developerPic;
        const string DeveloperPicProperty = "DeveloperPic";
        /// <summary>
        /// Gets or sets the project developer picture.
        /// </summary>
        public string DeveloperPic
        {
            get { return developerPic; }
            set
            {
                developerPic = value;
                OnPropertyChanged(DeveloperPicProperty);
            }
        }

        int developerId;
        /// <summary>
        /// Gets or sets the DeveloperId
        /// </summary>
        public int DeveloperId
        {
            get { return developerId; }
            set
            {
                developerId = value;
                OnPropertyChanged("DeveloperId");
            }
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        #region IDataErrorInfo Members
        //stores the list of properties that have errors

        List<string> errors = new List<string>();
        /// <summary>
        /// Gets the error message to display. Returns null if there is not an error
        /// </summary>
        public string Error
        {
            get
            {
                if (errors.Count != 0)
                    return "There are some errors on this page!";
                else
                    return String.Empty;
            }
            private set
            {
                OnPropertyChanged("Error");
            }
        }

        /// <summary>
        /// Validates the data in the object
        /// </summary>
        /// <param name="columnName"></param>
        /// <returns>Returns null if there is no error. Returns the error string if there is an error</returns>
        public string this[string columnName]
        {
            get
            {
                string result = null;
                switch (columnName)
                {
                    case NameProperty:
                        if (String.IsNullOrEmpty(Name))
                            result = "Please enter a valid name";
                        break;
                    case DescriptionProperty:
                        if (String.IsNullOrEmpty(Description))
                            result = "Please enter a description";
                        break;
                    case LinkProperty:
                        if (String.IsNullOrEmpty(Link))
                            result = "Please enter a link";
                        break;
                    case DeveloperProperty:
                        if (String.IsNullOrEmpty(Developer))
                            result = "Please enter a developer";
                        break;
                    default:
                        break;
                }
                if (result != null)//if there is an error add the columnName to the list of errors
                    errors.Add(columnName);
                else
                    errors.Remove(columnName);//else remove it from the list
                Error = result;
                return result;
            }
        }

        #endregion
    }
}
