using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using Entities;

namespace TodoList.WpfClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        private ObservableCollection<TodoItem> _ToDoItems = new ObservableCollection<TodoItem>();
        private ServiceReference1.TodoListServiceClient _Service;
        
        /// <summary>
        /// Will hold the newly created Item (top of the screen)
        /// </summary>
        private TodoItem _NewTodo;

        public MainWindow()
        {
            
            InitializeComponent();

            // Prepare the task creation panel
            PriorityComboBox.ItemsSource = TodoItem.PriorityFlags;
            _NewTodo = new TodoItem {Priority = PriorityFlag.Normal, DueDate = DateTime.Today};
            NewItemPanel.DataContext = _NewTodo;

            //Making sure we do not call the web service at design time
            if (!DesignerProperties.GetIsInDesignMode(new DependencyObject()))
            {
                _Service = new ServiceReference1.TodoListServiceClient();
                _Service.BeginGetItems(TodoListLoaded, null);
            }

        }

        /// <summary>
        ///  List Was Loaded from Web service
        ///     Let's bind to the UI and hook up to property changes
        /// </summary>
        private void TodoListLoaded(IAsyncResult result)
        {
            if(result.IsCompleted)
            {
               _ToDoItems = _Service.EndGetItems(result);

                foreach(TodoItem item in _ToDoItems)
                {
                    item.PropertyChanged += Item_PropertyChanged;
                }

               // FORWARD TO UI THREAD
               Dispatcher.BeginInvoke(
                    DispatcherPriority.Normal,
                    (DispatcherOperationCallback)delegate(object arg)
                    {
                        // HANDLED ON UI THREAD
                        TodoDataGrid.ItemsSource = _ToDoItems;
                        return null;
                    }, null);
            }
        }

        /// <summary>
        ///  A TodoItem was modified... 
        ///     Let's relay the changes to the web service
        /// </summary>
        void Item_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // an Item has changed let's update the web service
            TodoItem item = (TodoItem) sender;
            _Service.BeginUpdateItem(item, TodoListItemUpdated, item);
        }

        private void TodoListItemUpdated(IAsyncResult result)
        {
            if(!result.IsCompleted)
            {
                // TODO: Get original values from service here
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

                _Service.BeginCreateItem(_NewTodo, TodoItemCreated, _NewTodo);

            }
            catch (Exception ex)
            {
                DescriptionTextBox.Text = _NewTodo.Description;
                TitleTextBox.Text = _NewTodo.Title;
                MessageBox.Show(ex.Message);
            }

        }

        private void TodoItemCreated(IAsyncResult result)
        {
            if(result.IsCompleted)
            {
                // FORWARD TO UI THREAD
                Dispatcher.BeginInvoke(
                     DispatcherPriority.Normal,
                     (DispatcherOperationCallback)delegate(object arg)
                     {
                         // HANDLED ON UI THREAD
                         _NewTodo.ID = _Service.EndCreateItem(result);
                         _NewTodo.PropertyChanged += Item_PropertyChanged;
                         _ToDoItems.Add(_NewTodo);
                         _NewTodo = new TodoItem { Priority = PriorityFlag.Normal, DueDate = DateTime.Today };
                         NewItemPanel.DataContext = _NewTodo;
                         TitleTextBox.Focus(); 
                         return null;
                     }, null); 

            }
        }

        private void DeleteTodoItem_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            // Remove todoItem from the list
            if(MessageBox.Show("Ok to delete this Item?","Deleting Todo",MessageBoxButton.OKCancel,MessageBoxImage.Exclamation)==MessageBoxResult.OK)
            {
                TodoItem item = (TodoItem)((Button) sender).DataContext;
                _Service.BeginDeleteItem(item.ID, TodoItemDeleted,item);
            }
        }

         private void TodoItemDeleted(IAsyncResult result)
         {
             TodoItem item = (TodoItem)result.AsyncState;
             if (result.IsCompleted)
             {
                 // FORWARD TO UI THREAD
                 Dispatcher.BeginInvoke(
                      DispatcherPriority.Normal,
                      (DispatcherOperationCallback)delegate(object arg)
                      {
                          // HANDLED ON UI THREAD
                          item.PropertyChanged -= Item_PropertyChanged;
                         _ToDoItems.Remove(item);
                          return null;
                      }, null);
             }
         }

    }
}
