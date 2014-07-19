// © 2005-2006 Michele Leroux Bustamante. All rights reserved 
// Blogs: www.thatindigogirl.com, www.dasblonde.net
// IDesign: www.idesign.net
using System;
using System.ServiceModel;
using System.Security.Permissions;
using System.Security.Principal;

namespace HelloIndigo
{


    [ServiceContract(Namespace="http://www.thatindigogirl.com/2006/06/Samples")]
    public interface IHelloIndigoService
    {
        [OperationContract]
        string HelloIndigo(string inputString);
    }

    public class HelloIndigoService : IHelloIndigoService
    {
        #region IHelloIndigoService Members

        [PrincipalPermission(SecurityAction.Demand, Role="Users")]
        public string HelloIndigo(string inputString)
        {

            string s = String.Format("{0} \r\n Request at {1} with credentials for {2}.\r\n  Service running with identity {3}.", inputString, DateTime.Now, ServiceSecurityContext.Current.PrimaryIdentity.Name, WindowsIdentity.GetCurrent().Name);

            return s;
        }

        #endregion
    }

}

