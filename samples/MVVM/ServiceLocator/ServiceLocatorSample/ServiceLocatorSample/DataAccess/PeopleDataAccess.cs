using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServiceLocatorSample.DataAccess
{
    public class PeopleDataAccess : IPeopleDataAccess
    {
        public IEnumerable<Person> GetAllPersons()
        {
            return new List<Person>
            {
                new Person { Name = "Marlon", Surname = "Grech", Age=24}
            };
        }
    }
}
