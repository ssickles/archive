﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.42
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace QuickReturns.StockTrading.ExchangeServiceClient.localhost
{
    using System.Runtime.Serialization;
    using System;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "3.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://QuickReturns")]
    [System.SerializableAttribute()]
    public partial class Quote : object, System.Runtime.Serialization.IExtensibleDataObject
    {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private decimal AskField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private decimal BidField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string PublisherField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string TickerField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.DateTime UpdateDateTimeField;
        
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData
        {
            get
            {
                return this.extensionDataField;
            }
            set
            {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public decimal Ask
        {
            get
            {
                return this.AskField;
            }
            set
            {
                this.AskField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public decimal Bid
        {
            get
            {
                return this.BidField;
            }
            set
            {
                this.BidField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Publisher
        {
            get
            {
                return this.PublisherField;
            }
            set
            {
                this.PublisherField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Ticker
        {
            get
            {
                return this.TickerField;
            }
            set
            {
                this.TickerField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.DateTime UpdateDateTime
        {
            get
            {
                return this.UpdateDateTimeField;
            }
            set
            {
                this.UpdateDateTimeField = value;
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://QuickReturns", ConfigurationName="QuickReturns.StockTrading.ExchangeServiceClient.localhost.ITradeService")]
    public interface ITradeService
    {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://QuickReturns/ITradeService/GetQuote", ReplyAction="http://QuickReturns/ITradeService/GetQuoteResponse")]
        QuickReturns.StockTrading.ExchangeServiceClient.localhost.Quote GetQuote(string ticker);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://QuickReturns/ITradeService/PublishQuote", ReplyAction="http://QuickReturns/ITradeService/PublishQuoteResponse")]
        void PublishQuote(QuickReturns.StockTrading.ExchangeServiceClient.localhost.Quote quote);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    public interface ITradeServiceChannel : QuickReturns.StockTrading.ExchangeServiceClient.localhost.ITradeService, System.ServiceModel.IClientChannel
    {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    public partial class TradeServiceClient : System.ServiceModel.ClientBase<QuickReturns.StockTrading.ExchangeServiceClient.localhost.ITradeService>, QuickReturns.StockTrading.ExchangeServiceClient.localhost.ITradeService
    {
        
        public TradeServiceClient()
        {
        }
        
        public TradeServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName)
        {
        }
        
        public TradeServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress)
        {
        }
        
        public TradeServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress)
        {
        }
        
        public TradeServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress)
        {
        }
        
        public QuickReturns.StockTrading.ExchangeServiceClient.localhost.Quote GetQuote(string ticker)
        {
            return base.Channel.GetQuote(ticker);
        }
        
        public void PublishQuote(QuickReturns.StockTrading.ExchangeServiceClient.localhost.Quote quote)
        {
            base.Channel.PublishQuote(quote);
        }
    }
}