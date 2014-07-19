using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SyncRepositoryProviders.SyncReference;
using SyncRepositorySynchronization;
using SyncRepositoryDomainModel;

namespace SyncRepositoryProviders
{
    public class IdentityProvider : ISyncProvider<Identity>
    {
        public IdentityProvider()
        {

        }

        #region ISyncProvider<Identity> Members

        public List<Identity> Get(DateTime LastUpdated)
        {
            SyncClient proxy = new SyncClient();
            List<Identity> identities = proxy.GetIdentities(new Request() { LastRequest = LastUpdated });
            proxy.Close();
            return identities;
        }

        public void Save(Identity Item)
        {
            SyncClient proxy = new SyncClient();
            proxy.UpdateIdentity(Item);
            proxy.Close();
        }

        public void Delete(Identity Item)
        {
            SyncClient proxy = new SyncClient();
            proxy.DeleteIdentity(Item);
            proxy.Close();
        }

        #endregion
    }
}
