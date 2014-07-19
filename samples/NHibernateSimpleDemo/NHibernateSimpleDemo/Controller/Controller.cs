using System;
using System.Collections.Generic;
using System.Text;

namespace NHibernateSimpleDemo
{
    class Controller
    {
        #region Declarations

        // Member variables
        PersistenceManager m_PersistenceManager = new PersistenceManager();

        // Property variables
        private OrderSystem p_OrderSystem = new OrderSystem();

        #endregion

        #region Constructor

        public Controller()
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// The order system for the application.
        /// </summary>
        public OrderSystem OrderSystem
        {
            get { return p_OrderSystem; }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Clears the business object properties of the p_OrderSystem object.
        /// </summary>
        /// <param name="p_OrderSystem">The OrderSytem object for the application.</param>
        public void ClearBusinessObjects()
        {
            p_OrderSystem.Orders.Clear();
            p_OrderSystem.Customers.Clear();
            p_OrderSystem.Catalog.Clear();
        }

        /// <summary>
        /// Clears all records from the database.
        /// </summary>
        /// <remarks>We use this method to reset the database at the beginning or each run.</remarks>
        public void ClearDatabase()
        {
            m_PersistenceManager.ClearDatabase();
        }

        /// <summary>
        /// Converts an ICollection of dictionary keys to a string array.
        /// </summary>
        /// <param name="keys">The ICollection of keys to convert.</param>
        /// <returns>A string array of keys.</returns>
        public string[] ConvertKeys(ICollection<string> keys)
        {
            int i = 0;
            string[] keyArray = new string[keys.Count];
            foreach (string key in keys)
            {
                keyArray[i] = key;
                i++;
            }
            return keyArray;
        }

        /// <summary>
        /// Creates a new business model.
        /// </summary>
        public void CreateBusinessObjects()
        {
            p_OrderSystem.Populate();
        }

        /// <summary>
        /// Loads objects from the application database.
        /// </summary>
        /// <param name="p_OrderSystem">The OrderSytem object for the application.</param>
        /// <param name="m_PersistenceManager">The PersistenceManager for the application.</param>
        public void LoadBusinessObjects()
        {
            // Load objects
            p_OrderSystem.Catalog = m_PersistenceManager.RetrieveAll<Product>(SessionAction.BeginAndEnd);
            p_OrderSystem.Customers = m_PersistenceManager.RetrieveAll<Customer>(SessionAction.Begin);
            p_OrderSystem.Orders = m_PersistenceManager.RetrieveAll<Order>(SessionAction.End);

            // Notify the OrderSystem that it has been loaded
            p_OrderSystem.NotifyLoadComplete();
        }

        /// <summary>
        /// Performs a deletion test on the order system.
        /// </summary>
        /// <param name="p_OrderSystem">The OrderSytem object for the application.</param>
        public void PerformDeletionTest()
        {
            // Delete Customer Able Inc. from database (should also delete Order #0)
            m_PersistenceManager.Delete<Customer>(p_OrderSystem.Customers[0]);

            /* At this point, we should have two customers, Baker Inc. and
             * Charlie Inc. We should also have two orders; Able Inc's
             * order should have been deleted. */

            // Clear business objects, then reload from database
            ClearBusinessObjects();
            LoadBusinessObjects();

        }

        /// <summary>
        /// Saves business objects to the database
        /// </summary>
        /// <param name="p_OrderSystem">The OrderSytem object for the application.</param>
        /// <param name="m_PersistenceManager">The PersistenceManager for the application.</param>
        public void SaveBusinessObjects()
        {
            /* Note that we don't have to save the Orders list. All of the 
            * orders in the list are also in Customer.Orders lists, so when 
            * we save Customer objects, we are also saving Order objects. */

            // Save Products
            foreach (Product product in p_OrderSystem.Catalog)
            {
                m_PersistenceManager.Save<Product>(product);
            }

            // Save Customers (also saves Orders)
            foreach (Customer customer in p_OrderSystem.Customers)
            {
                m_PersistenceManager.Save<Customer>(customer);
            }
        }

        /// <summary>
        /// Performs an object comparison to show that only one instance of an object is loaded.
        /// </summary>
        /// <param name="p_OrderSystem">The OrderSytem object for the application.</param>
        public void ShowObjectComparisons()
        {
            // Write header
            Console.WriteLine("Object Comparison:");
            Console.WriteLine("------------------");

            // Compare Customer #1 to the Order #1 customer--should be equal
            Customer customerA = p_OrderSystem.Customers[0];
            Customer customerB = p_OrderSystem.Orders[0].Customer;
            bool sameObject = object.ReferenceEquals(customerA, customerB);

            // Write Customer result
            Console.WriteLine("Customer[0] equals Order[0].Customer: {0}", sameObject.ToString());
            Console.WriteLine();

            // Compare Order #1 to the Customer #1 order--should be equal
            Order orderA = p_OrderSystem.Orders[0];
            Order orderB = p_OrderSystem.Customers[0].Orders[0];
            sameObject = object.ReferenceEquals(customerA, customerB);

            // Write Order result
            Console.WriteLine("Order[0] equals Customer[0].Order[0]: {0}", sameObject.ToString());
            Console.WriteLine();
        }

        #endregion
    }
}
