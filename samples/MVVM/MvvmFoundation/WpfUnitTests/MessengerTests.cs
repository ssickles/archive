using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MvvmFoundation.Wpf;

namespace WpfUnitTests
{
    [TestClass]
    public class MessengerTests
    {
        public MessengerTests()
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
        public void MessageWithoutParameterIsReceived()
        {
            Messenger target = new Messenger();

            bool received1 = false;
            bool received2 = false;

            target.Register("MESSAGE", (Action)(() => received1 = true));
            target.Register("MESSAGE", (Action)(() => received2 = true));

            target.NotifyColleagues("MESSAGE");

            Assert.IsTrue(received1);
            Assert.IsTrue(received2);
        }

        [TestMethod]
        public void MessageWithParameterIsReceived()
        {
            Messenger target = new Messenger();

            bool received = false;

            string paramValue = "whatever";

            target.Register("MESSAGE", (Action<string>)(param => received = (param == paramValue)));

            target.NotifyColleagues("MESSAGE", paramValue);

            Assert.IsTrue(received);
        }

        [TestMethod]
        public void ColleaguesAreNotifiedInRegistrationOrder()
        {
            Messenger target = new Messenger();

            int notificationCounter = 0;
            int notified1 = 0;
            int notified2 = 0;

            target.Register("MESSAGE", (Action)(() => notified1 = ++notificationCounter));
            target.Register("MESSAGE", (Action)(() => notified2 = ++notificationCounter));

            target.NotifyColleagues("MESSAGE");

            Assert.AreEqual(1, notified1 );
            Assert.AreEqual(2, notified2);
        }

        [TestMethod]
        [ExpectedException(typeof(System.Reflection.TargetParameterCountException))]
        public void ExceptionIsThrownIfParameterIsMissing()
        {
            Messenger target = new Messenger();

            target.Register("MESSAGE", (Action<string>)(param => Debug.Write(param)));

            target.NotifyColleagues("MESSAGE");
        }

        [TestMethod]
        [ExpectedException(typeof(System.Reflection.TargetParameterCountException))]
        public void ExceptionIsThrownIfUnexpectedParameterIsPassed()
        {
            Messenger target = new Messenger();
            
            target.Register("MESSAGE", () => Debug.WriteLine("No parameter expected"));

            target.NotifyColleagues("MESSAGE", "Unexpected parameter");
        }

#if DEBUG
        [TestMethod]
        [ExpectedException(typeof(System.Reflection.TargetParameterCountException))]
        public void ExceptionIsThrownIfRegisterMixedParameterNoParameter()
        {
            Messenger target = new Messenger();

            target.Register<string>("MESSAGE", param => Debug.WriteLine(param));
            target.Register("MESSAGE", () => Debug.WriteLine("No parameter"));

        }

        [TestMethod]
        [ExpectedException(typeof(System.Reflection.TargetParameterCountException))]
        public void ExceptionIsThrownIfRegisterMixedNoParameterParameter()
        {
            Messenger target = new Messenger();

            target.Register("MESSAGE", () => Debug.WriteLine("No parameter"));
            target.Register<string>("MESSAGE", param => Debug.WriteLine(param));

        }
#endif

        [TestMethod]
        public void StaticCallbackMethodIsInvoked()
        {
            Messenger target = new Messenger();

            _wasStaticCallbackMethodInvoked = false;

            target.Register("MESSAGE", (Action)StaticCallbackMethod);

            target.NotifyColleagues("MESSAGE");

            Assert.IsTrue(_wasStaticCallbackMethodInvoked);
        }

        static bool _wasStaticCallbackMethodInvoked;
        static void StaticCallbackMethod()
        {
            _wasStaticCallbackMethodInvoked = true;
        }
    }
}