using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MvvmFoundation.Wpf;

namespace WpfUnitTests
{
    [TestClass]
    public class ObservableObjectTests
    {
        public ObservableObjectTests()
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

#if DEBUG
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void InvalidPropertyNameIsReportedInDebugMode()
        {
            ConcreteViewModel target = new ConcreteViewModel();
            target.PropertyThatThrows = "whatever";
        }
#else
        [TestMethod]
        public void InvalidPropertyNameIsNotReportedInReleaseMode()
        {
            ConcreteViewModel target = new ConcreteViewModel();
            target.PropertyThatThrows = "whatever";
        }
#endif

        [TestMethod]
        public void PropertyChangedIsRaised()
        {
            ConcreteViewModel target = new ConcreteViewModel();

            bool wasRaisedCorrectly = false;
            target.PropertyChanged += (sender, e) => wasRaisedCorrectly = e.PropertyName == "PropertyThatWorksCorrectly";
            target.PropertyThatWorksCorrectly = "whatever";

            Assert.IsTrue(wasRaisedCorrectly);
        }

        [TestMethod]
        public void PropertyChangedCanBeRaisedForAllPropertiesAtOnce()
        {
            ConcreteViewModel target = new ConcreteViewModel();

            target.RaisePropertyChangedForAllProperties();

            // If we made it this far, then an exception was not thrown during the 
            // property name verification process, so the test succeeded.
        }

        class ConcreteViewModel : ObservableObject
        {
            protected override bool ThrowOnInvalidPropertyName
            {
                get
                {
                    return true;
                }
            }

            public void RaisePropertyChangedForAllProperties()
            {
                base.RaisePropertyChanged(null);
            }

            public string PropertyThatThrows
            {
                get { return null; }
                set { base.RaisePropertyChanged("IncorrectName"); }
            }

            public string PropertyThatWorksCorrectly
            {
                get { return null; }
                set { base.RaisePropertyChanged("PropertyThatWorksCorrectly"); }
            }
        }
    }
}