using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Xml.Serialization;
using OldShipment;
using System.IO;
using System.Diagnostics;
using System.Reflection;

namespace tests
{
    internal class Mother
    {
        public static ShipGearHistory GetTestHistoryRecord()
        {
            return new ShipGearHistory()
            {

            };
        }
    }
    
    [TestClass]
    public class xml
    {
        [TestMethod]
        public void SaveXml()
        {
            XmlSerializer s = new XmlSerializer(typeof(ShipGearHistory), "");
            ShipGearHistory h = Mother.GetTestHistoryRecord();

            StringBuilder sb = new StringBuilder();
            using (TextWriter tr = new StringWriter(sb))
            {
                s.Serialize(tr, h);
            }
            Debug.WriteLine(sb.ToString());
        }

        [TestMethod]
        public void LoadXml()
        {
            XmlSerializer s = new XmlSerializer(typeof(ShipGearHistory), "");

            Stream sb = Assembly.GetExecutingAssembly().GetManifestResourceStream("tests.xml.ShipGearHistory.xml");

            ShipGearHistory h = (ShipGearHistory)s.Deserialize(sb);

            Assert.IsNotNull(h);
            Assert.AreEqual("1ZT788870200001193", h.ShipmentID);
            Assert.AreEqual("Create", h.LastAction);
            Assert.AreEqual("SG90101O_0001174", h.SGID);
            Assert.AreEqual(false, h.VoidFlag);
            Assert.AreEqual(90, h.FSI_ID);
            Assert.AreEqual(101, h.CompanyID);
            Assert.AreEqual("Sales Orders", h.Document);
            Assert.AreEqual("0001174", h.DocumentKey);
            Assert.AreEqual("", h.DocumentKey2);
            Assert.AreEqual(1, h.NumberOfPackages);
            Assert.AreEqual("", h.RateRuleApplied);
            Assert.AreEqual(65.95M, h.FreightCharges);
            Assert.AreEqual(new DateTime(2013, 2, 14, 0, 1, 2), h.ShipDate);
            Assert.AreEqual("Irvine Warehouse", h.CustomerName);
            Assert.AreEqual("(ABC) ABC Distribution and Service Corp.", h.FSCompanyName);
            Assert.AreEqual("DEV08", h.Workstation);
            Assert.AreEqual(true, h.AddedNotes);
            Assert.AreEqual(false, h.AddedFreight);
            Assert.AreEqual("this is a test", h.NotesText);
            Assert.AreEqual("UPS", h.Carrier);
            Assert.AreEqual("UPS2ndDayAir", h.Service);
            Assert.AreEqual("0001174", h.Reference);

            Assert.AreEqual("123", h.ShipmentInfo.SG_ID);
            Assert.AreEqual(true, h.ShipmentInfo.Voided);
        }
    }
}
