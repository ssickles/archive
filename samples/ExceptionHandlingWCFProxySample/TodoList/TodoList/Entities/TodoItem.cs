using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Text;

namespace Entities
{
    [DataContract]
    public class TodoItem : IDataErrorInfo, INotifyPropertyChanged
    {
        private string _Title;
        private DateTime? _DueDate;

        static TodoItem()
        {
            PriorityFlags.Add(PriorityFlag.High);
            PriorityFlags.Add(PriorityFlag.Normal);
            PriorityFlags.Add(PriorityFlag.Low);

            StatusFlags.Add(StatusFlag.NotStarted);
            StatusFlags.Add(StatusFlag.InProgress);
            StatusFlags.Add(StatusFlag.Deferred);
            StatusFlags.Add(StatusFlag.WaitingOnSomeoneElse);
            StatusFlags.Add(StatusFlag.Completed);
        }

        [DataMember]
        public string ID { get; set; }


        [DataMember]
        public string Title
        {
            get { return _Title; }
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new Exception("Title cannot be empty");
                _Title = value;
                NotifyPropertyChanged("Title");
            }
        }

        private string _Description;

        [DataMember]
        public string Description
        {
            get { return _Description; }
            set
            {
                _Description = value;
                NotifyPropertyChanged("Description");
            }
        }

        private PriorityFlag _Priority;

        [DataMember]
        public PriorityFlag Priority
        {
            get { return _Priority; }
            set
            {
                _Priority = value;
                NotifyPropertyChanged("Priority");
            }
        }

        private StatusFlag _Status;

        [DataMember]
        public StatusFlag Status
        {
            get { return _Status; }
            set
            {
                _Status = value;
                NotifyPropertyChanged("Status");
            }
        }

        private DateTime? _CreationDate;

        [DataMember]
        public DateTime? CreationDate
        {
            get { return _CreationDate; }
            set
            {
                if (CreationDate != null && value > DueDate)
                    throw new Exception("Creation date cannot be after Due date");
                _CreationDate = value;
                NotifyPropertyChanged("CreationDate");
            }
        }

        [DataMember]
        public DateTime? DueDate
        {
            get { return _DueDate; }
            set
            {
                if (value != null && value < CreationDate)
                    throw new Exception("Due date cannot be before creation date");
                _DueDate = value;
                NotifyPropertyChanged("DueDate");
            }
        }

        private DateTime? _CompletionDate;

        [DataMember]
        public DateTime? CompletionDate
        {
            get { return _CompletionDate; }
            set
            {
                _CompletionDate = value;
                NotifyPropertyChanged("CompletionDate");
            }
        }

        private double _PercentComplete;

        [DataMember]
        public double PercentComplete
        {
            get { return _PercentComplete; }
            set
            {
                if (_PercentComplete == 100 && value < 100)
                    _CompletionDate = null;
                if (value > 100)
                    throw new Exception("Percent value should be between 0 and 100");
                _PercentComplete = value;
                if (_PercentComplete == 100)
                    CompletionDate = DateTime.Now;
                NotifyPropertyChanged("PercentComplete");
            }
        }

        private string _Tags;

        [DataMember]
        public string Tags
        {
            get { return _Tags; }
            set
            {
                _Tags = value;
                NotifyPropertyChanged("Tags");
            }
        }

        public static ObservableCollection<PriorityFlag> PriorityFlags = new ObservableCollection<PriorityFlag>();
        public static ObservableCollection<StatusFlag> StatusFlags = new ObservableCollection<StatusFlag>();

        #region Implementation of IDataErrorInfo

        public string this[string columnName]
        {
            get
            {
                // apply property level validation rules
                if (columnName == "Title")
                {
                    if (string.IsNullOrEmpty(Title))
                        return "Title cannot be empty";
                }

                //if (columnName == "Age")
                //{
                //    if (Age < 0 || Age > 110)
                //        return "Age must be positive and less than 110";
                //}

                return "";
            }
        }


        public string Error
        {
            get
            {
                var error = new StringBuilder();

                // iterate over all of the properties
                // of this object - aggregating any validation errors
                PropertyDescriptorCollection props = TypeDescriptor.GetProperties(this);
                foreach (PropertyDescriptor prop in props)
                {
                    string propertyError = this[prop.Name];
                    if (propertyError != string.Empty)
                    {
                        error.Append((error.Length != 0 ? ", " : "") + propertyError);
                    }
                }

                // apply object level validation rules
                //if (StartTime.CompareTo(EndTime) > 0)
                //{
                //    error.Append((error.Length != 0 ? ", " : "") +
                //                  "EndTime must be after StartTime");
                //}

                return error.ToString();
            }
        }

        #endregion

        #region Implementation of INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }

    public enum PriorityFlag
    {
        Low,
        Normal,
        High
    }

    public enum StatusFlag
    {
        [Description("Not Started")] NotStarted,

        [Description("In Progress")] InProgress,

        [Description("Completed")] Completed,

        [Description("Waiting on someone else")] WaitingOnSomeoneElse,

        [Description("Deferred")] Deferred
    }
}
