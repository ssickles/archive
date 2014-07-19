using System;
using System.Collections.Generic;
using System.Text;

namespace NHibernateSimpleDemo
{
    /// <summary>
    /// An order placed by a customer.
    /// </summary>
    public class Order
    {
        #region Declarations

    	// Property variables
        private Customer p_Customer = null;
        private DateTime p_Date = DateTime.Today;
        private int p_ID = -1;
        private IList<Product> p_OrderItems = new List<Product>();

    	// Member variables
    
    	#endregion

    	#region Constructor

        public Order()
        {
        }

        #endregion

    	#region Properties

        /// <summary>
        /// The customer who placed the order.
        /// </summary>
        public virtual Customer Customer
        {
            get { return p_Customer; }
            set { p_Customer = value; }
        }

        /// <summary>
        /// The date of the order.
        /// </summary>
         public virtual DateTime Date
        {
            get { return p_Date; }
            set { p_Date = value; }
        }

        /// <summary>
        /// The order number of the order.
        /// </summary>
        /// <value>This value is assigned by the system.</value>
        public virtual int ID
        {
            get { return p_ID; }
            set { p_ID = value; }
        }

        /// <summary>
        /// The products purchased in this order.
        /// </summary>
        public virtual IList<Product> OrderItems
        {
            get { return p_OrderItems; }
            set { p_OrderItems = value; }
        }

    	#endregion

        #region Method Overrides

        public override string ToString()
        {
            string orderID = null;
            if (p_ID == -1)
            {
                orderID = "Unsaved Order";
            }
            else
            {
                orderID = String.Format("Order #{0}", p_ID.ToString());
            }

            return String.Format("{0} {1}, {2}", p_Customer.Name, orderID, p_Date.ToShortDateString());
        }

        #endregion
    }
}
