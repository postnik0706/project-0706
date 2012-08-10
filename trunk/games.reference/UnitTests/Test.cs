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
            Assert.AreEqual(26, games.reference.Program.GetAlphabet().Length);
        }
    }
}
