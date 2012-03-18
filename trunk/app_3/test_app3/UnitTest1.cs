using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Reflection;
using app_3;

namespace test_app3
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Contact_AsString_Test()
        {
            Contact c = new Contact() { DateOfBirth = DateTime.Parse("25-Aug-1971"), FirstName = "Test", LastName = "Me" };
            Assert.AreEqual(25, c.DateOfBirth.Day, "Day");
            Assert.AreEqual(8, c.DateOfBirth.Month, "Month");
            Assert.AreEqual(1971, c.DateOfBirth.Year);
            Assert.IsTrue(c.DateOfBirth.AddYears(35) < DateTime.Now);

            Assert.AreEqual(
@"Contact Test Me
	Email: 
	Phone: 
	DateOfBirth: 25/08/1971
	State: ", c.ToString(), "ToString");
        }
    }
}
