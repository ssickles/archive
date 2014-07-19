using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Entities;

namespace Contracts
{

    [ServiceContract(Namespace="urn://mobilewcf/samples/2009/04")]
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
