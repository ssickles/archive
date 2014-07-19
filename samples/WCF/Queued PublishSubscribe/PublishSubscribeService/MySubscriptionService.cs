//2006 IDesign Inc. 
//Questions? Comments? go to 
//http://www.idesign.net

using System;
using ServiceModelEx;
using System.ServiceModel;

[ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)] 
class MySubscriptionService : SubscriptionManager<IMyEvents>,IPersistentSubscriptionService
{}