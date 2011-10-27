using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ConsoleClient.FantasyService;
using System.ServiceModel;

namespace ConsoleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ChannelFactory<IFantasyService>("*");
            factory.Credentials.UserName.UserName = "scott";
            factory.Credentials.UserName.Password = "sas0927";
            var proxy = factory.CreateChannel();
            IList<string> leagueNames = proxy.GetLeagueNames();
            Console.WriteLine(string.Format("{0} league names returned.", leagueNames.Count));
            int teamWins = proxy.GetTeamWins(1);
            Console.WriteLine(string.Format("{0} team wins for team 1.", teamWins));
            Console.ReadLine();
        }
    }
}
