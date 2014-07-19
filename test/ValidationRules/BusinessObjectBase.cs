using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Reflection;

namespace ValidationRules
{
    public class BusinessObjectBase: IDataErrorInfo, INotifyPropertyChanged
    {
        private string _errors;

        public void Validate()
        {
            OnPropertyChanged("");
        }

        #region IDataErrorInfo Members

        public string Error
        {
            get { return null; }
            private set
            {
                _errors = value;
                OnPropertyChanged("Error");
            }
        }

        public string this[string columnName]
        {
            get
            {
                try
                {
                    PropertyInfo prop = this.GetType().GetProperty(columnName);
                    IValidation[] validations = (IValidation[])prop.GetCustomAttributes(typeof(IValidation), true);
                    foreach (IValidation validation in validations)
                    {
                        string valResult = validation.Validate(prop.Name, prop.GetValue(this, null));
                        if (!string.IsNullOrEmpty(valResult))
                        {
                            Error += valResult;
                            return valResult;
                        }
                    }
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
                return string.Empty;
            }
        }

        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string PropertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(PropertyName));
        }

        #endregion
    }
}
