using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MVVMHelper.MediatorLib;

namespace MediatorSample.ViewModels.Factories
{
    public class MediatorFactory
    {
        static Mediator commonMediator = null;
        
        public static Mediator GetCommonMediator()
        {
            if (commonMediator == null)
                commonMediator = GetNewMediator();
            return commonMediator;
        }

        public static Mediator GetNewMediator()
        {
            return new Mediator();
        }
    }

}
