using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Security.Permissions;

namespace WcfRoleBasedSecurity
{
    [ServiceContract]
    public interface IFantasyService
    {
        [OperationContract]
        IList<string> GetLeagueNames();
        [OperationContract]
        int GetTeamWins(int teamId);
    }
}
