using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.ServiceModel.Web;

namespace NeoIntegrationSample
{
    public class CrossDomainService: ICrossDomain
    {
        #region ICrossDomain Members

        public Stream GetPolicy()
        {
            string policy =
            @"<?xml version=""1.0""?>
              <!DOCTYPE cross-domain-policy SYSTEM ""http://www.macromedia.com/xml/dtds/cross-domain-policy.dtd"">
                <cross-domain-policy> 
	                <site-control permitted-cross-domain-policies=""master-only""/>
                    <allow-access-from domain=""*"" to-ports=""*"" />
                    <allow-http-request-headers-from domain=""*"" headers=""*"" />
                </cross-domain-policy>";
            WebOperationContext.Current.OutgoingResponse.ContentType = "application/xml";
            return new MemoryStream(Encoding.UTF8.GetBytes(policy));
        }

        #endregion
    }
}
