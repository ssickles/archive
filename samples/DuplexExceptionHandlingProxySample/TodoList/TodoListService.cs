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
using System.Threading;

namespace TodoList
{

    [ServiceBehavior(InstanceContextMode=InstanceContextMode.PerCall, ConcurrencyMode=ConcurrencyMode.Multiple)]
    public class TodoListService: ITodoListService
    {
        public static List<TodoItem> _GlobalTodoList = new List<TodoItem>();

        public static object _SubscriberLock = new object();
        public static List<ITodoListEvents> _TodoListEventSubscribers = new List<ITodoListEvents>();
        
        #region ITodoListService Members

        public List<TodoItem> GetItems()
        {
            Trace("Returning all items");
           
            return _GlobalTodoList;
        }

        public string CreateItem(TodoItem item)
        {

            item.ID = _GlobalTodoList.Count.ToString();
            _GlobalTodoList.Add(item);

            Trace(string.Format("Created new item {0}", item.ID));

            Publish(TodoListEvent.ItemAdded, item);

            return item.ID;
        }

        private void Publish(TodoListEvent todoListEvent, TodoItem item)
        {
            ITodoListEvents callback = OperationContext.Current.GetCallbackChannel<ITodoListEvents>();

            Thread t = new Thread(x =>
            {
                List<ITodoListEvents> toremove = new List<ITodoListEvents>();

                lock (_SubscriberLock)
                {
                    foreach (ITodoListEvents cb in _TodoListEventSubscribers)
                    {
                        if (cb.GetHashCode() != x.GetHashCode())
                        {
                            Console.WriteLine("Sending event to {0}", cb.GetHashCode());
                            try
                            {
                                if (todoListEvent == TodoListEvent.ItemAdded)
                                    cb.ItemAdded(item);
                                else if (todoListEvent == TodoListEvent.ItemChanged)
                                    cb.ItemChanged(item);
                                else if (todoListEvent == TodoListEvent.ItemDeleted)
                                    cb.ItemDeleted(item.ID);
                            }
                            catch (Exception ex)
                            {
                                FaultException faultex = ex as FaultException;
                                if (faultex == null)
                                {
                                    Console.WriteLine("Callback failed, removing {0}", cb.GetHashCode());
                                    toremove.Add(cb);
                                }
                            }
                        }
                    }
                    if (toremove.Count > 0)
                    {
                        foreach (ITodoListEvents cb in toremove)
                        {
                            _TodoListEventSubscribers.Remove(cb);
                        }
                    }
                }

            });
            t.Start(callback);

        }

        public void UpdateItem(TodoItem item)
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

            Trace(string.Format("Updating item {0}", item.ID));

            Publish(TodoListEvent.ItemChanged, found);

        }

        public void DeleteItem(string id)
        {
            Trace(string.Format("Deleting item {0}", id));
            _GlobalTodoList.RemoveAll(x => x.ID == id);

            Publish(TodoListEvent.ItemDeleted, new TodoItem { ID = id });

        }

        #endregion

        [Conditional("DEBUG")]
        private void Trace(string message)
        {
            Console.WriteLine(message);
        }


        #region ITodoListSubscriber Members


        public void Subscribe()
        {
            ITodoListEvents callback = OperationContext.Current.GetCallbackChannel<ITodoListEvents>();

            lock (_SubscriberLock)
            {
                if (!_TodoListEventSubscribers.Contains(callback))
                {
                    Console.WriteLine("Adding callback {0}", callback.GetHashCode());
                    _TodoListEventSubscribers.Add(callback);
                }
            }
        }

        public void Unsubscribe()
        {
            ITodoListEvents callback = OperationContext.Current.GetCallbackChannel<ITodoListEvents>();
            lock (_SubscriberLock)
            {
                if (_TodoListEventSubscribers.Contains(callback))
                {
                    Console.WriteLine("Removing callback {0}", callback.GetHashCode());
                    _TodoListEventSubscribers.Remove(callback);
                }
            }
        }

        #endregion

    }

}
