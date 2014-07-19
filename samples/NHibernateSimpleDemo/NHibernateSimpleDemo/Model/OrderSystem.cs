using System;
using System.Collections.Generic;
using System.Text;
using NHibernate.Mapping;

namespace NHibernateSimpleDemo
{
    /// <summary>
    /// The order system for this application
    /// </summary>
    public class OrderSystem
    {
    	#region Declarations

        // Events
        public event EventHandler Populated;

    	// Property variables
        private IList<Product> p_Catalog = new List<Product>();
        private IList<Customer> p_Customers = new List<Customer>();
        private IList<Order> p_Orders = new List<Order>();

        // Member variables
        private Random m_RandomNumberGenerator = new Random(DateTime.Now.Millisecond);

    	#endregion

    	#region Constructor

    	#endregion

    	#region Properties


        /// <summary>
        /// The seller's product catalog.
        /// </summary>
        public IList<Product> Catalog
        {
            get { return p_Catalog; }
            set { p_Catalog = value; }
        }

        /// <summary>
        /// The list of customers in the system.
        /// </summary>
        public IList<Customer> Customers
        {
            get { return p_Customers; }
            set { p_Customers = value; }
        }

        /// <summary>
        /// The list of orders in the system.
        /// </summary>
        public IList<Order> Orders
        {
            get { return p_Orders; }
            set { p_Orders = value; }
        }

    	#endregion

    	#region Public Methods

        /// <summary>
        /// Notifies the rest of the application that the order system has been loaded.
        /// </summary>
        public void NotifyLoadComplete()
        {
            // Fire event
            this.FirePopulatedEvent();
        }

        /// <summary>
        /// Populates the order system with business objects.
        /// </summary>
        public void Populate()
        {
            // Create business objects
            this.CreateProducts();
            this.CreateCustomers();
            this.CreateOrders();

            // Fire event
            this.FirePopulatedEvent();
        }

    	#endregion

        #region Event Invokers

        /// <summary>
        /// Invokes the Populated event.
        /// </summary>
        internal void FirePopulatedEvent()
        {
            if (this.Populated != null)
            {
                this.Populated(this, new EventArgs());
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Adds three different customers to the customer list.
        /// </summary>
        /// <param name="m_OrderSystem">The OrderSytem object for the application.</param>
        private void CreateCustomers()
        {
            // Create data array
            string[,] customerData = new string[,]
            {
                {"Able, Inc.", "100 Main Street", "Chicago", "IL", "60601"},
                {"Baker, Inc.", "200 Main Street", "Chicago", "IL", "60601"},
                {"Charlie, Inc.", "100 Main Street", "Chicago", "IL", "60601"},
            };

            // Create customer list
            Customer newCustomer = null;
            for (int i = 0; i < 3; i++)
            {
                // Create new customer
                newCustomer = new Customer();

                // Set name and address
                newCustomer.Name = customerData[i, 0];
                newCustomer.Address.StreetAddress = customerData[i, 1];
                newCustomer.Address.City = customerData[i, 2];
                newCustomer.Address.State = customerData[i, 3];
                newCustomer.Address.Zip = customerData[i, 4];

                // Add customer to list
                p_Customers.Add(newCustomer);
            }
        }

        /// <summary>
        /// Add two orders for each customer
        /// </summary>
        /// <param name="m_OrderSystem">The OrderSytem object for the application.</param>
        private void CreateOrders()
        {
            // Initialize
            Customer customer = null;
            Order order = null;

            // Iterate customers
            for (int i = 0; i < 3; i++)
            {
                // Create one new order for each customer
                customer = p_Customers[i];
                order = new Order();
                order.Date = DateTime.Today;
                order.Customer = customer;

                // Add four products to the order
                order.OrderItems = this.GetRandomProducts();

                // Add the order to the Customer object 
                p_Customers[i].Orders.Add(order);

                // Add the order to the Orders list
                p_Orders.Add(order);
            }
        }

        /// <summary>
        /// Adds ten different products to the product catalog.
        /// </summary>
        /// <param name="m_OrderSystem">The OrderSytem object for the application.</param>
        private void CreateProducts()
        {
            // Create data array
            string[] productName = new string[10];
            for (int i = 0; i < 10; i++)
            {
                productName[i] = String.Format("Widget, Type {0:D2}", i.ToString());
            }

            // Create product list
            Product newProduct = null;
            for (int i = 0; i < 10; i++)
            {
                // Set product properties
                newProduct = new Product();
                newProduct.Name = productName[i];

                // Add product to catalog
                p_Catalog.Add(newProduct);
            }
        }

        /// <summary>
        /// Gets four random product from the product list, with no duplications.
        /// </summary>
        /// <param name="products">The Products list from the order system.</param>
        /// <returns></returns>
        public IList<Product> GetRandomProducts()
        {
            /* To ensure uniqueness, we first create a list of product IDs.
             * Then we generate a random number to pull an ID from the list,
             * adter which we delete the ID from the list. That way, even if 
             * the same random number is generated twice, the product ID
             * selected will be unique. */

            // Initialize 
            int productID = -1;
            IList<Product> results = new List<Product>();

            // Create a list of product IDs
            List<int> productIdList = new List<int>();
            for (int i = 0; i < 10; i++)
            {
                productIdList.Add(i);
            }

            // Grab four products at random
            for (int i = 0; i < 4; i++)
            {
                // Get a random number
                int n = m_RandomNumberGenerator.Next(productIdList.Count - 1);

                // Get a product ID, then delete it
                productID = productIdList[n];
                productIdList.RemoveAt(n);

                // And add the product to the results list
                results.Add(p_Catalog[productID]);
            }

            // Set return value
            return results;
        }

        #endregion
    }
}
