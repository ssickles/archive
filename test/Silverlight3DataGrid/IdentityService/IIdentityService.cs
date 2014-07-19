using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IdentityStream.Data;
using System.ServiceModel;
using IdentityStream.DataModels;

namespace IdentityService
{
    [ServiceContract]
    public interface IIdentityService: IRemoteRepository<Identity, int>
    {
    }
}
