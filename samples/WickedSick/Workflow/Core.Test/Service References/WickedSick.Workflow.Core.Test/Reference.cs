﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.1433
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WickedSick.Workflow.Core.Test {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://WickedSick.Workflow.Core", ConfigurationName="WickedSick.Workflow.Core.Test.IWorkflowHost")]
    public interface IWorkflowHost {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://WickedSick.Workflow.Core/IWorkflowHost/StartWorkflow", ReplyAction="http://WickedSick.Workflow.Core/IWorkflowHost/StartWorkflowResponse")]
        void StartWorkflow(string ApplicationName, string WorkflowName, System.Collections.Generic.Dictionary<string, object> Parameters);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    public interface IWorkflowHostChannel : WickedSick.Workflow.Core.Test.IWorkflowHost, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    public partial class WorkflowHostClient : System.ServiceModel.ClientBase<WickedSick.Workflow.Core.Test.IWorkflowHost>, WickedSick.Workflow.Core.Test.IWorkflowHost {
        
        public WorkflowHostClient() {
        }
        
        public WorkflowHostClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public WorkflowHostClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public WorkflowHostClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public WorkflowHostClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public void StartWorkflow(string ApplicationName, string WorkflowName, System.Collections.Generic.Dictionary<string, object> Parameters) {
            base.Channel.StartWorkflow(ApplicationName, WorkflowName, Parameters);
        }
    }
}