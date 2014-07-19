using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MVVMHelper.Factory;
using MVVMHelper.MediatorLib;
using FileSystemServices;

namespace MediatorSample.ViewModels.Factories
{
    public class FilesViewModelFactory : IFactory
    {
        #region IFactory Members

        public object CreateViewModel(System.Windows.DependencyObject sender)
        {
            var vm = new FilesViewModel();
            vm.ServiceLocator.RegisterService<Mediator>(MediatorFactory.GetCommonMediator());
            vm.ServiceLocator.RegisterService<IExplorer>(new Explorer());
            vm.Initialize();
            return vm;
        }

        #endregion
    }
}
