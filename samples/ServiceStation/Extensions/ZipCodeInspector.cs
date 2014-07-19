using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Dispatcher;
using System.Text.RegularExpressions;

namespace Extensions
{
    public class ZipCodeInspector : IParameterInspector
    {
        int zipCodeParamIndex;
        string zipCodeFormat = @"\d{5}-\d{4}";

        public ZipCodeInspector() : this(0) { }

        public ZipCodeInspector(int zipCodeParamIndex)
        {
            this.zipCodeParamIndex = zipCodeParamIndex;
        }

        #region IParameterInspector Members

        public void AfterCall(string operationName, object[] outputs, object returnValue, object correlationState)
        {
        }

        public object BeforeCall(string operationName, object[] inputs)
        {
            string zipCodeParam = inputs[this.zipCodeParamIndex] as string;
            if (!Regex.IsMatch(zipCodeParam, this.zipCodeFormat, RegexOptions.None))
                throw new FaultException("Invalid zip code format. Required format: #####-####");
            return null;
        }

        #endregion
    }
}
