//2006 IDesign Inc.  
//Questions? Comments? go to 
//http://www.idesign.net

using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Diagnostics;
using System.Windows.Forms;

[ServiceContract(CallbackContract = typeof(IMyContractCallback))] 
interface IMyContract
{
   [OperationContract] 
   void DoSomething();
}
interface IMyContractCallback
{
   [OperationContract]
   void OnCallback();
}

[ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Reentrant)]
class MyService : IMyContract
{
   public void DoSomething()
   {
      Trace.WriteLine("DoSomething");
      IMyContractCallback callback = OperationContext.Current.GetCallbackChannel<IMyContractCallback>();
      callback.OnCallback();
   }
}