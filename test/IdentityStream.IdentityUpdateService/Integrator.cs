using System;
using System.Data.Common;
using System.Diagnostics;
using System.Reflection;
using System.ServiceModel;

namespace IdentityStream.Services.IntegrationServices
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public class Integrator : IIntegrator
    {
        #region IIntegrator Members

        /// <summary>
        /// Method for clients to use (right now specifically created for OI) to
        /// take a source Id that has been set and replace that source Id with
        /// the Id of their choosing. Helpful when Ids change overnight during
        /// a synchronization process.
        /// </summary>
        /// <param name="oldId">The current source Id to be replaced.</param>
        /// <param name="replacementId">The new Id to replace the current source Id.</param>
        public void ReplaceIdentitySourceId(string oldId, string replacementId)
        {
            try
            {
                if (oldId.Equals(replacementId))
                    throw new ArgumentException("The oldId can not be the same as the replacementId.");
            }
            catch (Exception ex)
            {
                throw IdentityStreamFormattedFaultException(ex);
            }
        }

        #endregion

        /// <summary>
        /// Logs the error and then extracts information from it to be inserted into a
        /// FaultException that is safe for throwing back to the client application with
        /// a few tidbits of useful information if the exception is logged on the client side.
        /// </summary>
        /// <param name="ex">The exception to log and extract information from.</param>
        /// <returns>A FaultException that contains just enough information for a client to see.</returns>
        private static FaultException<ErrorDetails> IdentityStreamFormattedFaultException(Exception ex)
        {
            MethodBase methodBase = new StackTrace().GetFrame(1).GetMethod();
            string methodName = string.Format("{0} - {1}", methodBase.DeclaringType.FullName, methodBase.Name);
            ServiceException se;
            if (ex is DbException)
            {
                if (ex.TargetSite.Name.ToLower() == "open")
                {
                    se = ServiceException.DatabaseOffline;
                }
                else
                {
                    se = ServiceException.DatabaseProcessingError;
                }
            }
            else
            {
                se = ServiceException.Application;
            }
            return new FaultException<ErrorDetails>(new ErrorDetails(methodName, DateTime.Now.ToString(), se));
        }
    }
}