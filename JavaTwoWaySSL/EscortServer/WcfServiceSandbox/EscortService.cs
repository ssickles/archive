using System;
using System.ServiceModel;
using WcfBehaviorExtensionSandbox;

namespace WcfServiceSandbox
{
    [ApplicationRequestContext]
    public class EscortService : IEscortService
    {
        #region IEscortService Members

        public bool Escort(int quantity)
        {
            var identityUid = ApplicationRequestContext.Current.IdentityUid;
            Console.WriteLine(identityUid);
            return quantity > 1;
        }

        #endregion
    }
}
