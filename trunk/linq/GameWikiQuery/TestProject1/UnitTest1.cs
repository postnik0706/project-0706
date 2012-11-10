using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void GetAlphabet_checks()
        {
            Assert.AreEqual(26, app_1.Program.GetAlphabet().Length);
        }
    }
}
