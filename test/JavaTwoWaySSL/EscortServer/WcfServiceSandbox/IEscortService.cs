using System.ServiceModel;

namespace WcfServiceSandbox
{
    [ServiceContract]
    public interface IEscortService
    {
        [OperationContract]
        [FaultContract(typeof(ErrorDetails))]
        bool Escort(int quantity);
    }
}