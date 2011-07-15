using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Windows.Threading;
using System.IO;
using System.ServiceModel.Web;

namespace NeoIntegrationSample
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public class BiometricsService: IBiometricsService
    {
        #region IBiometricsService Members

        public bool VerifyCustomer(string sourceSystemId, string firstName, string lastName)
        {
            CustomerVerification customerVerify = new CustomerVerification(sourceSystemId, string.Concat(firstName.Trim(), lastName.Trim()));
            bool? result = customerVerify.ShowDialog();
            if (result.HasValue)
                return result.Value;
            else
                return false;
        }

        #endregion
    }
}
