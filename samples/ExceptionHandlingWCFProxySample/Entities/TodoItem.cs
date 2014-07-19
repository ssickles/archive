// © 2009 Michele Leroux Bustamante. All rights reserved. 
// See http://wcfguidanceforwpf.codeplex.com for related whitepaper and updates
// For an intro to WCF see Michele's book: Learning WCF, O'Reilly 2007 (updated August 2008 for VS 2008)
// See http://www.thatindigogirl.com for the book code!

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Text;

namespace Entities
{
    [DataContract(Namespace = "http://wcfclientguidance.codeplex.com/2009/04/schemas")]
    public class TodoItem 
    {
        [DataMember(Order=1, IsRequired=false)]
        public string ID { get; set; }
        [DataMember(Order = 2, IsRequired = true)]
        public string Title { get; set; }
        [DataMember(Order = 3, IsRequired = true)]
        public string Description { get; set; }
        [DataMember(Order = 4, IsRequired = true)]
        public PriorityFlag Priority { get; set; }
        [DataMember(Order = 5, IsRequired = true)]
        public StatusFlag Status { get; set; }
        [DataMember(Order = 6, IsRequired = true)]
        public DateTime? CreationDate { get; set; }
        [DataMember(Order = 7, IsRequired = true)]
        public DateTime? DueDate { get; set; }
        [DataMember(Order = 8, IsRequired = true)]
        public DateTime? CompletionDate { get; set; }
        [DataMember(Order = 9, IsRequired = true)]
        public double PercentComplete { get; set; }
        [DataMember(Order = 10, IsRequired = true)]
        public string Tags { get; set; }
    }

    public enum PriorityFlag
    {
        Low,
        Normal,
        High
    }

    public enum StatusFlag
    {
        NotStarted,
        InProgress,
        Completed,
        WaitingOnSomeoneElse,
        Deferred
    }
}
