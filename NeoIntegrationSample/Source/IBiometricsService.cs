using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace NeoIntegrationSample
{
    [ServiceContract]
    public interface IBiometricsService
    {
        [OperationContract]
        bool VerifyCustomer(string sourceSystemId, string firstName, string lastName);
    }
}
