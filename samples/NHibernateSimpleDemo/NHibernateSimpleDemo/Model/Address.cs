using System;
using System.Collections.Generic;
using System.Text;

namespace NHibernateSimpleDemo
{
    /// <summary>
    /// The customer's address.
    /// </summary>
    public class Address
    {
        #region Declarations

    	// Property variables
        private string p_City = String.Empty;
        private string p_State = String.Empty;
        private string p_StreetAddress = String.Empty;
        private string p_Zip = string.Empty;

    	// Member variables
    
    	#endregion

    	#region Constructor

    	#endregion

    	#region Properties

        /// <summary>
        /// The city portion of the customer's address.
        /// </summary>
        public virtual string City
        {
            get { return p_City; }
            set { p_City = value; }
        }

        /// <summary>
        /// The street address portion of the customer's address.
        /// </summary>
        public virtual string StreetAddress
        {
            get { return p_StreetAddress; }
            set { p_StreetAddress = value; }
        }

        /// <summary>
        /// The state portion of the customer's address.
        /// </summary>
        public virtual string State
        {
            get { return p_State;}
            set { p_State = value;}
        }

        /// <summary>
        /// The zip code portion of the customer's address.
        /// </summary>
        public virtual string Zip
        {
            get { return p_Zip; }
            set { p_Zip = value; }
        }

    	#endregion

    	#region Methods

    	#endregion
    }
}
