using System.Runtime.Serialization;
using System.ServiceModel;

namespace IdentityStream.Services.IntegrationServices
{
    [ServiceContract]
    public interface IIntegrator
    {
        [OperationContract]
        [FaultContract(typeof(ErrorDetails))]
        void ReplaceIdentitySourceId(string oldId, string replacementId);
    }

    [DataContract(Name = "ErrorDetails")]
    public class ErrorDetails
    {
        public ErrorDetails(string MethodName, string DateTime, ServiceException ExceptionType)
        {
            this.MethodName = MethodName;
            this.DateTime = DateTime;
            this.ExceptionType = ExceptionType;
        }

        [DataMember(Name = "MethodName")]
        public string MethodName;

        [DataMember(Name = "DateTime")]
        public string DateTime;

        [DataMember(Name = "ExceptionType")]
        public ServiceException ExceptionType;
    }

    public enum ServiceException
    {
        DatabaseOffline,
        DatabaseProcessingError,
        Application
    }
}