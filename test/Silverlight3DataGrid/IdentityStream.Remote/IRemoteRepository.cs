using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace IdentityStream.Data
{
    [ServiceContract]
    public interface IRemoteRepository<T, Key>
    {
        [System.Runtime.Serialization.operOperationContract]
        T GetById(Key Id);
        [OperationContract]
        IList<T> Get(QueryObject Query);
        [OperationContract]
        void Insert(T Entity);
        [OperationContract]
        void Delete(T Entity);
    }
}
