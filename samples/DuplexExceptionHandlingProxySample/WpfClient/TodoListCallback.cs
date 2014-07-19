// © 2009 Michele Leroux Bustamante. All rights reserved. 
// See http://wcfguidanceforwpf.codeplex.com for related whitepaper and updates
// For an intro to WCF see Michele's book: Learning WCF, O'Reilly 2007 (updated August 2008 for VS 2008)
// See http://www.thatindigogirl.com for the book code!
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TodoList.WpfClient.ServiceReference1;
using System.ServiceModel;
using System.Windows;
using System.Threading;
using System.Diagnostics;

namespace TodoList.WpfClient
{
    [CallbackBehavior(ConcurrencyMode = ConcurrencyMode.Single, UseSynchronizationContext=false)]
    public class TodoListCallback: ITodoListServiceCallback
    {
        private MainWindow _MainWindow;
        
        public TodoListCallback(MainWindow window)
        {
            _MainWindow = window; 
        }

        #region ITodoListSubscriberCallback Members

        public void ItemAdded(TodoItem item)
        {
            try
            {
                Trace.WriteLine(string.Format("ItemAdded callback on thread {0}", Thread.CurrentThread.GetHashCode()));

                _MainWindow._SyncContext.Send(state =>
                {
                    _MainWindow._TodoItems.Add(item);
                }, null);
                
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message);
            }
        }

        public void ItemChanged(TodoItem item)
        {
            try
            {
                _MainWindow._SyncContext.Send(state =>
                {
                    TodoItem found = _MainWindow._TodoItems.First<TodoItem>(x => x.ID == item.ID);

                    found.PropertyChanged -= _MainWindow.Item_PropertyChanged;

                    found.CreationDate = item.CreationDate;
                    found.CompletionDate = item.CompletionDate;
                    found.Description = item.Description;
                    found.DueDate = item.DueDate;
                    found.Title = item.Title;
                    found.Priority = item.Priority;
                    found.PercentComplete = item.PercentComplete;
                    found.Status = item.Status;
                    found.Tags = item.Tags;

                    found.PropertyChanged += _MainWindow.Item_PropertyChanged;
                }, null);
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message);
            }
                
        }

        public void ItemDeleted(string id)
        {
            try
            {
                _MainWindow._SyncContext.Send(state =>
                {
                    TodoItem found = _MainWindow._TodoItems.First<TodoItem>(x => x.ID == id);
                    _MainWindow._TodoItems.Remove(found);
                }, null);
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message);
            }
            
        }

        #endregion
    }
}
