using System;
using System.Collections.Generic;
using System.Text;
using NHibernate;

namespace NHibernateSimpleDemo
{
    class Program
    {
        /// <summary>
        /// The main method for the program
        /// </summary>
        /// <remarks>Normally, a Controller class (or classes) would do most of the
        /// work that this method does. The Controller would receive requests from
        /// the UI and manipulate the business model to perform the requested action.</remarks>
        static void Main(string[] args)
        {
            /* Since the Program class represents the UI of the application, it has
             * very limited knowledge of the model--only enough to subscribe to the
             * Populate event. If we wanted to be very strict, we could route that
             * event through the Controller, which would subscribe to the model's
             * event, then fires one of its own to pass the event to the UI. That
             * approach would completely eliminate the UI's knowledge of the model. 
             * In that case, the OrderSystem object would not be a property of the
             * Controller, but a member variable instead. */

            // Initialize 
            Controller controller = new Controller();
            controller.OrderSystem.Populated +=new EventHandler(OrderSystem_Populated);

            // Pause to let the user examine console
            Pause("Completed NHibernate configuration\r\nNext step: Clear database from last run");

            // Clear the database
            controller.ClearDatabase();
            Pause("Completed clearing the database from last run\r\nNext step: Create business objects");

            // Get persistent objects
            controller.CreateBusinessObjects();
            Pause("Completed creating business objects\r\nNext step: Saving business objects to database");

            // Save order system
            controller.SaveBusinessObjects();
            Pause("Completed saving business objects\r\nNext step: Clear business objects from memory");

            // Clear business objects
            controller.ClearBusinessObjects();
            Pause("Completed clearing business objects from memory\r\nNext step: Reload business objects from database");

            // Reload objects
            controller.LoadBusinessObjects();
            Pause("Completed reloading business objects\r\nNext step: Object comparison");

            // Show object comparison
            controller.ShowObjectComparisons();
            Pause("Completed object comparisons\r\nNext step: Deletion test");

            // Perform deletion test
            Notify("Beginning deletion test");
            controller.PerformDeletionTest();
            Pause("Completed deletion test (should be 2 Customers)\r\nApplication run completed");
        }

        #region Event Handlers

        /// <summary>
        /// Prints a customer and order list when the order system is populated.
        /// </summary>
        static void OrderSystem_Populated(object sender, EventArgs e)
        {
            OrderSystem orderSystem = sender as OrderSystem;
            ShowCustomersAndOrders(orderSystem);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Outputs a message to the user on the console.
        /// </summary>
        /// <param name="message">The message to output.</param>
        private static void Notify(string message)
        {
            Console.WriteLine("{0}\r\n", message);
        }

        /// <summary>
        /// Pauses until the user hits Enter.
        /// </summary>
        private static void Pause(string prompt)
        {
            Console.WriteLine("\r\n{0}", prompt);
            Console.Write("\r\nHit the Enter key to continue");
            Console.ReadLine();
        }
        
        /// <summary>
        /// Prints a list of customers and their orders.
        /// </summary>
        /// <param name="m_OrderSystem">The OrderSytem object for the application.</param>
        private static void ShowCustomersAndOrders(OrderSystem orderSystem)
        {
            // Initialize
            Customer customer = null;
            Order order = null;

            // Print header
            Console.WriteLine("Customers and their orders:");
            Console.WriteLine("---------------------------------");

            // Print customers and orders
            for (int i = 0; i < orderSystem.Customers.Count; i++)
            {
                customer = orderSystem.Customers[i];
                Console.WriteLine("Customer #{0}: {1}", i.ToString(), customer.Name);
                for (int j = 0; j < customer.Orders.Count; j++)
                {
                    order = customer.Orders[j];
                    Console.WriteLine("-->Order dated {0}", order.Date);

                }

                for (int k = 0; k < order.OrderItems.Count; k++)
                {
                    Console.WriteLine("---->Order item #{0}: {1}", k.ToString(), order.OrderItems[k].Name);
                }
            }

            // Skip line
            Console.WriteLine();
        }

        #endregion
    }
}
