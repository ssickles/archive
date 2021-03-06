﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.3053
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MySQLDataLoading.ServiceReference1 {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "3.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="SortDirection", Namespace="http://schemas.datacontract.org/2004/07/IdentityStream.Services.BiometricServices" +
        "")]
    public enum SortDirection : int {
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Ascending = 0,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Descending = 1,
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "3.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="IdentityData", Namespace="http://schemas.datacontract.org/2004/07/IdentityStream.Services.BiometricServices" +
        "")]
    [System.SerializableAttribute()]
    public partial class IdentityData : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int IdField;
        
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
        public int Id {
            get {
                return this.IdField;
            }
            set {
                if ((this.IdField.Equals(value) != true)) {
                    this.IdField = value;
                    this.RaisePropertyChanged("Id");
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
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="ServiceReference1.IBiometricService")]
    public interface IBiometricService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IBiometricService/UpdateIdentity", ReplyAction="http://tempuri.org/IBiometricService/UpdateIdentityResponse")]
        void UpdateIdentity(string FirstName, string LastName, string CountryCode, int Active, int Id, int BioEnabled, string IdentityCode);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IBiometricService/RemoveIdentity", ReplyAction="http://tempuri.org/IBiometricService/RemoveIdentityResponse")]
        void RemoveIdentity(int IdentityId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IBiometricService/AddLogin", ReplyAction="http://tempuri.org/IBiometricService/AddLoginResponse")]
        int AddLogin(string Login, string Password, string ApplicationCode, int IdentityId, string SystemLoginId, string SystemLoginPassword, string RoleCode, int FirstLogon, int UseGeneratedPass, string OrigPassword);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IBiometricService/UpdateLogin", ReplyAction="http://tempuri.org/IBiometricService/UpdateLoginResponse")]
        void UpdateLogin(int Id, string Login, string Password, string ApplicationCode, int IdentityId, string SystemLoginId, string SystemLoginPassword, string RoleCode, int FirstLogon, int UseGeneratedPass, string OrigPassword);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IBiometricService/RemoveLogins", ReplyAction="http://tempuri.org/IBiometricService/RemoveLoginsResponse")]
        void RemoveLogins(int IdentityId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IBiometricService/UpdateTransaction", ReplyAction="http://tempuri.org/IBiometricService/UpdateTransactionResponse")]
        void UpdateTransaction(int TransactionId, string Description, int AuthenticationLevelId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IBiometricService/AddUpdateAuthenticationEnrollment", ReplyAction="http://tempuri.org/IBiometricService/AddUpdateAuthenticationEnrollmentResponse")]
        int AddUpdateAuthenticationEnrollment(int IdentityId, int AdministratorId, System.Collections.Generic.Dictionary<string, byte[]> Templates, System.Collections.Generic.Dictionary<string, int> Score);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IBiometricService/RemoveAuthenticationEnrollment", ReplyAction="http://tempuri.org/IBiometricService/RemoveAuthenticationEnrollmentResponse")]
        void RemoveAuthenticationEnrollment(int EnrollmentId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IBiometricService/AddAuthenticationTemplate", ReplyAction="http://tempuri.org/IBiometricService/AddAuthenticationTemplateResponse")]
        int AddAuthenticationTemplate(int EnrollmentId, string BiometricUnitCode, byte[] Template);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IBiometricService/AddAuthenticationTemplateScores", ReplyAction="http://tempuri.org/IBiometricService/AddAuthenticationTemplateScoresResponse")]
        int AddAuthenticationTemplateScores(int EnrollmentId, string BiometricUnitCode, byte[] Template, int Score);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IBiometricService/AddAuthenticationLevel", ReplyAction="http://tempuri.org/IBiometricService/AddAuthenticationLevelResponse")]
        int AddAuthenticationLevel(string Description);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IBiometricService/AddAuthenticationUnit", ReplyAction="http://tempuri.org/IBiometricService/AddAuthenticationUnitResponse")]
        int AddAuthenticationUnit(string code, string AuthenticationType, string UnitDescription, int EnrollmentSequence, int AuthenticationLevelId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IBiometricService/RemoveAuthenticationLevel", ReplyAction="http://tempuri.org/IBiometricService/RemoveAuthenticationLevelResponse")]
        int RemoveAuthenticationLevel(int Id);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IBiometricService/RemoveAuthenticationUnit", ReplyAction="http://tempuri.org/IBiometricService/RemoveAuthenticationUnitResponse")]
        int RemoveAuthenticationUnit(string UnitCode);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IBiometricService/UpdateAuthenticationLevel", ReplyAction="http://tempuri.org/IBiometricService/UpdateAuthenticationLevelResponse")]
        int UpdateAuthenticationLevel(int Id, string Description);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IBiometricService/UpdateAuthenticationUnit", ReplyAction="http://tempuri.org/IBiometricService/UpdateAuthenticationUnitResponse")]
        int UpdateAuthenticationUnit(string Description, int EnrollmentSequence, string AuthenticationType, string Code, int Id);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IBiometricService/AddAuthenticationLevelUnit", ReplyAction="http://tempuri.org/IBiometricService/AddAuthenticationLevelUnitResponse")]
        void AddAuthenticationLevelUnit(int Id, string code);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IBiometricService/RemoveAuthenticationLevelUnit", ReplyAction="http://tempuri.org/IBiometricService/RemoveAuthenticationLevelUnitResponse")]
        void RemoveAuthenticationLevelUnit(int Id);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IBiometricService/ImportT24Users", ReplyAction="http://tempuri.org/IBiometricService/ImportT24UsersResponse")]
        void ImportT24Users(System.Data.DataTable Data);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IBiometricService/ImportT24Customers", ReplyAction="http://tempuri.org/IBiometricService/ImportT24CustomersResponse")]
        void ImportT24Customers(System.Data.DataTable Data);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IBiometricService/ImportT24Companies", ReplyAction="http://tempuri.org/IBiometricService/ImportT24CompaniesResponse")]
        void ImportT24Companies(System.Data.DataTable Data);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IBiometricService/ImportT24AccountOfficers", ReplyAction="http://tempuri.org/IBiometricService/ImportT24AccountOfficersResponse")]
        void ImportT24AccountOfficers(System.Data.DataTable Data);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IBiometricService/ImportT24Languages", ReplyAction="http://tempuri.org/IBiometricService/ImportT24LanguagesResponse")]
        void ImportT24Languages(System.Data.DataTable Data);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IBiometricService/ProcessT24Imports", ReplyAction="http://tempuri.org/IBiometricService/ProcessT24ImportsResponse")]
        bool ProcessT24Imports();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IBiometricService/LogDebugMessage", ReplyAction="http://tempuri.org/IBiometricService/LogDebugMessageResponse")]
        void LogDebugMessage(string Message);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IBiometricService/LogInfoMessage", ReplyAction="http://tempuri.org/IBiometricService/LogInfoMessageResponse")]
        void LogInfoMessage(string Message);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IBiometricService/LogWarningMessage", ReplyAction="http://tempuri.org/IBiometricService/LogWarningMessageResponse")]
        void LogWarningMessage(string Message);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IBiometricService/LogErrorMessage", ReplyAction="http://tempuri.org/IBiometricService/LogErrorMessageResponse")]
        void LogErrorMessage(string Message);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IBiometricService/LogErrorMessageWithException", ReplyAction="http://tempuri.org/IBiometricService/LogErrorMessageWithExceptionResponse")]
        void LogErrorMessageWithException(string Message, System.Exception ex);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IBiometricService/GetRoles", ReplyAction="http://tempuri.org/IBiometricService/GetRolesResponse")]
        System.Data.DataSet GetRoles();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IBiometricService/GetApplications", ReplyAction="http://tempuri.org/IBiometricService/GetApplicationsResponse")]
        System.Data.DataSet GetApplications();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IBiometricService/GetApplicationByID", ReplyAction="http://tempuri.org/IBiometricService/GetApplicationByIDResponse")]
        System.Data.DataSet GetApplicationByID(int id);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IBiometricService/GetApplicationTypes", ReplyAction="http://tempuri.org/IBiometricService/GetApplicationTypesResponse")]
        System.Data.DataSet GetApplicationTypes();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IBiometricService/GetCountries", ReplyAction="http://tempuri.org/IBiometricService/GetCountriesResponse")]
        System.Data.DataSet GetCountries();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IBiometricService/GetCountryByID", ReplyAction="http://tempuri.org/IBiometricService/GetCountryByIDResponse")]
        System.Data.DataSet GetCountryByID(int id);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IBiometricService/GetReports", ReplyAction="http://tempuri.org/IBiometricService/GetReportsResponse")]
        System.Data.DataSet GetReports();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IBiometricService/GetReportByID", ReplyAction="http://tempuri.org/IBiometricService/GetReportByIDResponse")]
        System.Data.DataSet GetReportByID(int id);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IBiometricService/GetIdentities", ReplyAction="http://tempuri.org/IBiometricService/GetIdentitiesResponse")]
        MySQLDataLoading.ServiceReference1.IdentityData[] GetIdentities(out int TotalRecords, System.Collections.Generic.Dictionary<string, string> Filters, string SortColumn, MySQLDataLoading.ServiceReference1.SortDirection SortDirection, int PageSize, int Page);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IBiometricService/GetIdentityGroups", ReplyAction="http://tempuri.org/IBiometricService/GetIdentityGroupsResponse")]
        System.Data.DataSet GetIdentityGroups();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IBiometricService/GetIdentityGroupTransactions", ReplyAction="http://tempuri.org/IBiometricService/GetIdentityGroupTransactionsResponse")]
        System.Data.DataSet GetIdentityGroupTransactions();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IBiometricService/GetIdentityByName", ReplyAction="http://tempuri.org/IBiometricService/GetIdentityByNameResponse")]
        System.Data.DataSet GetIdentityByName(string FirstName, string LastName);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IBiometricService/GetIdentityByFullName", ReplyAction="http://tempuri.org/IBiometricService/GetIdentityByFullNameResponse")]
        System.Data.DataSet GetIdentityByFullName(string FullName);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IBiometricService/GetLogins", ReplyAction="http://tempuri.org/IBiometricService/GetLoginsResponse")]
        System.Data.DataSet GetLogins();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IBiometricService/GetLoginByID", ReplyAction="http://tempuri.org/IBiometricService/GetLoginByIDResponse")]
        System.Data.DataSet GetLoginByID(int id);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IBiometricService/GetAuthenticationTypes", ReplyAction="http://tempuri.org/IBiometricService/GetAuthenticationTypesResponse")]
        System.Data.DataSet GetAuthenticationTypes();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IBiometricService/GetAuthenticationUnits", ReplyAction="http://tempuri.org/IBiometricService/GetAuthenticationUnitsResponse")]
        System.Data.DataSet GetAuthenticationUnits();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IBiometricService/GetAuthenticationLevels", ReplyAction="http://tempuri.org/IBiometricService/GetAuthenticationLevelsResponse")]
        System.Data.DataSet GetAuthenticationLevels();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IBiometricService/GetTransactions", ReplyAction="http://tempuri.org/IBiometricService/GetTransactionsResponse")]
        System.Data.DataSet GetTransactions();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IBiometricService/GetReportData", ReplyAction="http://tempuri.org/IBiometricService/GetReportDataResponse")]
        System.Data.DataSet GetReportData(string storedProc);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IBiometricService/GetReportDataWithRange", ReplyAction="http://tempuri.org/IBiometricService/GetReportDataWithRangeResponse")]
        System.Data.DataSet GetReportDataWithRange(string storedProc, string startdate, string enddate);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IBiometricService/GetAuthenticationLevelUnits", ReplyAction="http://tempuri.org/IBiometricService/GetAuthenticationLevelUnitsResponse")]
        System.Data.DataSet GetAuthenticationLevelUnits();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IBiometricService/GetAuthenticationUnitsByTransaction", ReplyAction="http://tempuri.org/IBiometricService/GetAuthenticationUnitsByTransactionResponse")]
        System.Data.DataSet GetAuthenticationUnitsByTransaction(string TransactionCode);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IBiometricService/GetEnrolledUnitsByIdentity", ReplyAction="http://tempuri.org/IBiometricService/GetEnrolledUnitsByIdentityResponse")]
        System.Data.DataSet GetEnrolledUnitsByIdentity(int IdentityId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IBiometricService/GetIdentityTypes", ReplyAction="http://tempuri.org/IBiometricService/GetIdentityTypesResponse")]
        System.Data.DataSet GetIdentityTypes();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IBiometricService/GetActiveEnrollmentByIdentityId", ReplyAction="http://tempuri.org/IBiometricService/GetActiveEnrollmentByIdentityIdResponse")]
        System.Data.DataSet GetActiveEnrollmentByIdentityId(int IdentityId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IBiometricService/GetCustomersByNameOrId", ReplyAction="http://tempuri.org/IBiometricService/GetCustomersByNameOrIdResponse")]
        System.Data.DataSet GetCustomersByNameOrId(string firstName, string lastName, string t24Id);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IBiometricService/IsCorrectAuthenticationTemplate", ReplyAction="http://tempuri.org/IBiometricService/IsCorrectAuthenticationTemplateResponse")]
        bool IsCorrectAuthenticationTemplate(int IdentityId, string AuthenticationUnitCode, byte[] Template);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IBiometricService/AddIdentityGroup", ReplyAction="http://tempuri.org/IBiometricService/AddIdentityGroupResponse")]
        int AddIdentityGroup(string GroupName, string GroupDescription);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IBiometricService/UpdateIdentityGroup", ReplyAction="http://tempuri.org/IBiometricService/UpdateIdentityGroupResponse")]
        void UpdateIdentityGroup(int GroupId, string GroupName, string GroupDescription, string IdentityIdList, string TransactionIdList);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IBiometricService/RemoveIdentityGroup", ReplyAction="http://tempuri.org/IBiometricService/RemoveIdentityGroupResponse")]
        void RemoveIdentityGroup(int GroupId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IBiometricService/AddIdentity", ReplyAction="http://tempuri.org/IBiometricService/AddIdentityResponse")]
        int AddIdentity(string FirstName, string LastName, string CountryCode, int Active, int BioEnabled, string IdentityCode);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    public interface IBiometricServiceChannel : MySQLDataLoading.ServiceReference1.IBiometricService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    public partial class BiometricServiceClient : System.ServiceModel.ClientBase<MySQLDataLoading.ServiceReference1.IBiometricService>, MySQLDataLoading.ServiceReference1.IBiometricService {
        
        public BiometricServiceClient() {
        }
        
        public BiometricServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public BiometricServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public BiometricServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public BiometricServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public void UpdateIdentity(string FirstName, string LastName, string CountryCode, int Active, int Id, int BioEnabled, string IdentityCode) {
            base.Channel.UpdateIdentity(FirstName, LastName, CountryCode, Active, Id, BioEnabled, IdentityCode);
        }
        
        public void RemoveIdentity(int IdentityId) {
            base.Channel.RemoveIdentity(IdentityId);
        }
        
        public int AddLogin(string Login, string Password, string ApplicationCode, int IdentityId, string SystemLoginId, string SystemLoginPassword, string RoleCode, int FirstLogon, int UseGeneratedPass, string OrigPassword) {
            return base.Channel.AddLogin(Login, Password, ApplicationCode, IdentityId, SystemLoginId, SystemLoginPassword, RoleCode, FirstLogon, UseGeneratedPass, OrigPassword);
        }
        
        public void UpdateLogin(int Id, string Login, string Password, string ApplicationCode, int IdentityId, string SystemLoginId, string SystemLoginPassword, string RoleCode, int FirstLogon, int UseGeneratedPass, string OrigPassword) {
            base.Channel.UpdateLogin(Id, Login, Password, ApplicationCode, IdentityId, SystemLoginId, SystemLoginPassword, RoleCode, FirstLogon, UseGeneratedPass, OrigPassword);
        }
        
        public void RemoveLogins(int IdentityId) {
            base.Channel.RemoveLogins(IdentityId);
        }
        
        public void UpdateTransaction(int TransactionId, string Description, int AuthenticationLevelId) {
            base.Channel.UpdateTransaction(TransactionId, Description, AuthenticationLevelId);
        }
        
        public int AddUpdateAuthenticationEnrollment(int IdentityId, int AdministratorId, System.Collections.Generic.Dictionary<string, byte[]> Templates, System.Collections.Generic.Dictionary<string, int> Score) {
            return base.Channel.AddUpdateAuthenticationEnrollment(IdentityId, AdministratorId, Templates, Score);
        }
        
        public void RemoveAuthenticationEnrollment(int EnrollmentId) {
            base.Channel.RemoveAuthenticationEnrollment(EnrollmentId);
        }
        
        public int AddAuthenticationTemplate(int EnrollmentId, string BiometricUnitCode, byte[] Template) {
            return base.Channel.AddAuthenticationTemplate(EnrollmentId, BiometricUnitCode, Template);
        }
        
        public int AddAuthenticationTemplateScores(int EnrollmentId, string BiometricUnitCode, byte[] Template, int Score) {
            return base.Channel.AddAuthenticationTemplateScores(EnrollmentId, BiometricUnitCode, Template, Score);
        }
        
        public int AddAuthenticationLevel(string Description) {
            return base.Channel.AddAuthenticationLevel(Description);
        }
        
        public int AddAuthenticationUnit(string code, string AuthenticationType, string UnitDescription, int EnrollmentSequence, int AuthenticationLevelId) {
            return base.Channel.AddAuthenticationUnit(code, AuthenticationType, UnitDescription, EnrollmentSequence, AuthenticationLevelId);
        }
        
        public int RemoveAuthenticationLevel(int Id) {
            return base.Channel.RemoveAuthenticationLevel(Id);
        }
        
        public int RemoveAuthenticationUnit(string UnitCode) {
            return base.Channel.RemoveAuthenticationUnit(UnitCode);
        }
        
        public int UpdateAuthenticationLevel(int Id, string Description) {
            return base.Channel.UpdateAuthenticationLevel(Id, Description);
        }
        
        public int UpdateAuthenticationUnit(string Description, int EnrollmentSequence, string AuthenticationType, string Code, int Id) {
            return base.Channel.UpdateAuthenticationUnit(Description, EnrollmentSequence, AuthenticationType, Code, Id);
        }
        
        public void AddAuthenticationLevelUnit(int Id, string code) {
            base.Channel.AddAuthenticationLevelUnit(Id, code);
        }
        
        public void RemoveAuthenticationLevelUnit(int Id) {
            base.Channel.RemoveAuthenticationLevelUnit(Id);
        }
        
        public void ImportT24Users(System.Data.DataTable Data) {
            base.Channel.ImportT24Users(Data);
        }
        
        public void ImportT24Customers(System.Data.DataTable Data) {
            base.Channel.ImportT24Customers(Data);
        }
        
        public void ImportT24Companies(System.Data.DataTable Data) {
            base.Channel.ImportT24Companies(Data);
        }
        
        public void ImportT24AccountOfficers(System.Data.DataTable Data) {
            base.Channel.ImportT24AccountOfficers(Data);
        }
        
        public void ImportT24Languages(System.Data.DataTable Data) {
            base.Channel.ImportT24Languages(Data);
        }
        
        public bool ProcessT24Imports() {
            return base.Channel.ProcessT24Imports();
        }
        
        public void LogDebugMessage(string Message) {
            base.Channel.LogDebugMessage(Message);
        }
        
        public void LogInfoMessage(string Message) {
            base.Channel.LogInfoMessage(Message);
        }
        
        public void LogWarningMessage(string Message) {
            base.Channel.LogWarningMessage(Message);
        }
        
        public void LogErrorMessage(string Message) {
            base.Channel.LogErrorMessage(Message);
        }
        
        public void LogErrorMessageWithException(string Message, System.Exception ex) {
            base.Channel.LogErrorMessageWithException(Message, ex);
        }
        
        public System.Data.DataSet GetRoles() {
            return base.Channel.GetRoles();
        }
        
        public System.Data.DataSet GetApplications() {
            return base.Channel.GetApplications();
        }
        
        public System.Data.DataSet GetApplicationByID(int id) {
            return base.Channel.GetApplicationByID(id);
        }
        
        public System.Data.DataSet GetApplicationTypes() {
            return base.Channel.GetApplicationTypes();
        }
        
        public System.Data.DataSet GetCountries() {
            return base.Channel.GetCountries();
        }
        
        public System.Data.DataSet GetCountryByID(int id) {
            return base.Channel.GetCountryByID(id);
        }
        
        public System.Data.DataSet GetReports() {
            return base.Channel.GetReports();
        }
        
        public System.Data.DataSet GetReportByID(int id) {
            return base.Channel.GetReportByID(id);
        }
        
        public MySQLDataLoading.ServiceReference1.IdentityData[] GetIdentities(out int TotalRecords, System.Collections.Generic.Dictionary<string, string> Filters, string SortColumn, MySQLDataLoading.ServiceReference1.SortDirection SortDirection, int PageSize, int Page) {
            return base.Channel.GetIdentities(out TotalRecords, Filters, SortColumn, SortDirection, PageSize, Page);
        }
        
        public System.Data.DataSet GetIdentityGroups() {
            return base.Channel.GetIdentityGroups();
        }
        
        public System.Data.DataSet GetIdentityGroupTransactions() {
            return base.Channel.GetIdentityGroupTransactions();
        }
        
        public System.Data.DataSet GetIdentityByName(string FirstName, string LastName) {
            return base.Channel.GetIdentityByName(FirstName, LastName);
        }
        
        public System.Data.DataSet GetIdentityByFullName(string FullName) {
            return base.Channel.GetIdentityByFullName(FullName);
        }
        
        public System.Data.DataSet GetLogins() {
            return base.Channel.GetLogins();
        }
        
        public System.Data.DataSet GetLoginByID(int id) {
            return base.Channel.GetLoginByID(id);
        }
        
        public System.Data.DataSet GetAuthenticationTypes() {
            return base.Channel.GetAuthenticationTypes();
        }
        
        public System.Data.DataSet GetAuthenticationUnits() {
            return base.Channel.GetAuthenticationUnits();
        }
        
        public System.Data.DataSet GetAuthenticationLevels() {
            return base.Channel.GetAuthenticationLevels();
        }
        
        public System.Data.DataSet GetTransactions() {
            return base.Channel.GetTransactions();
        }
        
        public System.Data.DataSet GetReportData(string storedProc) {
            return base.Channel.GetReportData(storedProc);
        }
        
        public System.Data.DataSet GetReportDataWithRange(string storedProc, string startdate, string enddate) {
            return base.Channel.GetReportDataWithRange(storedProc, startdate, enddate);
        }
        
        public System.Data.DataSet GetAuthenticationLevelUnits() {
            return base.Channel.GetAuthenticationLevelUnits();
        }
        
        public System.Data.DataSet GetAuthenticationUnitsByTransaction(string TransactionCode) {
            return base.Channel.GetAuthenticationUnitsByTransaction(TransactionCode);
        }
        
        public System.Data.DataSet GetEnrolledUnitsByIdentity(int IdentityId) {
            return base.Channel.GetEnrolledUnitsByIdentity(IdentityId);
        }
        
        public System.Data.DataSet GetIdentityTypes() {
            return base.Channel.GetIdentityTypes();
        }
        
        public System.Data.DataSet GetActiveEnrollmentByIdentityId(int IdentityId) {
            return base.Channel.GetActiveEnrollmentByIdentityId(IdentityId);
        }
        
        public System.Data.DataSet GetCustomersByNameOrId(string firstName, string lastName, string t24Id) {
            return base.Channel.GetCustomersByNameOrId(firstName, lastName, t24Id);
        }
        
        public bool IsCorrectAuthenticationTemplate(int IdentityId, string AuthenticationUnitCode, byte[] Template) {
            return base.Channel.IsCorrectAuthenticationTemplate(IdentityId, AuthenticationUnitCode, Template);
        }
        
        public int AddIdentityGroup(string GroupName, string GroupDescription) {
            return base.Channel.AddIdentityGroup(GroupName, GroupDescription);
        }
        
        public void UpdateIdentityGroup(int GroupId, string GroupName, string GroupDescription, string IdentityIdList, string TransactionIdList) {
            base.Channel.UpdateIdentityGroup(GroupId, GroupName, GroupDescription, IdentityIdList, TransactionIdList);
        }
        
        public void RemoveIdentityGroup(int GroupId) {
            base.Channel.RemoveIdentityGroup(GroupId);
        }
        
        public int AddIdentity(string FirstName, string LastName, string CountryCode, int Active, int BioEnabled, string IdentityCode) {
            return base.Channel.AddIdentity(FirstName, LastName, CountryCode, Active, BioEnabled, IdentityCode);
        }
    }
}
