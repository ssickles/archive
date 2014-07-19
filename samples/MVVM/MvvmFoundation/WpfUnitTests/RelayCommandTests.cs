using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MvvmFoundation.Wpf;

namespace WpfUnitTests
{
    [TestClass]
    public class RelayCommandTests
    {
        public RelayCommandTests()
        {
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void CommandExecutes()
        {
            bool executed = false;
            RelayCommand target = new RelayCommand(() => executed = true);
            target.Execute(null);
            Assert.IsTrue(executed);
        }

        [TestMethod]
        public void CanExecuteReturnsFalse()
        {
            RelayCommand target = new RelayCommand(() => Console.WriteLine(), () => false);
            bool result = target.CanExecute(null);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ReceiveCorrectParameter()
        {
            bool canExecuteGotParam = false;
            bool executeGotParam = false;
            
            string paramValue = "whatever";

            RelayCommand<string> target = new RelayCommand<string>(
                (param) => executeGotParam = (param == paramValue), 
                (param) => canExecuteGotParam = (param == paramValue));

            target.CanExecute(paramValue);
            target.Execute(paramValue);

            Assert.IsTrue(canExecuteGotParam);
            Assert.IsTrue(executeGotParam);
        }
    }
}