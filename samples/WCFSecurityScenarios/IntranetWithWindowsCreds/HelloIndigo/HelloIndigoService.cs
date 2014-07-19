// © 2005-2006 Michele Leroux Bustamante. All rights reserved 
// Blogs: www.thatindigogirl.com, www.dasblonde.net
// IDesign: www.idesign.net
using System;
using System.ServiceModel;
using System.Security.Principal;
using System.Security.Permissions;

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

        //[OperationBehavior(Impersonation=ImpersonationOption.NotAllowed)]
        [PrincipalPermission(SecurityAction.Demand, Role="BUILTIN\\Users")]
        public string HelloIndigo(string inputString)
        {
            Console.WriteLine();
            Console.WriteLine("Request at {0} with credentials for {1}.", DateTime.Now, ServiceSecurityContext.Current.PrimaryIdentity.Name);
            Console.WriteLine("Service running with identity {0}", WindowsIdentity.GetCurrent().Name);
            Console.WriteLine();

            return inputString;
        }

        #endregion
    }

}

