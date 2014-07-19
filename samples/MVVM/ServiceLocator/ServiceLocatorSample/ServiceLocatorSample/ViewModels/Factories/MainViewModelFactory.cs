using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using ServiceLocatorSample.DataAccess;
using MVVMHelper.Factory;
using MVVMHelper.Common;

namespace ServiceLocatorSample.ViewModels.Factories
{
    public class MainViewModelFactory : IFactory
    {
        public object CreateViewModel(DependencyObject sender)
        {
            var vm = new MainViewModel();
            if (Designer.IsDesignMode)
                vm.ServiceLocator.RegisterService<IPeopleDataAccess>(new DesignTimeData.DesignTimePeopleDataAccess());
            else
                vm.ServiceLocator.RegisterService<IPeopleDataAccess>(new PeopleDataAccess());
            return vm;
        }
    }
}
