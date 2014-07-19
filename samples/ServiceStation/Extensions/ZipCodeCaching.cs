using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Channels;

namespace Extensions
{
    public class ZipCodeCaching : Attribute, IOperationBehavior
    {
        #region IOperationBehavior Members

        public void AddBindingParameters(OperationDescription operationDescription, 
            BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyClientBehavior(OperationDescription operationDescription, 
            ClientOperation clientOperation)
        {
        }

        public void ApplyDispatchBehavior(OperationDescription operationDescription, 
            DispatchOperation dispatchOperation)
        {
            dispatchOperation.Invoker = new ZipCodeCacher(dispatchOperation.Invoker);
        }

        public void Validate(OperationDescription operationDescription)
        {
        }

        #endregion
    }
}
