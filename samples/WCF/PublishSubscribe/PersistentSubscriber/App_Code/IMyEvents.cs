//2006 IDesign Inc. 
//Questions? Comments? go to 
//http://www.idesign.net

using System;
using System.ServiceModel;


[ServiceContract]
interface IMyEvents
{
   [OperationContract(IsOneWay = true)]
   void OnEvent1();

   [OperationContract(IsOneWay = true)]
   void OnEvent2(int number);

   [OperationContract(IsOneWay = true)]
   void OnEvent3(int number,string text);
}