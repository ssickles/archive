using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace WcfBehaviorExtensionSandbox
{
    public class ApplicationRequestContext : IExtension<OperationContext>
    {
        #region IExtension<OperationContext> Members

        public void Attach(OperationContext owner)
        {
            string name = owner.IncomingMessageProperties.Security.ServiceSecurityContext.PrimaryIdentity.Name;
            //_IdentityUid = new Guid(name);
            _IdentityUid = Guid.Empty;
        }

        public void Detach(OperationContext owner)
        {

        }

        #endregion

        private Guid _IdentityUid;
        public Guid IdentityUid
        {
            get { return _IdentityUid; }
        }

        public static ApplicationRequestContext Current
        {
            get { return OperationContext.Current.Extensions.Find<ApplicationRequestContext>(); }
        }
    }
}
