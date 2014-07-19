using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestClient.ServiceReference1;

namespace TestClient
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceReference1.TodoListServiceClient proxy = new TestClient.ServiceReference1.TodoListServiceClient();

            // create items
            List<string> newItemIds = new List<string>();
            Console.WriteLine("Creating new todo items...");
            for (int i = 0; i < 10; i++)
            {
                TodoItem item = new TodoItem();
                item.Title = string.Format("Item {0}", i);
                item.Description = string.Format("Description for Item {0}", i);
                item.Tags = "Testing";
                item.DueDate = DateTime.Now;

                string id = proxy.CreateItem(item);
                newItemIds.Add(id);
            }
            Console.WriteLine();

            // show item ids
            foreach (string s in newItemIds)
                Console.WriteLine(s);

            Console.WriteLine();

            // get all items
            Console.WriteLine("Calling GetItems()...");
            List<TodoItem> items = proxy.GetItems();
            foreach (TodoItem item in items)
                Console.WriteLine("{0}, {1}, {2}", item.Id, item.DueDate.ToShortDateString(), item.Title);
            Console.WriteLine();

            // modify item
            Console.WriteLine("Modifying Item 1 with UpdateItem()...");
            TodoItem toUpdate = items[0];
            toUpdate.Title = "Updated Item";
            proxy.UpdateItem(toUpdate);
            Console.WriteLine();

            // delete an item 
            Console.WriteLine("Calling DeleteItem()...");
            TodoItem toDelete = items[5];
            proxy.DeleteItem(toDelete.Id);
            Console.WriteLine();

            // delete invalid item (test exception handling)
            Console.WriteLine("Calling DeleteItem()...");
            try
            {
                proxy.DeleteItem(Guid.NewGuid().ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.WriteLine();

            // get all items again
            Console.WriteLine("Calling GetItems()...");
            items = proxy.GetItems();
            foreach (TodoItem item in items)
                Console.WriteLine("{0}, {1}, {2}", item.Id, item.DueDate.ToShortDateString(), item.Title);
            Console.WriteLine();

            Console.ReadLine();
        }
    }
}
