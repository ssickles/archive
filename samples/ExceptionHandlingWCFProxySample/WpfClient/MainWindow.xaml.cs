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

namespace TodoList.WpfClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        private ObservableCollection<TodoItem> _ToDoItems = new ObservableCollection<TodoItem>();
        private TodoListServiceProxy _Proxy;
        
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
            PriorityComboBox.ItemsSource = TodoItemFlags.PriorityFlags;
            _NewTodo = new TodoItem { Priority = PriorityFlag.Normal, DueDate = DateTime.Today };
            NewItemPanel.DataContext = _NewTodo;

            _Proxy = new TodoListServiceProxy("default");

            try
            {
                _ToDoItems = _Proxy.GetItems();
                foreach (TodoItem item in _ToDoItems)
                {
                    item.PropertyChanged += Item_PropertyChanged;
                }

                TodoDataGrid.ItemsSource = _ToDoItems;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());

                throw;
            }
        }

        /// <summary>
        ///  A TodoItem was modified... 
        ///     Let's relay the changes to the web service
        /// </summary>
        void Item_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // an Item has changed let's update the web service
            try
            {
                TodoItem item = (TodoItem)sender;
                _Proxy.UpdateItem(item);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
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
                _ToDoItems.Add(_NewTodo);
                
                _NewTodo = new TodoItem { Priority = PriorityFlag.Normal, DueDate = DateTime.Today };
                NewItemPanel.DataContext = _NewTodo;

                TitleTextBox.Focus();
            }
            catch (Exception ex)
            {
                DescriptionTextBox.Text = _NewTodo.Description;
                TitleTextBox.Text = _NewTodo.Title;
                MessageBox.Show(ex.ToString());
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
                    _ToDoItems.Remove(item);

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }


            }
        }

        private void window_Closing(object sender, CancelEventArgs e)
        {
            _Proxy.Dispose();
        }

   }
}
