using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ActivatorClient;

namespace ActivatorClientTest
{
    [TestClass]
    public class utility
    {
        [TestCleanup]
        public void Cleanup()
        {
            CmdLine.Reset();
        }

        [TestMethod]
        public void CommandLineParameters_HelpRequired()
        {
            CmdLine.Load( new string[] {"/?"} );
            Assert.AreEqual(true, CmdLine.Parameters.HelpRequired);
        }

        [TestMethod]
        public void CommandLineParameters_All_Set_Up()
        {
            CmdLine.Load(new string[] { "/Bldg", "1", "/Activate" });
            Assert.AreEqual(1, CmdLine.Parameters.BuildingID);
            Assert.AreEqual(true, CmdLine.Parameters.IsToActivate);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException),
            "Not all the parameters were set up")]
        public void CommandLineParameters_LackOfOneMustFail()
        {
            CmdLine.Load(new string[] { "/Bldg", "1" });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException),
            "Not all the parameters were set up")]
        public void CommandLineParameters_NoAnyParameters()
        {
            CmdLine.Load(new string[] { "" });
        }
    }
}
