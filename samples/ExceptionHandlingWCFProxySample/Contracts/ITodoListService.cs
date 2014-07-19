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

    [ServiceContract(Namespace="http://wcfclientguidance.codeplex.com/2009/04")]
    public interface ITodoListService
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
