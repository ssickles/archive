using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel.Dispatcher;

namespace Extensions
{
    public class ZipCodeCacher : IOperationInvoker
    {
        IOperationInvoker innerOperationInvoker;
        Dictionary<string, string> zipCodeCache = new Dictionary<string, string>();

        public ZipCodeCacher(IOperationInvoker innerOperationInvoker)
        {
            this.innerOperationInvoker = innerOperationInvoker;
        }

        #region IOperationInvoker Members

        public object[] AllocateInputs()
        {
            return this.innerOperationInvoker.AllocateInputs();
        }

        public object Invoke(object instance, object[] inputs, out object[] outputs)
        {
            string zipcode = inputs[0] as string;
            string value;

            if (this.zipCodeCache.TryGetValue(zipcode, out value))
            {
                outputs = new object[0];
                return value;
            }
            else
            {
                value = (string)this.innerOperationInvoker.Invoke(
                    instance, inputs, out outputs);
                zipCodeCache[zipcode] = value;
                return value;
            }
        }

        public IAsyncResult InvokeBegin(object instance, object[] inputs, 
            AsyncCallback callback, object state)
        {
            return this.innerOperationInvoker.InvokeBegin(
                instance, inputs, callback, state);
        }

        public object InvokeEnd(object instance, out object[] outputs, IAsyncResult result)
        {
            return this.innerOperationInvoker.InvokeEnd(
                instance, out outputs, result);
        }

        public bool IsSynchronous
        {
            get { return innerOperationInvoker.IsSynchronous;  }
        }

        #endregion
    }
}
