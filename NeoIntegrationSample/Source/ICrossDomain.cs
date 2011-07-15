using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.IO;
using System.ServiceModel.Web;

namespace NeoIntegrationSample
{
    [ServiceContract]
    public interface ICrossDomain
    {
        [OperationContract]
        [WebGet(UriTemplate = "/crossdomain.xml")]
        Stream GetPolicy();
    }
}
