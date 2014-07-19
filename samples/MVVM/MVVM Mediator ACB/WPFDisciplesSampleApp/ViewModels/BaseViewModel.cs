using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using WPFDisciples.Common;

namespace WPFDisciples.ViewModels
{
    /// <summary>
    /// Base View Model that implements the INotifyPropertyChanged for all other Models
    /// </summary>
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Gets the Mediator instance (this is done to avoid having to write Mediator.Instance everytime)
        /// </summary>
        public Mediator Mediator
        {
            get
            {
                return Mediator.Instance;
            }
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
