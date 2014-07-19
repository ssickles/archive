using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Channels;

namespace Extensions
{
    public class ZipCodeValidation : Attribute, IOperationBehavior
    {
        #region IOperationBehavior Members

        public void AddBindingParameters(OperationDescription operationDescription, 
            BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyClientBehavior(OperationDescription operationDescription, 
            ClientOperation clientOperation)
        {
            ZipCodeInspector zipCodeInspector = new ZipCodeInspector();
            clientOperation.ParameterInspectors.Add(zipCodeInspector);
        }

        public void ApplyDispatchBehavior(OperationDescription operationDescription, 
            DispatchOperation dispatchOperation)
        {
            ZipCodeInspector zipCodeInspector = new ZipCodeInspector();
            dispatchOperation.ParameterInspectors.Add(zipCodeInspector);
        }

        public void Validate(OperationDescription operationDescription)
        {
        }

        #endregion
    }
}
