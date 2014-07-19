//2006 IDesign Inc. 
//Questions? Comments? go to 
//http://www.idesign.net

using System;
using System.ServiceModel;
using System.ServiceModel.Channels;

[ServiceContract]
public interface IMyEvents
{
   [OperationContract(IsOneWay = true)]
   void OnEvent1();

   [OperationContract(IsOneWay = true)]
   void OnEvent2(int number);

   [OperationContract(IsOneWay = true)]
   void OnEvent3(int number,string text);
}

partial class MyEventsProxy : ClientBase<IMyEvents>,IMyEvents
{
   public MyEventsProxy()
   {}

   public MyEventsProxy(string endpointConfigurationName) : base(endpointConfigurationName)
   {}

   public MyEventsProxy(string endpointConfigurationName,string remoteAddress) : base(endpointConfigurationName,remoteAddress)
   {}

   public MyEventsProxy(string endpointConfigurationName,EndpointAddress remoteAddress) : base(endpointConfigurationName,remoteAddress)
   {}

   public MyEventsProxy(Binding binding,EndpointAddress remoteAddress) : base(binding,remoteAddress)
   {}

   public void OnEvent1()
   {
      Channel.OnEvent1();
   }

   public void OnEvent2(int number)
   {
      Channel.OnEvent2(number);
   }

   public void OnEvent3(int number,string text)
   {
      Channel.OnEvent3(number,text);
   }
}
