using System;
using System.Collections.Generic;
using System.Text;

namespace NHibernateSimpleDemo
{
    /// <summary>
    /// A customer.
    /// </summary>
    public class Customer
    {
    	#region Declarations

        // Property variables
        private Address p_Address = new Address();
        private int p_ID = -1;
        private string p_Name = "[New Customer]";
        private IList<Order> p_Orders = new List<Order>();
    
    	#endregion

    	#region Constructor

        public Customer()
        {

        }

    	#endregion

    	#region Properties

        /// <summary>
        /// The customer's address.
        /// </summary>
        public virtual Address Address
        {
            get { return p_Address; }
            set { p_Address = value; }
        }

        /// <summary>
        /// The customer number of this customer
        /// </summary>
        /// <remarks>This value is assigned by the system.</remarks>
        public virtual int ID
        {
            get { return p_ID;}
            set { p_ID = value;}
        }

        /// <summary>
        /// The customer's name
        /// </summary>
        /// <remarks>For simplicity, we only use one name field, instead of first name, last name, and so on.</remarks>
        public virtual string Name
        {
            get { return p_Name; }
            set { p_Name = value; }
        }

        /// <summary>
        /// All orders placed by this customer.
        /// </summary>
        public virtual IList<Order> Orders
        {
            get { return p_Orders; }
            set { p_Orders = value; }
        }

        #endregion

    	#region Method Overrides

        public override string ToString()
        {
            return p_Name;
        }

    	#endregion
    }
}
