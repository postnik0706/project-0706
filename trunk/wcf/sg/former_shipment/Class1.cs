using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace OldShipment
{
    public enum WeightType { Unknown, LBS, KG };
    
    public class ShipmentInfo
    {
        [XmlAttribute]
        public string SG_ID { get; set; }
        [XmlAttribute(AttributeName = "Voided")]
        public string voided { get; set; }
        [XmlIgnore]
        public bool Voided { get { return bool.Parse(voided); } set { voided = value.ToString(); } }
        [XmlAttribute]
        public string LastAction { get; set; }
    }

    public class Package
    {
        [XmlAttribute(AttributeName = "SG_ID")]
        public string SG_ID { get; set; }
        [XmlAttribute(AttributeName = "Voided")]
        public string voided { get; set; }
        [XmlIgnore]
        public bool Voided { get { return bool.Parse(voided); } set { voided = value.ToString(); } }
        [XmlAttribute]
        public string LastAction { get; set; }
        [XmlAttribute]
        public string Carrier { get; set; }
        [XmlAttribute]
        public string TrackingNumber { get; set; }
        [XmlAttribute]
        public string PackageType { get; set; }
        [XmlAttribute]
        public decimal PackageWeight { get; set; }
        [XmlAttribute(AttributeName = "PackageWeightUM")]
        public string packageWeightUM { get; set; }
        [XmlIgnore]
        public WeightType PackageWeightUM { get { return (WeightType)Enum.Parse(typeof(WeightType), packageWeightUM); } set { packageWeightUM = value.ToString(); } }
        [XmlAttribute]
        public string Reference { get; set; }
        [XmlAttribute]
        public decimal DeclaredValue { get; set; }
        [XmlAttribute]
        public decimal DimHeight { get; set; }
        [XmlAttribute]
        public decimal DimWidth { get; set; }
        [XmlAttribute]
        public decimal DimLength { get; set; }
        [XmlAttribute]
        public string weightUOM { get; set; }
        [XmlIgnore]
        public WeightType WeightUOM { get { return (WeightType)Enum.Parse(typeof(WeightType), weightUOM); } set { weightUOM = value.ToString(); } }
        [XmlAttribute]
        public string dimUOM { get; set; }

    }

    [XmlRoot(ElementName="SHIPGEARHISTORY")]
    public class ShipGearHistory
    {
        [XmlAttribute]
        public string ShipmentID { get; set; }
        [XmlAttribute]
        public string LastAction { get; set; }
        [XmlAttribute]
        public string SGID { get; set; }
        [XmlAttribute(AttributeName = "VoidFlag")]
        public string voidFlag { get; set; }
        [XmlIgnore]
        public bool VoidFlag { get { return bool.Parse(voidFlag.ToLower()); } set { voidFlag = value.ToString(); } }
        [XmlAttribute]
        public int FSI_ID { get; set; }
        [XmlAttribute]
        public int CompanyID { get; set; }
        [XmlAttribute]
        public string Document { get; set; }
        [XmlAttribute]
        public string DocumentKey { get; set; }
        [XmlAttribute]
        public string DocumentKey2 { get; set; }
        [XmlAttribute]
        public int NumberOfPackages { get; set; }
        [XmlAttribute]
        public string RateRuleApplied { get; set; }
        [XmlAttribute]
        public decimal FreightCharges { get; set; }
        /*[XmlIgnore]
        public decimal FreightCharges { get { return decimal.Parse(freightCharges); } set { freightCharges = value.ToString(); } } */
        [XmlAttribute(AttributeName = "ShipDate")]
        public string shipDate { get; set; }
        [XmlIgnore]
        public DateTime ShipDate { get { return DateTime.Parse(shipDate); } set { shipDate = value.ToString(); } }
        [XmlAttribute]
        public string CustomerName { get; set; }
        [XmlAttribute]
        public string FSCompanyName { get; set; }
        [XmlAttribute]
        public string Workstation { get; set; }
        [XmlAttribute(AttributeName = "AddedNotes")]
        public string addedNotes { get; set; }
        [XmlIgnore]
        public bool AddedNotes { get { return bool.Parse(addedNotes); } set { addedNotes = value.ToString(); } }
        [XmlAttribute(AttributeName = "AddedFreight")]
        public string addedFreight { get; set; }
        [XmlIgnore]
        public bool AddedFreight { get { return bool.Parse(addedFreight); } set { addedFreight = value.ToString(); } }
        [XmlAttribute]
        public string NotesText { get; set; }
        [XmlAttribute]
        public string Carrier { get; set; }
        [XmlAttribute]
        public string Service { get; set; }
        [XmlAttribute]
        public string Reference { get; set; }

        [XmlElement(ElementName = "SHIPMENTINFO")]
        public ShipmentInfo ShipmentInfo { get; set; }

        [XmlElement(ElementName = "PACKAGE")]
        public Package Package { get; set; }
    }
}