using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LoadingAdorner
{
    /// <summary>
    /// Model for our Demo app
    /// </summary>
    public class DemoModel
    {
        #region Sample Data
        Person[] sampleData = new []
            {
                new Person { Name= "Marlon", Surname="Grech"},
                new Person { Name= "Jasmine", Surname="Grech"},
                new Person { Name= "Karl", Surname="Aguis"},
                new Person { Name= "Jon", Surname="Caruana"},
                new Person { Name= "Mario", Surname="Mizzi"},
                new Person { Name= "Raffaele", Surname="Bianco"},
                new Person { Name= "Josh", Surname="Smith"},
                new Person { Name= "Karl", Surname="Shiflett"},
                new Person { Name= "Sascha", Surname="Barber"},
                new Person { Name= "Dr", Surname="WPF"},
                new Person { Name= "Lester", Surname="Lobo"},
                new Person { Name= "Ema", Surname="Grech"}
            };
        #endregion

        public IList<Person> GetData()
        {
            return sampleData;
        }

        public IList<Person> GetDataByName(string name)
        {
            return (from x in sampleData
                       where x.Name.StartsWith(name, StringComparison.InvariantCultureIgnoreCase)
                       select x).ToList();
        }
    }
}
