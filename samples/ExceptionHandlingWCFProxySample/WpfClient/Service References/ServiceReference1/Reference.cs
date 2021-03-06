﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.3053
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TodoList.WpfClient.ServiceReference1 {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "3.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="TodoItem", Namespace="http://wcfclientguidance.codeplex.com/2009/04/schemas")]
    [System.SerializableAttribute()]
    public partial class TodoItem : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string IDField;
        
        private string TitleField;
        
        private string DescriptionField;
        
        private TodoList.WpfClient.ServiceReference1.PriorityFlag PriorityField;
        
        private TodoList.WpfClient.ServiceReference1.StatusFlag StatusField;
        
        private System.Nullable<System.DateTime> CreationDateField;
        
        private System.Nullable<System.DateTime> DueDateField;
        
        private System.Nullable<System.DateTime> CompletionDateField;
        
        private double PercentCompleteField;
        
        private string TagsField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ID {
            get {
                return this.IDField;
            }
            set {
                if ((object.ReferenceEquals(this.IDField, value) != true)) {
                    this.IDField = value;
                    this.RaisePropertyChanged("ID");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(IsRequired=true)]
        public string Title {
            get {
                return this.TitleField;
            }
            set {
                if ((object.ReferenceEquals(this.TitleField, value) != true)) {
                    this.TitleField = value;
                    this.RaisePropertyChanged("Title");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(IsRequired=true, Order=2)]
        public string Description {
            get {
                return this.DescriptionField;
            }
            set {
                if ((object.ReferenceEquals(this.DescriptionField, value) != true)) {
                    this.DescriptionField = value;
                    this.RaisePropertyChanged("Description");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(IsRequired=true, Order=3)]
        public TodoList.WpfClient.ServiceReference1.PriorityFlag Priority {
            get {
                return this.PriorityField;
            }
            set {
                if ((this.PriorityField.Equals(value) != true)) {
                    this.PriorityField = value;
                    this.RaisePropertyChanged("Priority");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(IsRequired=true, Order=4)]
        public TodoList.WpfClient.ServiceReference1.StatusFlag Status {
            get {
                return this.StatusField;
            }
            set {
                if ((this.StatusField.Equals(value) != true)) {
                    this.StatusField = value;
                    this.RaisePropertyChanged("Status");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(IsRequired=true, Order=5)]
        public System.Nullable<System.DateTime> CreationDate {
            get {
                return this.CreationDateField;
            }
            set {
                if ((this.CreationDateField.Equals(value) != true)) {
                    this.CreationDateField = value;
                    this.RaisePropertyChanged("CreationDate");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(IsRequired=true, Order=6)]
        public System.Nullable<System.DateTime> DueDate {
            get {
                return this.DueDateField;
            }
            set {
                if ((this.DueDateField.Equals(value) != true)) {
                    this.DueDateField = value;
                    this.RaisePropertyChanged("DueDate");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(IsRequired=true, Order=7)]
        public System.Nullable<System.DateTime> CompletionDate {
            get {
                return this.CompletionDateField;
            }
            set {
                if ((this.CompletionDateField.Equals(value) != true)) {
                    this.CompletionDateField = value;
                    this.RaisePropertyChanged("CompletionDate");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(IsRequired=true, Order=8)]
        public double PercentComplete {
            get {
                return this.PercentCompleteField;
            }
            set {
                if ((this.PercentCompleteField.Equals(value) != true)) {
                    this.PercentCompleteField = value;
                    this.RaisePropertyChanged("PercentComplete");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(IsRequired=true, Order=9)]
        public string Tags {
            get {
                return this.TagsField;
            }
            set {
                if ((object.ReferenceEquals(this.TagsField, value) != true)) {
                    this.TagsField = value;
                    this.RaisePropertyChanged("Tags");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "3.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="PriorityFlag", Namespace="http://schemas.datacontract.org/2004/07/Entities")]
    public enum PriorityFlag : int {
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Low = 0,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Normal = 1,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        High = 2,
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "3.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="StatusFlag", Namespace="http://schemas.datacontract.org/2004/07/Entities")]
    public enum StatusFlag : int {
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        NotStarted = 0,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        InProgress = 1,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Completed = 2,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        WaitingOnSomeoneElse = 3,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Deferred = 4,
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://wcfclientguidance.codeplex.com/2009/04", ConfigurationName="ServiceReference1.ITodoListService")]
    public interface ITodoListService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://wcfclientguidance.codeplex.com/2009/04/ITodoListService/GetItems", ReplyAction="http://wcfclientguidance.codeplex.com/2009/04/ITodoListService/GetItemsResponse")]
        System.Collections.ObjectModel.ObservableCollection<TodoList.WpfClient.ServiceReference1.TodoItem> GetItems();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://wcfclientguidance.codeplex.com/2009/04/ITodoListService/CreateItem", ReplyAction="http://wcfclientguidance.codeplex.com/2009/04/ITodoListService/CreateItemResponse" +
            "")]
        string CreateItem(TodoList.WpfClient.ServiceReference1.TodoItem item);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://wcfclientguidance.codeplex.com/2009/04/ITodoListService/UpdateItem", ReplyAction="http://wcfclientguidance.codeplex.com/2009/04/ITodoListService/UpdateItemResponse" +
            "")]
        void UpdateItem(TodoList.WpfClient.ServiceReference1.TodoItem item);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://wcfclientguidance.codeplex.com/2009/04/ITodoListService/DeleteItem", ReplyAction="http://wcfclientguidance.codeplex.com/2009/04/ITodoListService/DeleteItemResponse" +
            "")]
        void DeleteItem(string id);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    public interface ITodoListServiceChannel : TodoList.WpfClient.ServiceReference1.ITodoListService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    public partial class TodoListServiceClient : System.ServiceModel.ClientBase<TodoList.WpfClient.ServiceReference1.ITodoListService>, TodoList.WpfClient.ServiceReference1.ITodoListService {
        
        public TodoListServiceClient() {
        }
        
        public TodoListServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public TodoListServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public TodoListServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public TodoListServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public System.Collections.ObjectModel.ObservableCollection<TodoList.WpfClient.ServiceReference1.TodoItem> GetItems() {
            return base.Channel.GetItems();
        }
        
        public string CreateItem(TodoList.WpfClient.ServiceReference1.TodoItem item) {
            return base.Channel.CreateItem(item);
        }
        
        public void UpdateItem(TodoList.WpfClient.ServiceReference1.TodoItem item) {
            base.Channel.UpdateItem(item);
        }
        
        public void DeleteItem(string id) {
            base.Channel.DeleteItem(id);
        }
    }
    
    public class TodoListServiceProxy : ExceptionHandlingProxyBase<ITodoListService>, ITodoListService {
        
        public TodoListServiceProxy() : 
                this("") {
        }
        
        public TodoListServiceProxy(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public TodoListServiceProxy(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public TodoListServiceProxy(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public System.Collections.ObjectModel.ObservableCollection<TodoList.WpfClient.ServiceReference1.TodoItem> GetItems() {
            return base.Invoke<System.Collections.ObjectModel.ObservableCollection<TodoList.WpfClient.ServiceReference1.TodoItem>>("GetItems");
        }
        
        public string CreateItem(TodoList.WpfClient.ServiceReference1.TodoItem item) {
            return base.Invoke<string>("CreateItem", item);
        }
        
        public void UpdateItem(TodoList.WpfClient.ServiceReference1.TodoItem item) {
            base.Invoke("UpdateItem", item);
        }
        
        public void DeleteItem(string id) {
            base.Invoke("DeleteItem", id);
        }
    }
}
