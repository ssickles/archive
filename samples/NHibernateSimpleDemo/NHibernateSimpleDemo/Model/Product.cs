using System;
using System.Collections.Generic;
using System.Text;

namespace NHibernateSimpleDemo
{
    /// <summary>
    /// A product from the seller's catalog.
    /// </summary>
    public class Product
    {
    	#region Declarations

    	// Property variables
        private int p_ID = -1;

    	// Member variables
        private string p_Name = String.Empty;
    
    	#endregion

    	#region Constructor

        public Product()
        {
        }

    	#endregion

    	#region Properties

        /// <summary>
        /// The ID of the product.
        /// </summary>
        public virtual int ID
        {
            get { return p_ID; }
            set { p_ID = value; }
        }

        /// <summary>
        /// The name of the product.
        /// </summary>
        public virtual string Name
        {
            get { return p_Name; }
            set { p_Name = value; }
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
