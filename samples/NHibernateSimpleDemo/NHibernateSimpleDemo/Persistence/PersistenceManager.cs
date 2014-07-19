using System;
using System.Collections.Generic;
using System.Text;
using NHibernate;
using NHibernate.Cfg;
using System.Xml;
using System.Reflection;
using NHibernate.Expression;
using System.Data.SqlClient;
using System.Collections.Specialized;

[assembly: log4net.Config.XmlConfigurator(Watch=true)]
namespace NHibernateSimpleDemo
{
    /// <summary>
    /// Specifies whether to begin a new session, continue an existing session, or end an existing session.
    /// </summary>
    public enum SessionAction { Begin, Continue, End, BeginAndEnd }

    public class PersistenceManager : IDisposable
    {
    	#region Declarations

    	// Member variables
        private ISessionFactory m_SessionFactory = null;
        private ISession m_Session = null;
    
    	#endregion

    	#region Constructor

        /// <summary>
        /// Default constructor.
        /// </summary>
        public PersistenceManager()
        {
            this.ConfigureLog4Net();
            this.ConfigureNHibernate();
        }

    	#endregion

        #region IDisposable Members

        public void  Dispose()
        {
 	        m_SessionFactory.Dispose();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Clears all records from all tables in the database
        /// </summary>
        public void ClearDatabase()
        {
            // Initialize
            SqlConnection connection = m_SessionFactory.ConnectionProvider.GetConnection() as SqlConnection;
            SqlCommand command = null;
            string[] dataTables = new string[] { "OrderItems", "Products", "Orders", "Customers" };
            string sql = null;

            // Delete all records from all tables
            using (connection)
            {
                // Iterate tables
                for (int i = 0; i < dataTables.Length; i++)
                {
                    // Build query and command, and execute
                    sql = String.Format("Delete from {0}", dataTables[i]);
                    command = new SqlCommand(sql, connection);
                    command.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Close this Persistence Manager and release all resources (connection pools, etc). It is the responsibility of the application to ensure that there are no open Sessions before calling Close().
        /// </summary>
        public void Close()
		{
			m_SessionFactory.Close();
		}

        /// <summary>
        /// Deletes an object of a specified type.
        /// </summary>
        /// <param name="itemsToDelete">The items to delete.</param>
        /// <typeparam name="T">The type of objects to delete.</typeparam>
        public void Delete<T>(T item)
        {
            using (ISession session = m_SessionFactory.OpenSession())
            {
                using (session.BeginTransaction())
                {
                    session.Delete(item);
                    session.Transaction.Commit();
                }
            }
        }

        /// <summary>
        /// Deletes objects of a specified type.
        /// </summary>
        /// <param name="itemsToDelete">The items to delete.</param>
        /// <typeparam name="T">The type of objects to delete.</typeparam>
        public void Delete<T>(IList<T> itemsToDelete)
        {
            using (ISession session = m_SessionFactory.OpenSession())
            {
                foreach (T item in itemsToDelete)
                {
                    using (session.BeginTransaction())
                    {
                        session.Delete(item);
                        session.Transaction.Commit();
                    }
                }
            }
        }

        /// <summary>
        /// Retrieves all objects of a given type.
        /// </summary>
        /// <typeparam name="T">The type of the objects to be retrieved.</typeparam>
        /// <returns>A list of all objects of the specified type.</returns>
        public IList<T> RetrieveAll<T>(SessionAction sessionAction)
        {
            /* Note that NHibernate guarantees that two object references will point to the
             * same object only if the references are set in the same session. For example,
             * Order #123 under the Customer object Able Inc and Order #123 in the Orders
             * list will point to the same object only if we load Customers and Orders in 
             * the same session. If we load them in different sessions, then changes that
             * we make to Able Inc's Order #123 will not be reflected in Order #123 in the
             * Orders list, since the references point to different objects. That's why we
             * maintain a session as a member variable, instead of as a local variable. */
            
            // Open a new session if specified
            if ((sessionAction == SessionAction.Begin) || (sessionAction == SessionAction.BeginAndEnd))
            {
                m_Session = m_SessionFactory.OpenSession();
            }

            // Retrieve all objects of the type passed in
            ICriteria targetObjects = m_Session.CreateCriteria(typeof(T));
            IList<T> itemList = targetObjects.List<T>();

            // Close the session if specified
            if ((sessionAction == SessionAction.End) || (sessionAction == SessionAction.BeginAndEnd))
            {
                m_Session.Close();
                m_Session.Dispose();
            }

            // Set return value
            return itemList;
        }

        /// <summary>
        /// Retrieves objects of a specified type where a specified property equals a specified value.
        /// </summary>
        /// <typeparam name="T">The type of the objects to be retrieved.</typeparam>
        /// <param name="propertyName">The name of the property to be tested.</param>
        /// <param name="propertyValue">The value that the named property must hold.</param>
        /// <returns>A list of all objects meeting the specified criteria.</returns>
        public IList<T> RetrieveEquals<T>(string propertyName, object propertyValue)
        {
            using (ISession session = m_SessionFactory.OpenSession())
            {
                // Create a criteria object with the specified criteria
                ICriteria criteria = session.CreateCriteria(typeof(T));
                criteria.Add(Expression.Eq(propertyName, propertyValue));

                // Get the matching objects
                IList<T> matchingObjects = criteria.List<T>();

                // Set return value
                return matchingObjects;
            }
        }

        /// <summary>
        /// Saves an object and its persistent children.
        /// </summary>
        public void Save<T>(T item)
        {
            using (ISession session = m_SessionFactory.OpenSession())
            {
                using (session.BeginTransaction())
                {
                    session.SaveOrUpdate(item);
                    session.Transaction.Commit();
                }
            }
        }

        #endregion

    	#region Private Methods

        /// <summary>
        /// Configures Log4Net to work with NHibernate.
        /// </summary>
        private void ConfigureLog4Net()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        /// <summary>
        /// Configures NHibernate and creates a member-level session factory.
        /// </summary>
        private void ConfigureNHibernate()
        {
            // Initialize
            Configuration cfg = new Configuration();
            cfg.Configure();

            /* Note: The AddAssembly() method requires that mappings be 
             * contained in hbm.xml files whose BuildAction properties 
             * are set to ‘Embedded Resource’. */

            // Add class mappings to configuration object
            Assembly thisAssembly = typeof(Customer).Assembly;
            cfg.AddAssembly(thisAssembly);

            // Create session factory from configuration object
            m_SessionFactory = cfg.BuildSessionFactory();
        }

    	#endregion
    }
}
