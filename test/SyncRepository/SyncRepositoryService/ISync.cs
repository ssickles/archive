using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using SyncRepositoryDomainModel;

namespace SyncRepositoryService
{
    [ServiceContract]
    public interface ISync
    {
        [OperationContract]
        List<Identity> GetIdentities(Request Request);

        [OperationContract]
        void UpdateIdentity(Identity Identity);

        [OperationContract]
        void DeleteIdentity(Identity Identity);
    }
}
