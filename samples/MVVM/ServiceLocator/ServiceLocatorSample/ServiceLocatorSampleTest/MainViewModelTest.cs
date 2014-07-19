using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServiceLocatorSample.ViewModels;
using ServiceLocatorSampleTest.Mocks;
using ServiceLocatorSample.DataAccess;

namespace ServiceLocatorSampleTest
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class MainViewModelTest
    {
        [TestMethod]
        public void TestPeopleLoadData()
        {
            MainViewModel viewModel = new MainViewModel();
            viewModel.ServiceLocator.RegisterService<IPeopleDataAccess>(new PeopleDataAccessMock());
            
            Assert.AreEqual(viewModel.People.Count, PeopleDataAccessMock.PeopleCount,
                "Invalid number of People returned");
            Assert.AreEqual(viewModel.People[0].Name, PeopleDataAccessMock.FirstPersonName,
                "Invalid item in people list");
            Assert.AreEqual(viewModel.People[0].Surname, PeopleDataAccessMock.FirstPersonSurname,
                "Invalid item in people list");

        }
    }
}
