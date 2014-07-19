using System;
using System.Collections.Generic;
using System.Security.Permissions;
using System.Threading;
using System.Security.Principal;

namespace WcfRoleBasedSecurity
{
    public class FantasyService : IFantasyService
    {
        public IList<string> GetLeagueNames()
        {
            return new List<string> { "This League Is Real", "St. Louis H2H" };
        }

        //[PrincipalPermission(SecurityAction.Demand, Role = "administrators")]
        public int GetTeamWins(int teamId)
        {
            IPrincipal i = Thread.CurrentPrincipal;
            return 5;
        }
    }
}
