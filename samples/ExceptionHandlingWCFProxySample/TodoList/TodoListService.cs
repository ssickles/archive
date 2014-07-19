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
using Contracts;
using System.Diagnostics;

namespace TodoList
{

    [ServiceBehavior(InstanceContextMode=InstanceContextMode.PerCall, ConcurrencyMode=ConcurrencyMode.Multiple)]
    public class TodoListService: ITodoListService
    {
        private static object _TodoListLock = new object();
        private static List<TodoItem> _GlobalTodoList = new List<TodoItem>();

        static TodoListService()
        {
            TodoItem item = new TodoItem()
            {
                ID = Guid.NewGuid().ToString(),
                Title = "WCF/Mobile Whitepaper",
                Description = "http://wcfguidanceformobile.codeplex.com",
                Status = StatusFlag.Completed,
                Priority = PriorityFlag.Normal,
                CompletionDate = new DateTime(2009, 4, 20),
                DueDate = new DateTime(2009, 4, 20),
                PercentComplete = 100
            };
            _GlobalTodoList.Add(item);

            item = new TodoItem()
            {
                ID = Guid.NewGuid().ToString(),
                Title = "WCF/WPF Whitepaper",
                Description = "http://wcfguidanceforwpf.codeplex.com",
                Status = StatusFlag.Completed,
                Priority = PriorityFlag.Normal,
                CompletionDate = new DateTime(2009, 5, 10),
                DueDate = new DateTime(2009, 5, 10),
                PercentComplete = 100
            };
            _GlobalTodoList.Add(item);

            item = new TodoItem()
            {
                ID = Guid.NewGuid().ToString(),
                Title = "CardSpace/WPF Webcasts",
                Description = "http://wpfandcardspace.codeplex.com",
                Status = StatusFlag.InProgress,
                Priority = PriorityFlag.High,
                DueDate = new DateTime(2009, 6, 30),
                PercentComplete = 100
            };
            _GlobalTodoList.Add(item);

            item = new TodoItem()
            {
                ID = Guid.NewGuid().ToString(),
                Title = "WPF Publish and Subscribe Template",
                Description = "http://wpfservicehost.codeplex.com",
                Status = StatusFlag.Completed,
                Priority = PriorityFlag.Normal,
                CompletionDate = new DateTime(2009, 6, 5),
                DueDate = new DateTime(2009, 6, 5),
                PercentComplete = 100
            };
            _GlobalTodoList.Add(item);

            item = new TodoItem()
            {
                ID = Guid.NewGuid().ToString(),
                Title = "WPF ServiceHost Template",
                Description = "http://wpfservicehost.codeplex.com",
                Status = StatusFlag.Completed,
                Priority = PriorityFlag.Normal,
                CompletionDate = new DateTime(2009, 6, 5),
                DueDate = new DateTime(2009, 6, 5),
                PercentComplete = 100
            };
            _GlobalTodoList.Add(item);

            item = new TodoItem()
            {
                ID = Guid.NewGuid().ToString(),
                Title = "Exception Handling WCF Proxy Generator",
                Description = "http://wcfproxygenerator.codeplex.com",
                Status = StatusFlag.Completed,
                Priority = PriorityFlag.Normal,
                CompletionDate = new DateTime(2009, 6, 5),
                DueDate = new DateTime(2009, 6, 5),
                PercentComplete = 100
            };
            _GlobalTodoList.Add(item);

            item = new TodoItem()
            {
                ID = Guid.NewGuid().ToString(),
                Title = "Claims-Based WPF Clients Whitepaper",
                Description = "http://claimsbasedwpf.codeplex.com",
                Status = StatusFlag.InProgress,
                Priority = PriorityFlag.High,
                DueDate = new DateTime(2009, 6, 26),
                PercentComplete = 50
            };

            item = new TodoItem()
            {
                ID = Guid.NewGuid().ToString(),
                Title = "WPF and ClickOnce Webcasts",
                Description = "http://wpfandclickonce.codeplex.com",
                Status = StatusFlag.InProgress,
                Priority = PriorityFlag.High,
                DueDate = new DateTime(2009, 6, 22),
                PercentComplete = 80
            };
            _GlobalTodoList.Add(item);


        }

        #region ITodoListService Members


        public List<TodoItem> GetItems()
        {
            Trace("Returning all items");
            return _GlobalTodoList;
        }

        public string CreateItem(TodoItem item)
        {
            lock (_TodoListLock)
            {
                item.ID = _GlobalTodoList.Count.ToString();
                _GlobalTodoList.Add(item);
            }

            Trace(string.Format("Created new item {0}", item.ID));

            return item.ID;
        }

        public void UpdateItem(TodoItem item)
        {
            lock (_TodoListLock)
            {
                TodoItem found = _GlobalTodoList.Find(x => x.ID == item.ID);
                found.Title = item.Title;
                found.Description = item.Description;
                found.DueDate = item.DueDate;
                found.CompletionDate = item.CompletionDate;
                found.PercentComplete = item.PercentComplete;
                found.Priority = item.Priority;
                found.Status = item.Status;
                found.Tags = item.Tags;
            }
            Trace(string.Format("Updating item {0}", item.ID));
        }

        public void DeleteItem(string id)
        {
            Trace(string.Format("Deleting item {0}", id));
            lock (_TodoListLock)
            {
                _GlobalTodoList.RemoveAll(x => x.ID == id);
            }
        }

        #endregion

        [Conditional("DEBUG")]
        private void Trace(string message)
        {
            Console.WriteLine(message);
        }

    }

}
