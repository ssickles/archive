using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ConsoleClient.FantasyService;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Net;

namespace ConsoleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ChannelFactory<IFantasyService>("*");
            factory.Credentials.UserName.UserName = "scott";
            factory.Credentials.UserName.Password = "sas0927";
            factory.Endpoint.Behaviors.Add(new CustomHeaderEndpointBehavior());
            var proxy = factory.CreateChannel();
            MessageHeader header = MessageHeader.CreateHeader("userId", "http://www.identitystream.com", "Custom Header");

            using (OperationContextScope scope = new OperationContextScope((IContextChannel)proxy))
            {
                OperationContext.Current.OutgoingMessageHeaders.Add(header);

                //HttpRequestMessageProperty httpRequestProperty = new HttpRequestMessageProperty();
                //httpRequestProperty.Headers.Add("myCustomHeader", "Custom Happy Value.");
                //httpRequestProperty.Headers.Add(HttpRequestHeader.UserAgent, "my user agent");
                //OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = httpRequestProperty;

            }
            IList<string> leagueNames = proxy.GetLeagueNames();
            Console.WriteLine(string.Format("{0} league names returned.", leagueNames.Count));
            int teamWins = proxy.GetTeamWins(1);
            Console.WriteLine(string.Format("{0} team wins for team 1.", teamWins));
            Console.ReadLine();
        }
    }
}
