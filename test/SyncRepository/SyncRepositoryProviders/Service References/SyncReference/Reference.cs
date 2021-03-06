﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.3082
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SyncRepositoryProviders.SyncReference {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="SyncReference.ISync")]
    public interface ISync {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISync/GetIdentities", ReplyAction="http://tempuri.org/ISync/GetIdentitiesResponse")]
        System.Collections.Generic.List<SyncRepositoryDomainModel.Identity> GetIdentities(SyncRepositoryDomainModel.Request Request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISync/UpdateIdentity", ReplyAction="http://tempuri.org/ISync/UpdateIdentityResponse")]
        void UpdateIdentity(SyncRepositoryDomainModel.Identity Identity);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISync/DeleteIdentity", ReplyAction="http://tempuri.org/ISync/DeleteIdentityResponse")]
        void DeleteIdentity(SyncRepositoryDomainModel.Identity Identity);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    public interface ISyncChannel : SyncRepositoryProviders.SyncReference.ISync, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    public partial class SyncClient : System.ServiceModel.ClientBase<SyncRepositoryProviders.SyncReference.ISync>, SyncRepositoryProviders.SyncReference.ISync {
        
        public SyncClient() {
        }
        
        public SyncClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public SyncClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public SyncClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public SyncClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public System.Collections.Generic.List<SyncRepositoryDomainModel.Identity> GetIdentities(SyncRepositoryDomainModel.Request Request) {
            return base.Channel.GetIdentities(Request);
        }
        
        public void UpdateIdentity(SyncRepositoryDomainModel.Identity Identity) {
            base.Channel.UpdateIdentity(Identity);
        }
        
        public void DeleteIdentity(SyncRepositoryDomainModel.Identity Identity) {
            base.Channel.DeleteIdentity(Identity);
        }
    }
}
