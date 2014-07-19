using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using MVVMHelper.Common;

namespace MVVMHelper.ViewModel
{
    /// <summary>
    /// Base class for all view models
    /// </summary>
    public class BaseViewModel : NotifyPropertyChangedBase
    {
        ServiceLocator serviceLocator = new ServiceLocator();

        /// <summary>
        /// Gets the service locator 
        /// </summary>
        public ServiceLocator ServiceLocator
        {
            get 
            {
                return serviceLocator; 
            }
        }

        /// <summary>
        /// Gets a service from the service locator
        /// </summary>
        /// <typeparam name="T">The type of service to return</typeparam>
        /// <returns>Returns a service that was registered with the Type T</returns>
        public T GetService<T>()
        {
            return serviceLocator.GetService<T>();
        }

    }
}
