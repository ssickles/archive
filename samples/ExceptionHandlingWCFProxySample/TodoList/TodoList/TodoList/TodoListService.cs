using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Entities;
using Contracts;
using System.Diagnostics;

namespace TodoList
{

    [ServiceBehavior(InstanceContextMode=InstanceContextMode.Single, ConcurrencyMode=ConcurrencyMode.Single, UseSynchronizationContext=false, IgnoreExtensionDataObject=true)]
    public class TodoListService: ITodoListService
    {
        List<TodoItem> m_globalTodoList = new List<TodoItem>();
        private int _ItemCount = 0;

        TodoListService()
        {
            Random rnd = new Random();

            for (int x = 1; x < 15; x++)
            {
                m_globalTodoList.Add(new TodoItem
                {
                    Title = string.Format("Todo Item #{0}", x),
                    ID = x.ToString(),
                    CreationDate = DateTime.Today.AddDays(-x),
                    Status = StatusFlag.InProgress,
                    DueDate = DateTime.Today.AddDays(x),
                    Priority = (PriorityFlag)rnd.Next(0, 3),
                    Description = string.Format("blah {0} blah {0} blah {0} blah {0} blah {0} blah", x),
                    PercentComplete = rnd.NextDouble() * 100.00

                });
            }
            _ItemCount = 14;
        }

        #region ITodoListService Members

        public List<TodoItem> GetItems()
        {
            Trace("Returning all items");
           
            return m_globalTodoList;
        }

        public string CreateItem(TodoItem item)
        {
            _ItemCount++;
            item.ID = _ItemCount.ToString();
            m_globalTodoList.Add(item);

            Trace(string.Format("Created new item {0}", item.ID));

            return item.ID;
        }

        public void UpdateItem(TodoItem item)
        {
            TodoItem found = m_globalTodoList.Find(x => x.ID == item.ID);
            found.Title = item.Title;
            found.Description = item.Description;
            found.DueDate = item.DueDate;
            found.CompletionDate = item.CompletionDate;
            found.PercentComplete = item.PercentComplete;
            found.Priority = item.Priority;
            found.Status = item.Status;
            found.Tags = item.Tags;

            Trace(string.Format("Updating item {0}", item.ID));
        }

        public void DeleteItem(string id)
        {
            Trace(string.Format("Deleting item {0}", id));
            m_globalTodoList.RemoveAll(x => x.ID == id);
        }

        #endregion

        [Conditional("DEBUG")]
        private void Trace(string message)
        {
            Console.WriteLine(message);
        }

    }

}
