using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceLocatorSample.DataAccess;

namespace ServiceLocatorSample.DesignTimeData
{
    internal class DesignTimePeopleDataAccess : IPeopleDataAccess
    {
        public IEnumerable<Person> GetAllPersons()
        {
            return new List<Person>
            {
                new Person { Name = "Design Name", Surname = "Design Surname", Age=24}
            };
        }
    }
}