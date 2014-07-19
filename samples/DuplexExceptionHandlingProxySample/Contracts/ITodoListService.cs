// © 2009 Michele Leroux Bustamante. All rights reserved. 
// See http://wcfguidanceforwpf.codeplex.com for related whitepaper and updates
// For an intro to WCF see Michele's book: Learning WCF, O'Reilly 2007 (updated August 2008 for VS 2008)
// See http://www.thatindigogirl.com for the book code!

   
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Entities;

namespace Contracts
{

    public enum TodoListEvent
    {
        ItemAdded,
        ItemChanged,
        ItemDeleted
    }

    [ServiceContract(Namespace = "http://wcfclientguidance.codeplex.com/2009/04")]
    public interface ITodoListEvents
    {
        [OperationContract(IsOneWay = true)]
        void ItemAdded(TodoItem item);

        [OperationContract(IsOneWay = true)]
        void ItemChanged(TodoItem item);

        [OperationContract(IsOneWay = true)]
        void ItemDeleted(string id);
    }

    [ServiceContract(Namespace="http://wcfclientguidance.codeplex.com/2009/04", CallbackContract=typeof(ITodoListEvents))]
    public interface ITodoListService: ISubscriber
    {
        [OperationContract]
        List<TodoItem> GetItems();

        [OperationContract]
        string CreateItem(TodoItem item);
        [OperationContract]
        void UpdateItem(TodoItem item);
        [OperationContract]
        void DeleteItem(string id);
    }


}
