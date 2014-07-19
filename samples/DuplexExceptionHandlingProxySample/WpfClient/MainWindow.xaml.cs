// © 2009 Michele Leroux Bustamante. All rights reserved. 
// See http://wcfguidanceforwpf.codeplex.com for related whitepaper and updates
// For an intro to WCF see Michele's book: Learning WCF, O'Reilly 2007 (updated August 2008 for VS 2008)
// See http://www.thatindigogirl.com for the book code!

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using TodoList.WpfClient.ServiceReference1;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading;
using System.Diagnostics;

namespace TodoList.WpfClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public SynchronizationContext _SyncContext = SynchronizationContext.Current;
        public ObservableCollection<TodoItem> _TodoItems = new ObservableCollection<TodoItem>();

        private TodoListServiceDuplexProxy<TodoListCallback> _Proxy;        
        /// <summary>
        /// Will hold the newly created Item (top of the screen)
        /// </summary>
        private TodoItem _NewTodo;

        public MainWindow()
        {
            
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(MainWindow_Loaded);

        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {

            // Prepare the task creation panel
            PriorityComboBox.ItemsSource = TodoItem.PriorityFlags;
            _NewTodo = new TodoItem { Priority = PriorityFlag.Normal, DueDate = DateTime.Today };
            NewItemPanel.DataContext = _NewTodo;

            InitializeProxy();
            GetData();
        }

        private void GetData()
        {
            try
            {
                _TodoItems = _Proxy.GetItems();

                foreach (TodoItem item in _TodoItems)
                {
                    item.PropertyChanged += Item_PropertyChanged;
                }

                TodoDataGrid.ItemsSource = _TodoItems;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void InitializeProxy()
        {
                try
                {
                    Trace.WriteLine(string.Format("Initializing proxy on thread {0}", Thread.CurrentThread.GetHashCode()));

                    TodoListCallback callback = new TodoListCallback(this);
                    this.Title = string.Format("Todo List Client: Thread ID {0}", Thread.CurrentThread.GetHashCode());

                 
                    _Proxy = new TodoListServiceDuplexProxy<TodoListCallback>(callback,"default");
                    _Proxy.AfterRecreateProxy += new EventHandler(_Proxy_AfterRecreateProxy);
                    _Proxy.Subscribe();

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

        }

        void _Proxy_AfterRecreateProxy(object sender, EventArgs e)
        {
            _Proxy.Subscribe();
            // marshal UI work to UI thread
            this._SyncContext.Post((x) =>
                {
                    GetData();
                }, null
            );

        }

        /// <summary>
        ///  A TodoItem was modified... 
        ///     Let's relay the changes to the web service
        /// </summary>
        public void Item_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // an Item has changed let's update the web service
            try
            {
                TodoItem item = (TodoItem)sender;
                _Proxy.UpdateItem(item);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        ///  Add a TodoItem
        /// </summary>
        private void AddTodoButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                _NewTodo.CreationDate = DateTime.Today;
                _NewTodo.Description = DescriptionTextBox.Text;
                _NewTodo.DueDate = DueDatePicker.SelectedDate.GetValueOrDefault(DateTime.Today);
                _NewTodo.Title = TitleTextBox.Text;
                _NewTodo.Priority = (PriorityFlag) PriorityComboBox.SelectedItem;

                _NewTodo.ID = _Proxy.CreateItem(_NewTodo);

                _NewTodo.PropertyChanged += Item_PropertyChanged;
                _TodoItems.Add(_NewTodo);
                
                _NewTodo = new TodoItem { Priority = PriorityFlag.Normal, DueDate = DateTime.Today };
                NewItemPanel.DataContext = _NewTodo;

                
                TitleTextBox.Focus();
            }
            catch (Exception ex)
            {
                DescriptionTextBox.Text = _NewTodo.Description;
                TitleTextBox.Text = _NewTodo.Title;
                MessageBox.Show(ex.Message);
            }

        }


        private void DeleteTodoItem_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            // Remove todoItem from the list
            if(MessageBox.Show("Ok to delete this Item?","Deleting Todo",MessageBoxButton.OKCancel,MessageBoxImage.Exclamation)==MessageBoxResult.OK)
            {
                TodoItem item = (TodoItem)((Button) sender).DataContext;
                try
                {
                    _Proxy.DeleteItem(item.ID);

                    item.PropertyChanged -= Item_PropertyChanged;
                    _TodoItems.Remove(item);

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }


            }
        }

        private void window_Closing(object sender, CancelEventArgs e)
        {
            this._Proxy.Dispose();
        }

   }
}
