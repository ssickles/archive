using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServiceLocatorSample.DataAccess
{
    public interface IPeopleDataAccess
    {
        IEnumerable<Person> GetAllPersons();
    }
}
