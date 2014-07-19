//2006 IDesign Inc.  
//Questions? Comments? go to 
//http://www.idesign.net

using System;
using System.ServiceModel;
using System.ServiceModel.Channels;


[ServiceContract(CallbackContract=typeof(IMyContractCallback))]
public interface IMyContract
{
   [OperationContract]
   void DoSomething();
}


public interface IMyContractCallback
{
   [OperationContract]
   void OnCallback();
}


public partial class MyContractClient : DuplexClientBase<IMyContract>,IMyContract
{
   public MyContractClient(InstanceContext callbackInstance) : base(callbackInstance)
   {}

   public MyContractClient(InstanceContext callbackInstance,string endpointConfigurationName) : base(callbackInstance,endpointConfigurationName)
   {}

   public void DoSomething()
   {
      Channel.DoSomething();
   }
}
