using Microsoft.VisualStudio.TestTools.UnitTesting;
using MvvmFoundation.Wpf;

namespace WpfUnitTests
{
    [TestClass]
    public class PropertyObserverTests
    {
        public PropertyObserverTests()
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

        [TestMethod]
        public void RegisteredHandlersAreInvoked()
        {
            ObservableThing thing = new ObservableThing();
            PropertyObserver<ObservableThing> target = new PropertyObserver<ObservableThing>(thing);
            bool wasInvoked = false;
            target.RegisterHandler(t => t.SomeProperty, t => wasInvoked = (t == thing));

            thing.SomeProperty = "whatever";

            Assert.IsTrue(wasInvoked);
        }

        [TestMethod]
        public void UnregisteredHandlersAreNotInvoked()
        {
            ObservableThing thing = new ObservableThing();
            PropertyObserver<ObservableThing> target = new PropertyObserver<ObservableThing>(thing);
            bool wasInvoked = false;

            target.RegisterHandler(t => t.SomeProperty, t => wasInvoked = true);
            target.UnregisterHandler(t => t.SomeProperty);
            thing.SomeProperty = "whatever";

            Assert.IsFalse(wasInvoked);
        }

        class ObservableThing : ObservableObject
        {
            public string SomeProperty
            {
                get { return _someProperty; }
                set
                {
                    if (value == _someProperty)
                        return;

                    _someProperty = value;

                    base.RaisePropertyChanged("SomeProperty");
                }
            }

            string _someProperty;
        }
    }
}