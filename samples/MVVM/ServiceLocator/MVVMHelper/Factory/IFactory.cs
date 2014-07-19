using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace MVVMHelper.Factory
{
    public interface IFactory
    {
        object CreateViewModel(DependencyObject sender);
    }
}
