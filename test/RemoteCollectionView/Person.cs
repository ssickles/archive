using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IdentityStream.Data;
using System.Collections;

namespace RemoteCollectionViewApp
{
    public class Person
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public int Age { get; set; }

        private List<Person> People
        {
            get
            {
                List<Person> persons = new List<Person> {
                    new Person
                    { 
                        Age=32, 
                        City="Bangalore", 
                        Country="India", 
                        FirstName="Brij", 
                        LastName="Mohan"
                    },
                    new Person
                    { 
                        Age=32, 
                        City="Bangalore", 
                        Country="India", 
                        FirstName="Arun", 
                        LastName="Dayal"
                    },
                    new Person
                    { 
                        Age=38, 
                        City="Bangalore", 
                        Country="India", 
                        FirstName="Dave", 
                        LastName="Marchant"
                    },
                    new Person
                    { 
                        Age=38,
                        City="Northampton",
                        Country="United Kingdom", 
                        FirstName="Henryk", 
                        LastName="S"
                    },
                    new Person
                    { 
                        Age=40, 
                        City="Northampton", 
                        Country="United Kingdom", 
                        FirstName="Alton", 
                        LastName="B"
                    },
                    new Person
                    { 
                        Age=28, 
                        City="Birmingham",
                        Country="United Kingdom",
                        FirstName="Anup", 
                        LastName="J"
                    },
                    new Person
                    { 
                        Age=27,
                        City="Jamshedpur",
                        Country="India", 
                        FirstName="Sunita", 
                        LastName="Mohan"
                    },
                    new Person
                    { 
                        Age=2, 
                        City="Bangalore", 
                        Country="India", 
                        FirstName="Shristi", 
                        LastName="Dayal"
                    }
                };

                return persons;
            }
        }

        public List<Person> GetPersons()
        {
            return People;
        }

        #region IRemoteRepository Members

        public List<Person> Get(QueryObject Query, out int totalResults)
        {
            List<Person> people = People.Skip(Query.Offset).Take(Query.PageSize).ToList();
            totalResults = 8;
            return people;
        }

        #endregion
    }
}
