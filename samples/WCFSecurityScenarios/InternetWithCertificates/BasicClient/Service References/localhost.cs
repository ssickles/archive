﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.42
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PartnerClient.localhost
{
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://www.thatindigogirl.com/2006/06/Samples", ConfigurationName="PartnerClient.localhost.IHelloIndigoService")]
    public interface IHelloIndigoService
    {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://www.thatindigogirl.com/2006/06/Samples/IHelloIndigoService/HelloIndigo", ReplyAction="http://www.thatindigogirl.com/2006/06/Samples/IHelloIndigoService/HelloIndigoResp" +
            "onse")]
        string HelloIndigo(string inputString);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    public interface IHelloIndigoServiceChannel : PartnerClient.localhost.IHelloIndigoService, System.ServiceModel.IClientChannel
    {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    public partial class HelloIndigoServiceClient : System.ServiceModel.ClientBase<PartnerClient.localhost.IHelloIndigoService>, PartnerClient.localhost.IHelloIndigoService
    {
        
        public HelloIndigoServiceClient()
        {
        }
        
        public HelloIndigoServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName)
        {
        }
        
        public HelloIndigoServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress)
        {
        }
        
        public HelloIndigoServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress)
        {
        }
        
        public HelloIndigoServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress)
        {
        }
        
        public string HelloIndigo(string inputString)
        {
            return base.Channel.HelloIndigo(inputString);
        }
    }
}