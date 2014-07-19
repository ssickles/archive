using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MVVMHelper.ViewModel;
using ServiceLocatorSample.DataAccess;
using System.Collections.ObjectModel;

namespace ServiceLocatorSample.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private IList<Person> people;
        public IList<Person> People
        {
            get
            {
                if (people == null)
                    LoadPeople();
                return people;
            }
        }

        private void LoadPeople()
        {
            people = new ObservableCollection<Person>();

            //get the data access via the service locator.
            var dataAccess = GetService<IPeopleDataAccess>();
            foreach (var item in dataAccess.GetAllPersons())
                people.Add(item);
        }
    }
}
