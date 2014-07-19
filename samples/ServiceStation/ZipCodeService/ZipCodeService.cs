using System;
using System.Collections.Generic;
using System.Text;
using Extensions;

namespace ZipCodeServiceLibrary
{
    //[NoBasicEndpointValidator]
    //[ConsoleMessageTracing]
    //[XsdValidation]
    public class ZipCodeService : IZipCodeService
    {
        #region IZipCodeService Members

        public string Lookup(string zipcode)
        {
            Console.WriteLine("Lookup method invoked with value '{0}'", zipcode);

            switch (zipcode)
            {
                case "84041-2941": return "Layton, UT";
                case "97206-6825": return "Portland, OR";
                case "85383-8718": return "Peoria, AZ";
                case "83221-5665": return "Blackfoot, ID";
                case "68112-2238": return "Omaha, NE";
                default: return "unknown zip code";
            }
        }

        #endregion
    }
}
