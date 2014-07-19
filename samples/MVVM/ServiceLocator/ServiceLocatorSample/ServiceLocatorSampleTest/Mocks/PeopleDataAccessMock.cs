using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceLocatorSample.DataAccess;

namespace ServiceLocatorSampleTest.Mocks
{
    public class PeopleDataAccessMock : IPeopleDataAccess
    {
        public const int PeopleCount = 1;
        public const string FirstPersonName = "Marlon";
        public const string FirstPersonSurname = "Grech";

        #region IPeopleDataAccess Members

        public IEnumerable<Person> GetAllPersons()
        {
            return new List<Person>
            {
                new Person { Name = FirstPersonName, Surname = FirstPersonSurname, Age=12 }
            };
        }

        #endregion
    }
}
