using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using IdentityStream.Data;
using IdentityStream.DataModels;

namespace IdentityService
{
    public class IdentityService : IIdentityService
    {
        private static List<Identity> _identities;

        static IdentityService()
        {
            _identities = new List<Identity>();
            _identities.Add(new Identity()
            {
                Id = 1,
                FirstName = "Scott",
                LastName = "Sickles"
            });
            _identities.Add(new Identity()
            {
                Id = 2,
                FirstName = "Brad",
                LastName = "Sickles"
            });
            _identities.Add(new Identity()
            {
                Id = 3,
                FirstName = "Charlotte",
                LastName = "Sickles"
            });
            _identities.Add(new Identity()
            {
                Id = 4,
                FirstName = "Connor",
                LastName = "Sickles"
            });
            _identities.Add(new Identity()
            {
                Id = 5,
                FirstName = "Dale",
                LastName = "Sickles"
            });
            _identities.Add(new Identity()
            {
                Id = 6,
                FirstName = "Carol",
                LastName = "Sickles"
            });
            _identities.Add(new Identity()
            {
                Id = 7,
                FirstName = "Kristen",
                LastName = "Lewis"
            });
            _identities.Add(new Identity()
            {
                Id = 8,
                FirstName = "David",
                LastName = "Lewis"
            });
            _identities.Add(new Identity()
            {
                Id = 9,
                FirstName = "Tracy",
                LastName = "Holtzer"
            });
            _identities.Add(new Identity()
            {
                Id = 10,
                FirstName = "Ryan",
                LastName = "Holtzer"
            });
            _identities.Add(new Identity()
            {
                Id = 11,
                FirstName = "Kristen",
                LastName = "Hoyt"
            });
        }

        #region IRemoteRepository<Identity> Members

        public Identity GetById(int id)
        {
            return (from i in _identities
                    where i.Id == id
                    select i).SingleOrDefault();
        }

        public IList<Identity> Get(QueryObject Query)
        {
            return _identities;
        }

        public void Insert(Identity Entity)
        {
            _identities.Add(Entity);
        }

        public void Delete(Identity Entity)
        {
            _identities.Remove(Entity);
        }

        #endregion
    }
}
