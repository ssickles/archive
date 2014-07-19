using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;
using IdentityStream.Data;
using System.Linq;

namespace Silverlight3DataGrid
{
    public class Person : IRemoteRepository<Person, int>
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

        #region IRemoteRepository<Person,int> Members

        public Person GetById(int Id)
        {
            Person person = (from p in People
                             where p.Id == Id
                             select p).SingleOrDefault();
            return person;
        }

        public IList<Person> Get(QueryObject Query)
        {
            return People;
        }

        public void Insert(Person Entity)
        {
            People.Add(Entity);
        }

        public void Delete(Person Entity)
        {
            People.Remove(Entity);
        }

        #endregion
    }
}
