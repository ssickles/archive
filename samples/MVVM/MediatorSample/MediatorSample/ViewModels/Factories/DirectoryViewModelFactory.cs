using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MVVMHelper.Factory;
using MVVMHelper.MediatorLib;
using FileSystemServices;

namespace MediatorSample.ViewModels.Factories
{
    public class DirectoryViewModelFactory : IFactory
    {
        #region IFactory Members

        public object CreateViewModel(System.Windows.DependencyObject sender)
        {
            //For this demo I did not create a design time model.

            var vm = new DirectoriesViewModel();
            vm.ServiceLocator.RegisterService<Mediator>(
                MediatorFactory.GetCommonMediator());
            vm.ServiceLocator.RegisterService<IExplorer>(
                new Explorer());
            vm.LoadData();
            return vm;
        }

        #endregion
    }
}
