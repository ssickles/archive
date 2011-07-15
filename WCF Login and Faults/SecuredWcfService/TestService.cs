using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IdentityModel.Policy;
using System.ServiceModel;

namespace SecuredWcfService
{
    public class TestService: ITestService
    {
        #region ITestService Members

        public string Hello()
        {
            return string.Format("Hello {0}", OperationContext.Current.ServiceSecurityContext.PrimaryIdentity.Name);
        }

        #endregion
    }
}
