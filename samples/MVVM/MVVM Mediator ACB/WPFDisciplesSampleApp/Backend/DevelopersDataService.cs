using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WPFDisciples.Backend
{
    public class DevelopersDataService
    {
        /// <summary>
        /// Gets the list of Developers from the database
        /// </summary>
        /// <returns>Return the list of devs. Return null if there is an error</returns>
        public static IEnumerable<Developers> GetDevelopers()
        {
            try
            {
                using (UniblueLabsEntities entities = new UniblueLabsEntities())
                {
                    return (from x in entities.Developers
                            select x).ToList();
                }
            }
            catch { return null; }
        }
    }
}
