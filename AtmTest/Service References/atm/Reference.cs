﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.225
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AtmTest.atm {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="atm.IAtmIntegrationService")]
    public interface IAtmIntegrationService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IAtmIntegrationService/verifyCustomer", ReplyAction="http://tempuri.org/IAtmIntegrationService/verifyCustomerResponse")]
        int verifyCustomer(string customerIdentifier, byte[] template);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IAtmIntegrationServiceChannel : AtmTest.atm.IAtmIntegrationService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class AtmIntegrationServiceClient : System.ServiceModel.ClientBase<AtmTest.atm.IAtmIntegrationService>, AtmTest.atm.IAtmIntegrationService {
        
        public AtmIntegrationServiceClient() {
        }
        
        public AtmIntegrationServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public AtmIntegrationServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public AtmIntegrationServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public AtmIntegrationServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public int verifyCustomer(string customerIdentifier, byte[] template) {
            return base.Channel.verifyCustomer(customerIdentifier, template);
        }
    }
}
