using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SyncRepositoryDomainModel;
using SyncRepositorySynchronization;
using SyncRepositoryProviders;
using System.Collections.ObjectModel;

namespace SyncRepositoryClient
{
    public class Repository
    {
        private DomainModelSync<Identity> _identitySync;

        public Repository()
        {
            _identitySync = new DomainModelSync<Identity>(new IdentityProvider());
        }

        static Repository()
        {
            if (Current == null)
                Current = new Repository();
        }

        public void Activate()
        {
            _identitySync.Activate(10000);
        }

        public static Repository Current { get; private set; }

        public ObservableCollection<Identity> Identities
        {
            get { return _identitySync.Items; }
        }
    }
}
