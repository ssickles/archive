using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel;
using SyncRepositorySynchronization;
using System.Collections;
using WickedSick.Data;

namespace SyncRepositoryDomainModel
{
    [DataContract]
    [Table("identities")]
    public class Identity: INotifyPropertyChanged, ISynchronize
    {
        private Guid _id = Guid.NewGuid();
        private string _firstName = string.Empty;
        private string _lastName = string.Empty;
        private DateTime _lastUpdated = DateTime.MinValue;

        [DataMember]
        [Field("id", true)]
        public Guid Id
        {
            get { return _id; }
            set
            {
                if (_id != value)
                {
                    _id = value;
                    RaisePropertyChanged("Id");
                }
            }
        }

        [DataMember]
        [Field("first_name")]
        public string FirstName
        {
            get { return _firstName; }
            set
            {
                if (_firstName != value)
                {
                    _firstName = value;
                    RaisePropertyChanged("FirstName");
                }
            }
        }

        [DataMember]
        [Field("last_name")]
        public string LastName
        {
            get { return _lastName; }
            set
            {
                if (_lastName != value)
                {
                    _lastName = value;
                    RaisePropertyChanged("LastName");
                }
            }
        }

        [DataMember]
        [Field("_last_updated")]
        public DateTime LastUpdated
        {
            get { return _lastUpdated; }
            set
            {
                if (_lastUpdated != value)
                {
                    _lastUpdated = value;
                    RaisePropertyChanged("LastUpdated");
                }
            }
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string PropertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(PropertyName));
        }

        #endregion

        #region IEquatable<ISynchronize> Members

        public bool Equals(ISynchronize other)
        {
            Identity i = other as Identity;
            if (i != null && i.Id == Id)
                return true;
            else
                return false;
        }

        #endregion
    }
}
