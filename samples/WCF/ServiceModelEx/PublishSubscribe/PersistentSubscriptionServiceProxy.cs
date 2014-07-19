//2006 IDesign Inc. 
//Questions? Comments? go to 
//http://www.idesign.net

using System;
using System.ServiceModel;
using System.Runtime.Serialization;
using System.ServiceModel.Channels;

namespace ServiceModelEx
{
   public partial class PersistentSubscriptionServiceClient : ClientBase<IPersistentSubscriptionService>,IPersistentSubscriptionService
   {
      public PersistentSubscriptionServiceClient()
      {}

      public PersistentSubscriptionServiceClient(string endpointConfigurationName) : base(endpointConfigurationName)
      {}

      public PersistentSubscriptionServiceClient(string endpointConfigurationName,string remoteAddress) : base(endpointConfigurationName,remoteAddress)
      {}

      public PersistentSubscriptionServiceClient(string endpointConfigurationName,EndpointAddress remoteAddress) : base(endpointConfigurationName,remoteAddress)
      {}

      public PersistentSubscriptionServiceClient(Binding binding,EndpointAddress remoteAddress) : base(binding,remoteAddress)
      {}

      public void PersistSubscribe(string address,string eventsContract,string eventOperation)
      {
         Channel.PersistSubscribe(address,eventsContract,eventOperation);
      }

      public void PersistUnsubscribe(string address,string eventsContract,string eventOperation)
      {
         Channel.PersistUnsubscribe(address,eventsContract,eventOperation);
      }

      public PersistentSubscription[] GetAllSubscribers()
      {
         return Channel.GetAllSubscribers();
      }

      public PersistentSubscription[] GetSubscribersToContract(string eventsContract)
      {
         return Channel.GetSubscribersToContract(eventsContract);
      }

      public string[] GetSubscribersToContractEventType(string eventsContract,string eventOperation)
      {
         return Channel.GetSubscribersToContractEventType(eventsContract,eventOperation);
      }

      public PersistentSubscription[] GetAllSubscribersFromAddress(string address)
      {
         return Channel.GetAllSubscribersFromAddress(address);
      }
   }
}
