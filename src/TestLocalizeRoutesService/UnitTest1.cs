using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IHomer.Services.LocalizeRoutes;
using IHomer.Services.LocalizeRoutes.Entities;
using System.Globalization;

namespace TestResourceService
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class UnitTest1
    {
        public UnitTest1()
        {
            //
            // TODO: Add constructor logic here
            //
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
        public void TestSplitCamelCase()
        {
            Console.WriteLine(Resource.SplitResourceCode("casedWordHTTPWriter"));
            Console.WriteLine(Resource.SplitResourceCode("nothing"));
            Console.WriteLine(Resource.SplitResourceCode("ABBA"));
            Console.WriteLine(Resource.SplitResourceCode("AnABBA_CD"));
            Console.WriteLine(Resource.SplitResourceCode("Computer"));
            Console.WriteLine(Resource.SplitResourceCode("aComputer"));
            Console.WriteLine(Resource.SplitResourceCode("anAppleComputer"));
            Console.WriteLine(Resource.SplitResourceCode("anIBM"));
            Console.WriteLine(Resource.SplitResourceCode("anIBMComputer"));
            Console.WriteLine(Resource.SplitResourceCode("AppleComputer"));
            Console.WriteLine(Resource.SplitResourceCode("IBMComputer"));
            Console.WriteLine(Resource.SplitResourceCode("One123Two789"));
            Console.WriteLine(Resource.SplitResourceCode("One_123Two_789"));
            Console.WriteLine(Resource.SplitResourceCode("HTTPWriter"));
            Console.WriteLine(Resource.SplitResourceCode("HTTPWriter2"));
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void TestUpperRoute()
        {
            Console.WriteLine(Resource.GetUpperRoute("Deelnemer"));
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void TestResourceProvider()
        {
            var home = new RouteResourceProvider("/");
            var dateEn = home.GetObject("Date", new CultureInfo("en"));
            var dateNl = home.GetObject("Date", new CultureInfo("nl"));
            Assert.AreEqual(dateEn, dateNl);
        }

    }
}
