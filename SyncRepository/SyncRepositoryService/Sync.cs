using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using SyncRepositoryDomainModel;

namespace SyncRepositoryService
{
    public class Sync : ISync
    {
        private static List<Identity> _identities = new List<Identity>();

        static Sync()
        {
            _identities.Add(new Identity() { Id = new Guid("6e40bfcb-e639-430b-b616-47ef47c0cf6f"), FirstName = "Scott", LastName = "Sickles", LastUpdated = DateTime.Now });
            _identities.Add(new Identity() { Id = new Guid("9089e195-7616-45be-b60f-0c5da973cf9f"), FirstName = "Brad", LastName = "Sickles", LastUpdated = DateTime.Now });
            _identities.Add(new Identity() { Id = new Guid("6d87ce59-f95d-4ef6-8c3a-71e6332e458b"), FirstName = "Dale", LastName = "Sickles", LastUpdated = DateTime.Now });
            _identities.Add(new Identity() { Id = new Guid("e62db83f-0f6c-44c8-9a12-76a97d7ad356"), FirstName = "Carol", LastName = "Sickles", LastUpdated = DateTime.Now });
        }

        public List<Identity> GetIdentities(Request Request)
        {
            List<Identity> identities = _identities.Where(delegate(Identity Iden) { return Iden.LastUpdated > Request.LastRequest; }).ToList();
            return identities;
        }

        public void UpdateIdentity(Identity Identity)
        {
            DeleteIdentity(Identity);
            Identity.LastUpdated = DateTime.Now;
            _identities.Add(Identity);
        }

        public void DeleteIdentity(Identity Identity)
        {
            _identities.Remove(Identity);
        }
    }
}
