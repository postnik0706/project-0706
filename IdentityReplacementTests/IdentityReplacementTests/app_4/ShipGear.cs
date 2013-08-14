#region Copyright (c)2013 V-Technologies, LLC. All rights reserved.
/*
  The software and associated documentation supplied hereunder are property of V-Technologies, LLC.
  except where noted and/or subject to other licenses.
*/
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using System.ComponentModel.DataAnnotations.Schema;
using VTechnologies.ShipGear.Entities;

namespace VTechnologies.ShipGear.Entities
{
    public class Shipment
    {
        private int _shipmentId;
        public int ShipmentId
        {
            get
            {
                if (ShipmentId_ == null || ShipmentId_ == Guid.Empty)
                {
                    ShipmentId_ = Guid.NewGuid();
                }
                return _shipmentId;
            }
            set
            {
                _shipmentId = value;
            }
        }
        public int SourceId { get; set; }///
        public Source Source { get; set; }
        public int CompanyId { get; set; }///
        public SourceCompany Company { get; set; }

        public int NumberOfPackages { get; set; }///
        public bool RateRuleApplied { get; set; }
        public DateTime? ShipDate { get; set; }///
        [MaxLength(15)]
        public string Workstation { get; set; } ///            // 15 characters - OS requirement - NetBIOS.
        public bool Voided { get; set; }///
        [MaxLength(35)]
        public string Reference1 { get; set; }///               // Carrier. Reference2 in case of UPS. FXGPONbr in case of FedEx. UPS=35, FDX=30.
        [MaxLength(35)]
        public string Reference2 { get; set; }///
        [MaxLength(35)]
        public string Reference3 { get; set; }///UPS-not assigned
        [MaxLength(35)]
        public string Reference4 { get; set; }///UPS-not assigned
        [MaxLength(35)]
        public string Reference5 { get; set; }///UPS-not assigned
        [MaxLength(30)]
        public string TrackingNumber { get; set; }///          // Carrier. UPS=18, FDX=30.

        public int CarrierId { get; set; }///
        public EnumValue CarrierEnum { get; set; }
        [NotMapped]
        public Carrier Carrier
        {
            get
            {
                return CarrierId.EnumFromDb<Carrier>();
            }
            set
            {
                CarrierId = typeof(Carrier).GetEnumTypeId() * Constants.MAX_ENUM_MEMBERS + (int)value;
            }
        }

        public int ServiceId { get; set; }///
        public EnumValue ServiceEnum { get; set; }
        [NotMapped]
        public ServiceType Service
        {
            get
            {
                return ServiceId.EnumFromDb<ServiceType>();
            }
            set
            {
                ServiceId = typeof(ServiceType).GetEnumTypeId() * Constants.MAX_ENUM_MEMBERS + (int)value;
            }
        }

        public int PaymentTypeId { get; set; }
        public EnumValue PaymentTypeEnum { get; set; }
        [NotMapped]
        public PaymentType PaymentType /// UPS-not assigned
        {
            get
            {
                return PaymentTypeId.EnumFromDb<PaymentType>();
            }
            set
            {
                PaymentTypeId = typeof(PaymentType).GetEnumTypeId() * Constants.MAX_ENUM_MEMBERS + (int)value;
            }
        }
        [MaxLength(128)]
        public string Zone { get; set; }

        public DocumentInfo DocumentInfo { get; set; }
        public International International { get; set; }

        public int? SenderId { get; set; }
        public Address Sender { get; set; }

        public int? RecipientId { get; set; }
        public Address Recipient { get; set; }

        [MaxLength(64)]
        public string SGID { get; set; }
        [MaxLength(64)]
        public string SG_ID { get; set; }
        public DateTime? EstDeliveryDate { get; set; }           // UPS only. eNotify addition. Uses UPS online tools.

        public Guid ShipmentId_ { get; set; }

        public List<Package> Packages { get; set; }

        public ShipmentOptions ShipmentOptions { get; set; }

        public int? TotalsId { get; set; }
        public Totals Totals { get; set; }

        [MaxLength(400)]
        public string SpecialInstructions { get; set; }
        public decimal BilledWeight { get; set; }
        public Address ThirdPartyAddress { get; set; }
        [MaxLength(5)]
        public string SCAC { get; set; }

        public int BillToId { get; set; }
        public EnumValue BillToEnum { get; set; }
        [NotMapped]
        public BillingType BillTo
        {
            get
            {
                return BillToId.EnumFromDb<BillingType>();
            }
            set
            {
                BillToId = typeof(BillingType).GetEnumTypeId() * Constants.MAX_ENUM_MEMBERS + (int)value;
            }
        }

        [MaxLength(128)]
        public string ServiceName { get; set; }
        [MaxLength(128)]
        public string DepartmentCode { get; set; }
        [MaxLength(128)]
        public string DepartmentNotes { get; set; }

        public Rate Rate { get; set; }

        public List<UserDefined> UserDefined { get; set; }

        public int SourceInterfaceId { get; set; }
        public SourceInterface SourceInterface { get; set; }
    }

    public class DocumentInfo
    {
        [Key, ForeignKey("Shipment")]
        public int Id { get; set; }
        public Shipment Shipment { get; set; }

        public int SourceId { get; set; }
        [MaxLength(15)]
        public string Document { get; set; }            // Document Type from FS. Can be 'Sales Orders', 'Customers', 'Invoice'.
        [MaxLength(22)]
        public string DocumentKey { get; set; }         // Document Key from FS, i.e. Order Number or Invoice Number. PT/SG=20, MAS=7, QB=11
        [MaxLength(22)]
        public string DocumentKey2 { get; set; }        // For Peachtree / Sage 50, fills in part from Document Key separated with '@'.
        [MaxLength(62)]
        public string FSCompanyName { get; set; }       // Financial system company name. PT/SG=39, MAS=50, QB=59
        public bool AddedNotes { get; set; }
        public bool AddedFreight { get; set; }
        [MaxLength(4000)]
        public string NotesText { get; set; }           // Financial system, line item notes in ordes or invoices. MAS=2048, PT/SG(Customer Note)=2000, QB=4095
        [MaxLength(32)]
        public string OrderNumber { get; set; }
        [MaxLength(32)]
        public string InvoiceNumber { get; set; }
        [MaxLength(32)]
        public string PONumber { get; set; }
        public DateTime? OrderDate { get; set; }
    }

    public class International
    {
        [Key, ForeignKey("Shipment")]
        public int Id { get; set; }
        public Shipment Shipment { get; set; }

        public int BillTransportationToId { get; set; }
        public EnumValue BillTransportationToEnum { get; set; }
        [NotMapped]
        public BillingType BillTransportationTo
        {
            get
            {
                return BillTransportationToId.EnumFromDb<BillingType>();
            }
            set
            {
                BillTransportationToId = typeof(BillingType).GetEnumTypeId() * Constants.MAX_ENUM_MEMBERS + (int)value;
            }
        }

        public int BillDutyAndTaxToId { get; set; }
        public EnumValue BillDutyAndTaxToEnum { get; set; }
        [NotMapped]
        public BillingType BillDutyAndTaxTo
        {
            get
            {
                return BillDutyAndTaxToId.EnumFromDb<BillingType>();
            }
            set
            {
                BillDutyAndTaxToId = typeof(BillingType).GetEnumTypeId() * Constants.MAX_ENUM_MEMBERS + (int)value;
            }
        }

        public bool SplitDutyAndTax { get; set; }

        // Exchange Collect
        public bool ExchangeCollect { get; set; }
        [MaxLength(50)]
        public string BuyerEmailAddress { get; set; }
        [MaxLength(50)]
        public string SellerEmailAddress { get; set; }
        public decimal ExchangeCollectAmount { get; set; }

        public int ExchangeCollectCurrencyTypeId { get; set; }
        public EnumValue ExchangeCollectCurrencyTypeEnum { get; set; }
        [NotMapped]
        public ExchangeCollectCurrencyType ExchangeCollectCurrency
        {
            get
            {
                return ExchangeCollectCurrencyTypeId.EnumFromDb<ExchangeCollectCurrencyType>();
            }
            set
            {
                ExchangeCollectCurrencyTypeId = typeof(ExchangeCollectCurrencyType).GetEnumTypeId() * Constants.MAX_ENUM_MEMBERS + (int)value;
            }
        }

        public decimal? BuyersPercentOfServiceFee { get; set; }
        [MaxLength(512)]
        public string DescriptionOfGoods { get; set; }
        [MaxLength(50)]
        public string AdditionalComments { get; set; }

        public bool DocumentsOnly { get; set; }
    }

    public class Address
    {
        public int Id { get; set; }
        [MaxLength(35)]
        public string Code { get; set; }                // UPS: 30. FedEx: Recipient = 35, Sender = 15.
        [MaxLength(10)]
        public string AccountNumber { get; set; }       // UPS: 10. FedEx: 9.
        [MaxLength(14)]
        public string Phone { get; set; }
        [MaxLength(14)]
        public string Fax { get; set; }
        [MaxLength(120)]
        public string Email { get; set; }               // FedEx: 120. UPS: 50.
        [MaxLength(35)]
        public string Contact { get; set; }             // FedEx: 35. UPS: 35.
        [MaxLength(35)]
        public string Company { get; set; }             // FEdEx: 35, UPS: 35.              // Financial system. UPS/ShipToCompany. FDX/RecipientCompany. MAS=30, PT/SG=39, QB=41.
        [MaxLength(35)]
        public string AddrLine1 { get; set; }           // FedEx: 35, UPS: 35.
        [MaxLength(35)]
        public string AddrLine2 { get; set; }           // same
        [MaxLength(35)]
        public string AddrLine3 { get; set; }           // same
        [MaxLength(35)]
        public string City { get; set; }                // UPS: 30, FedEx: 35.
        [MaxLength(2)]
        public string State { get; set; }
        [MaxLength(15)]
        public string Zip { get; set; }                 // UPS: 9, FedEx: 15.
        [MaxLength(3)]
        public string Country { get; set; }             // UPS: 3, FedEx: 2.
        public bool Residential { get; set; }
        [MaxLength(12)]
        public string TaxId { get; set; }

        public int TaxIdTypeId { get; set; }
        public EnumValue TaxIdTypeEnum { get; set; }
        [NotMapped]
        public TaxIdType TaxIdType
        {
            get
            {
                return TaxIdTypeId.EnumFromDb<TaxIdType>();
            }
            set
            {
                TaxIdTypeId = typeof(TaxIdType).GetEnumTypeId() * Constants.MAX_ENUM_MEMBERS + (int)value;
            }
        }
        [MaxLength(64)]
        public string LocationId { get; set; }
    }

    public class Notification
    {
        public int Id { get; set; }

        [MaxLength(35)]
        public string FromName { get; set; }                // UPS: 35.
        [MaxLength(20)]
        public string Subject { get; set; }                 // UPS: 20.
        [MaxLength(150)]
        public string ShipMemo { get; set; }                // FedEx: 120. UPS: 150.
        [MaxLength(150)]
        public string DeliveryMemo { get; set; }                // FedEx: 120. UPS: 150.
        [MaxLength(160)]
        public string FailedEmailAddress { get; set; }

        public List<NotificationRecipient> Recipients { get; set; }
    }

    public class NotificationRecipient
    {
        public int Id { get; set; }

        [MaxLength(128)]
        public string CompanyOrName { get; set; }
        public int RecipientTypeId { get; set; }
        public EnumValue RecipientTypeEnum { get; set; }
        [NotMapped]
        public NotificationRecipientType RecipientType
        {
            get
            {
                return RecipientTypeId.EnumFromDb<NotificationRecipientType>();
            }
            set
            {
                RecipientTypeId = typeof(NotificationRecipientType).GetEnumTypeId() * Constants.MAX_ENUM_MEMBERS + (int)value;
            }
        }

        public int NotificationMethodTypeId { get; set; }
        public EnumValue NotificationMethodTypeEnum { get; set; }
        [NotMapped]
        public NotificationMethodType NotificationMethod
        {
            get
            {
                return NotificationMethodTypeId.EnumFromDb<NotificationMethodType>();
            }
            set
            {
                NotificationMethodTypeId = typeof(NotificationMethodType).GetEnumTypeId() * Constants.MAX_ENUM_MEMBERS + (int)value;
            }
        }

        public bool ShipNotification { get; set; }
        public bool DeliveryNotification { get; set; }
        public bool ExceptionNotification { get; set; }

        [MaxLength(120)]
        public string Email { get; set; }
        [MaxLength(14)]
        public string Phone { get; set; }
        [MaxLength(14)]
        public string Fax { get; set; }

        [MaxLength(128)]
        public string Contact { get; set; }
        [MaxLength(22)]
        public string InternationalPhoneFax { get; set; }

        public int Notificationid { get; set; }
        public Notification Notification { get; set; }
    }

    public class ShipmentOptions
    {
        [Key, ForeignKey("Shipment")]
        public int Id { get; set; }

        public Shipment Shipment { get; set; }

        // Return Service
        public bool? ReturnService { get; set; }
        public int ReturnServiceTypeId { get; set; }
        public EnumValue ReturnServiceTypeEnum { get; set; }
        public ReturnServiceType ReturnServiceType
        {
            get
            {
                return ReturnServiceTypeId.EnumFromDb<ReturnServiceType>();
            }
            set
            {
                ReturnServiceTypeId = typeof(ReturnServiceType).GetEnumTypeId() * Constants.MAX_ENUM_MEMBERS + (int)value;
            }
        }

        // Saturday Delivery
        public bool? SaturdayDelivery { get; set; }

        // Saturday Pickup
        public bool? SaturdayPickup { get; set; }

        // Notification Option
        public bool? NotificationOption { get; set; }

        // Delivery Confirmation
        public int SignatureOptionsId { get; set; }
        public EnumValue SignatureOptionsEnum { get; set; }
        [NotMapped]
        public SignatureOptionsType DeliveryConfirmationSignature
        {
            get
            {
                return SignatureOptionsId.EnumFromDb<SignatureOptionsType>();
            }
            set
            {
                SignatureOptionsId = typeof(SignatureOptionsType).GetEnumTypeId() * Constants.MAX_ENUM_MEMBERS + (int)value;
            }
        }

        public int? NotificationId { get; set; }
        public Notification Notification { get; set; }

        public int? UPSShipmentOptionsId { get; set; }
        public UPSShipmentOptions UPS { get; set; }

        public int? FedExShipmentOptionsId { get; set; }
        public FedExShipmentOptions FedEx { get; set; }

        public bool InsideDeliveryFlag { get; set; }
        public bool InsidePickupFlag { get; set; }

        public ShipmentHandlingCharge HandlingCharge { get; set; }
    }

    public class UPSShipmentOptions
    {
        public int Id { get; set; }

        // Carbon Neutral
        public bool? CarbonNeutral { get; set; }

        public int EndorsementTypeId { get; set; }
        public EnumValue EndorsementTypeEnum { get; set; }
        [NotMapped]
        public EndorsementType Endorsement
        {
            get
            {
                return EndorsementTypeId.EnumFromDb<EndorsementType>();
            }
            set
            {
                EndorsementTypeId = typeof(EndorsementType).GetEnumTypeId() * Constants.MAX_ENUM_MEMBERS + (int)value;
            }
        }

        public bool LTLIndicator { get; set; }
    }

    public class FedExShipmentOptions
    {
        public int Id { get; set; }

        [MaxLength(10)]
        public string SignatureReleaseNumber { get; set; }                  // FedEx only, = 10
    }

    public class Package
    {
        public int Id { get; set; }
        [MaxLength(30)]
        public string TrackingNumber { get; set; }                          // UPS = 30

        public int PackageTypeId { get; set; }
        public EnumValue PackageTypeEnum { get; set; }
        [NotMapped]
        public PackageType PackageType
        {
            get
            {
                return PackageTypeId.EnumFromDb<PackageType>();
            }
            set
            {
                PackageTypeId = typeof(PackageType).GetEnumTypeId() * Constants.MAX_ENUM_MEMBERS + (int)value;
            }
        }

        public decimal PackageWeight { get; set; }
        public decimal ActualWeight { get; set; }
        public int WeightUOMId { get; set; }
        public EnumValue WeightUOMEnum { get; set; }
        [NotMapped]
        public WeightUOMType WeightUOM
        {
            get
            {
                return WeightUOMId.EnumFromDb<WeightUOMType>();
            }
            set
            {
                WeightUOMId = typeof(WeightUOMType).GetEnumTypeId() * Constants.MAX_ENUM_MEMBERS + (int)value;
            }
        }

        [MaxLength(35)]
        public string Reference1 { get; set; }
        [MaxLength(35)]
        public string Reference2 { get; set; }
        [MaxLength(35)]
        public string Reference3 { get; set; }
        [MaxLength(35)]
        public string Reference4 { get; set; }
        [MaxLength(35)]
        public string Reference5 { get; set; }
        public decimal Height { get; set; }
        public decimal Width { get; set; }
        public decimal Length { get; set; }
        [MaxLength(755)]
        public string MerchandiseDescription { get; set; }

        public int DimUOMId { get; set; }
        public EnumValue DimUOMEnum { get; set; }
        [NotMapped]
        public DimUOMType DimUOM
        {
            get
            {
                return DimUOMId.EnumFromDb<DimUOMType>();
            }
            set
            {
                DimUOMId = typeof(DimUOMType).GetEnumTypeId() * Constants.MAX_ENUM_MEMBERS + (int)value;
            }
        }

        [MaxLength(40)]
        public string PackageContents1 { get; set; }
        [MaxLength(40)]
        public string PackageContents2 { get; set; }

        public int ShipmentId { get; set; }
        public Shipment Shipment { get; set; }

        public Rate Rate { get; set; }

        public int? PackageOptionId { get; set; }
        public PackageOptions PackageOptions { get; set; }

        public List<UserDefined> UserDefined { get; set; }
    }

    public class PackageOptions
    {
        public int Id { get; set; }

        public bool ResidentialDeliveryFlag { get; set; }
        public bool OverSizeFlag { get; set; }
        public bool BlockShipData { get; set; }
        public bool NonStandardContainer { get; set; }

        public HazMat HazMat { get; set; }

        public COD COD { get; set; }

        public bool HALFlag { get; set; }
        public int? HALAddressId { get; set; }
        public Address HALAddress { get; set; }

        public UPSPackageOptions UPSPackageOptions { get; set; }
        public FedExPackageOptions FedExPackageOptions { get; set; }

        public bool DeliveryConfirmation { get; set; }
        public int SignatureOptionsId { get; set; }
        public EnumValue SignatureOptionsEnum { get; set; }
        [NotMapped]
        public SignatureOptionsType Signature
        {
            get
            {
                return SignatureOptionsId.EnumFromDb<SignatureOptionsType>();
            }
            set
            {
                SignatureOptionsId = typeof(SignatureOptionsType).GetEnumTypeId() * Constants.MAX_ENUM_MEMBERS + (int)value;
            }
        }

        public bool InsuranceFlag { get; set; }
        public decimal? InsuranceAmount { get; set; }
        public int InsuranceTypeId { get; set; }
        public EnumValue InsuranceTypeEnum { get; set; }
        [NotMapped]
        public InsuranceType InsuranceType
        {
            get
            {
                return InsuranceTypeId.EnumFromDb<InsuranceType>();
            }
            set
            {
                InsuranceTypeId = typeof(InsuranceType).GetEnumTypeId() * Constants.MAX_ENUM_MEMBERS + (int)value;
            }
        }
        public bool? InsuranceAmountShipperPaid { get; set; }

        public bool DryIce { get; set; }
        public decimal DryIceWeight { get; set; }
        public int DryIceWeightUOMTypeId { get; set; }
        public EnumValue DryIceWeightUOMTypeEnum { get; set; }
        [NotMapped]
        public WeightUOMType DryIceWeightUoM
        {
            get
            {
                return DryIceWeightUOMTypeId.EnumFromDb<WeightUOMType>();
            }
            set
            {
                DryIceWeightUOMTypeId = typeof(WeightUOMType).GetEnumTypeId() * Constants.MAX_ENUM_MEMBERS + (int)value;
            }
        }

        // Notification Option
        public bool NotificationOption { get; set; }
        public Notification Notification { get; set; }

        // Handling Charge
        public PackageHandlingCharge HandlingCharge { get; set; }
    }

    public class ShipmentHandlingCharge
    {
        [Key, ForeignKey("ShipmentOptions")]
        public int Id { get; set; }
        public ShipmentOptions ShipmentOptions { get; set; }

        public int HandlingChargeTypeId { get; set; }
        public EnumValue HandlingChargeTypeEnum { get; set; }
        [NotMapped]
        public HandlingChargeType HandlingChargeType
        {
            get
            {
                return HandlingChargeTypeId.EnumFromDb<HandlingChargeType>();
            }
            set
            {
                HandlingChargeTypeId = typeof(HandlingChargeType).GetEnumTypeId() * Constants.MAX_ENUM_MEMBERS + (int)value;
            }
        }

        public int VariableHandlingChargeTypeId { get; set; }
        public EnumValue VariableHandlingChargeTypeEnum { get; set; }
        [NotMapped]
        public VariableHandlingChargeType VariableHandlingChargeType
        {
            get
            {
                return VariableHandlingChargeTypeId.EnumFromDb<VariableHandlingChargeType>();
            }
            set
            {
                VariableHandlingChargeTypeId = typeof(VariableHandlingChargeType).GetEnumTypeId() * Constants.MAX_ENUM_MEMBERS + (int)value;
            }
        }

        public int AmountIncreaseDecreaseTypeId { get; set; }
        public EnumValue AmountIncreaseDecreaseTypeEnum { get; set; }
        [NotMapped]
        public AmountIncreaseDecreaseType AmountIncreaseDecreaseType
        {
            get
            {
                return AmountIncreaseDecreaseTypeId.EnumFromDb<AmountIncreaseDecreaseType>();
            }
            set
            {
                AmountIncreaseDecreaseTypeId = typeof(AmountIncreaseDecreaseType).GetEnumTypeId() * Constants.MAX_ENUM_MEMBERS + (int)value;
            }
        }

        public decimal FlatRate { get; set; }
        public decimal Percentage { get; set; }
    }

    public class COD
    {
        [Key, ForeignKey("PackageOptions")]
        public int Id { get; set; }
        public PackageOptions PackageOptions { get; set; }

        public int CollectionTypeId { get; set; }
        public EnumValue CollectionTypeEnum { get; set; }
        [NotMapped]
        public CODCollectionType CollectionType
        {
            get
            {
                return CollectionTypeId.EnumFromDb<CODCollectionType>();
            }
            set
            {
                CollectionTypeId = typeof(CODCollectionType).GetEnumTypeId() * Constants.MAX_ENUM_MEMBERS + (int)value;
            }
        }

        public int ChargeTypeId { get; set; }
        public EnumValue ChargeTypeEnum { get; set; }
        [NotMapped]
        public CODChargeType ChargeType
        {
            get
            {
                return ChargeTypeId.EnumFromDb<CODChargeType>();
            }
            set
            {
                ChargeTypeId = typeof(CODChargeType).GetEnumTypeId() * Constants.MAX_ENUM_MEMBERS + (int)value;
            }
        }

        public decimal Amount { get; set; }
        [MaxLength(128)]
        public string Reference { get; set; }                   // FDX - CODGroundReference
        public bool ECOD { get; set; }
        public bool AddShippingCharges { get; set; }

        public int? RemittanceId { get; set; }
        public Address Remittance { get; set; }
    }

    public class HazMat
    {
        [Key, ForeignKey("PackageOptions")]
        public int Id { get; set; }
        public PackageOptions PackageOptions { get; set; }

        [MaxLength(128)]
        public string Code { get; set; }
        [MaxLength(128)]
        public string DOTShippingNameOfMaterial1 { get; set; }
        [MaxLength(128)]
        public string DOTShippingNameOfMaterial2 { get; set; }
        [MaxLength(128)]
        public string DOTShippingNameOfMaterial3 { get; set; }
        [MaxLength(128)]
        public string ClassOrDivisionNumber { get; set; }
        [MaxLength(128)]
        public string IdentificationNumber { get; set; }
        [MaxLength(128)]
        public string PackingGroup { get; set; }
        public float Weight { get; set; }

        public int WeightUOMTypeId { get; set; }
        public EnumValue WeightUOMEnum { get; set; }
        [NotMapped]
        public WeightUOMType WeightUOM
        {
            get
            {
                return WeightUOMTypeId.EnumFromDb<WeightUOMType>();
            }
            set
            {
                WeightUOMTypeId = typeof(WeightUOMType).GetEnumTypeId() * Constants.MAX_ENUM_MEMBERS + (int)value;
            }
        }

        [MaxLength(128)]
        public string TypeDOTLabel { get; set; }
    }

    public class UPSPackageOptions
    {
        public int Id { get; set; }
        public bool AdditionalHandling { get; set; }

        public bool ShipperRelease { get; set; }

        public bool VerbalConfirmation { get; set; }
        [MaxLength(128)]
        public string VerbalConfirmationName { get; set; }
        [MaxLength(14)]
        public string VerbalConfirmationPhone { get; set; }

        public bool USPSDeliveryConfirmation { get; set; }
    }

    public class PackageHandlingCharge
    {
        [Key, ForeignKey("PackageOptions")]
        public int Id { get; set; }
        public PackageOptions PackageOptions { get; set; }

        public int HandlingChargeTypeId { get; set; }
        public EnumValue HandlingChargeTypeEnum { get; set; }
        [NotMapped]
        public HandlingChargeType HandlingChargeType
        {
            get
            {
                return HandlingChargeTypeId.EnumFromDb<HandlingChargeType>();
            }
            set
            {
                HandlingChargeTypeId = typeof(HandlingChargeType).GetEnumTypeId() * Constants.MAX_ENUM_MEMBERS + (int)value;
            }
        }

        public int VariableHandlingChargeTypeId { get; set; }
        public EnumValue VariableHandlingChargeTypeEnum { get; set; }
        [NotMapped]
        public VariableHandlingChargeType VariableHandlingChargeType
        {
            get
            {
                return VariableHandlingChargeTypeId.EnumFromDb<VariableHandlingChargeType>();
            }
            set
            {
                VariableHandlingChargeTypeId = typeof(VariableHandlingChargeType).GetEnumTypeId() * Constants.MAX_ENUM_MEMBERS + (int)value;
            }
        }

        public int AmountIncreaseDecreaseTypeId { get; set; }
        public EnumValue AmountIncreaseDecreaseTypeEnum { get; set; }
        [NotMapped]
        public AmountIncreaseDecreaseType AmountIncreaseDecreaseType
        {
            get
            {
                return AmountIncreaseDecreaseTypeId.EnumFromDb<AmountIncreaseDecreaseType>();
            }
            set
            {
                AmountIncreaseDecreaseTypeId = typeof(AmountIncreaseDecreaseType).GetEnumTypeId() * Constants.MAX_ENUM_MEMBERS + (int)value;
            }
        }

        public decimal FlatRate { get; set; }
        public decimal Percentage { get; set; }
    }

    public class FedExPackageOptions
    {
        public int Id { get; set; }

        public bool AlcoholFlag { get; set; }
        public int AlcoholPackages { get; set; }
        public int AlcoholQuantity { get; set; }
        public decimal AlcoholVolume { get; set; }

        public int AlcoholPackagingTypeId { get; set; }
        public EnumValue AlcoholPackagingTypeEnum { get; set; }
        [NotMapped]
        public AlcoholPackagingType AlcoholPackagingType
        {
            get
            {
                return AlcoholPackagingTypeId.EnumFromDb<AlcoholPackagingType>();
            }
            set
            {
                AlcoholPackagingTypeId = typeof(AlcoholPackagingType).GetEnumTypeId() * Constants.MAX_ENUM_MEMBERS + (int)value;
            }
        }

        public int AlcoholTypeId { get; set; }
        public EnumValue AlcoholTypeEnum { get; set; }
        [NotMapped]
        public AlcoholType AlcoholType
        {
            get
            {
                return AlcoholTypeId.EnumFromDb<AlcoholType>();
            }
            set
            {
                AlcoholTypeId = typeof(AlcoholType).GetEnumTypeId() * Constants.MAX_ENUM_MEMBERS + (int)value;
            }
        }

        public bool HomeDeliveryFlag { get; set; }
        public int HomeDeliveryTypeId { get; set; }
        public EnumValue HomeDeliveryTypeEnum { get; set; }
        [NotMapped]
        public HomeDeliveryType HomeDeliveryType
        {
            get
            {
                return HomeDeliveryTypeId.EnumFromDb<HomeDeliveryType>();
            }
            set
            {
                HomeDeliveryTypeId = typeof(HomeDeliveryType).GetEnumTypeId() * Constants.MAX_ENUM_MEMBERS + (int)value;
            }
        }

        [MaxLength(14)]
        public string HomeDeliveryPhone { get; set; }
        public DateTime? HomeDeliveryDeliveryDate { get; set; }
        [MaxLength(14)]
        public string HomeDeliveryAlternativePhoneNumber { get; set; }
        [MaxLength(1024)]
        public string HomeDeliveryInstructions { get; set; }
    }

    public class Rate
    {
        public int Id { get; set; }

        public ChargeList ListCharges { get; set; }
        public ChargeList CustomCharges { get; set; }
        public ChargeList AppliedCharges { get; set; }

        public bool IsPackage { get; set; }

        public Package Package { get; set; }

        public Shipment Shipment { get; set; }
    }

    public class ChargeList
    {
        public int Id { get; set; }

        public Rate Rate { get; set; }
        public int? RateId { get; set; }

        public List<Charge> Charges { get; set; }
    }

    public class Charge
    {
        public int Id { get; set; }

        public int ChargeListId { get; set; }
        public ChargeList ChargeList { get; set; }

        public decimal Amount { get; set; }
        [MaxLength(4000)]
        public string Description { get; set; }

        public int ChargeKindId { get; set; }
        public EnumValue ChargeKindEnum { get; set; }
        [NotMapped]
        public ChargeKind ChargeKind
        {
            get
            {
                return ChargeKindId.EnumFromDb<ChargeKind>();
            }
            set
            {
                ChargeKindId = typeof(ChargeKind).GetEnumTypeId() * Constants.MAX_ENUM_MEMBERS + (int)value;
            }
        }

        public int ChargeTypeId { get; set; }
        public EnumValue ChargeTypeEnum { get; set; }
        [NotMapped]
        public ChargeType ChargeType
        {
            get
            {
                return ChargeTypeId.EnumFromDb<ChargeType>();
            }
            set
            {
                ChargeTypeId = typeof(ChargeType).GetEnumTypeId() * Constants.MAX_ENUM_MEMBERS + (int)value;
            }
        }
    }

    public class Totals : ICloneable
    {
        public int Id { get; set; }

        public decimal TotalActualFreight { get; set; }
        public decimal TotalAppliedFreight { get; set; }
        public bool FreightRuleDisabled { get; set; }
        [MaxLength]
        public string FieldValues { get; set; }
        [MaxLength]
        public string RuleText { get; set; }

        public int? ShipmentId { get; set; }
        public Shipment Shipment { get; set; }

        public object Clone()
        {
            return new Totals()
            {
                TotalActualFreight = this.TotalActualFreight,
                TotalAppliedFreight = this.TotalAppliedFreight,
                FreightRuleDisabled = this.FreightRuleDisabled,
                FieldValues = this.FieldValues,
                RuleText = this.RuleText
            };
        }
    }

    public class UpdateShipmentParameters
    {
        public bool AddedFreight_set { get; set; }
        public bool AddedFreight { get; set; }

        public bool AddedNotes_set { get; set; }
        public bool AddedNotes { get; set; }
        public string NotesText { get; set; }

        public bool Voided_set { get; set; }
        public bool Voided { get; set; }
    }

    // UDFs
    public class UserFieldDefinition
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

    public class UserDefined
    {
        public int Id { get; set; }

        public string UserFieldDefinitionId { get; set; }
        public UserFieldDefinition UserFieldDefinition { get; set; }

        public int? ShipmentId { get; set; }
        public Shipment Shipment { get; set; }

        public int? PackageId { get; set; }
        public Package Package { get; set; }

        [MaxLength]
        public string Value { get; set; }
    }




    ///
    /// XML files
    /// 


    /// <summary>
    /// FSI_Companies.xml
    /// 
    /// </summary>
    public class Source
    {
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        public List<SourceCompany> Companies { get; set; }
    }

    public class SourceCompany
    {
        public int Id { get; set; }

        public int SourceId { get; set; }
        public Source Source { get; set; }

        public string Name { get; set; }
        public bool Enabled { get; set; }
        public List<SourceCustomField> SourceFields { get; set; }
        public List<SourceDBConnectionInfo> ConnectionInfo { get; set; }
    }

    public class SourceInterface : ICloneable
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public int SourceId { get; set; }
        public Source Source { get; set; }

        [MaxLength(64)]
        public string Name { get; set; }
        public bool Enabled { get; set; }
        [MaxLength(8)]
        public string Version { get; set; }

        public List<SourceCompany> Companies { get; set; }   // Ignored in EF; is used to pass list of companies when process FS_Interface.xml, then update FsiCompany.
        // List of companies will update FsiCompany with corresponding FsiInterfaceId.
        // Ignored because FsiCompanies.xml and FsInterface.xml process separately.
        // Cannot populate FsiInterface before FsiCompanies - that would be required if FsiInterface was principal to FsiCompanies due to List<FsiCompanies> field.
        // Cannot populate because the table also references FsiId which means FsiCompanies must be populated first.

        public object Clone()
        {
            return new SourceInterface()
            {
                SourceId = this.SourceId,
                Name = this.Name,
                Enabled = this.Enabled,
                Version = this.Version
            };
        }
    }

    public class SourceCompanyConnection : ICloneable
    {
        public int Id { get; set; }

        public Workstation Workstation { get; set; }
        public int? WorkstationId { get; set; }

        public Source Source { get; set; }
        public int? SourceId { get; set; }

        public SourceInterface SourceInterface { get; set; }
        public int? SourceInterfaceId { get; set; }

        public SourceCompany SourceCompany { get; set; }
        public int? SourceCompanyId { get; set; }

        public object Clone()
        {
            return new SourceCompanyConnection()
            {
                WorkstationId = this.WorkstationId,
                SourceId = this.SourceId,
                SourceInterfaceId = this.SourceInterfaceId,
                SourceCompanyId = this.SourceCompanyId
            };
        }
    }

    public class SourceDBConnectionInfo
    {
        public int Id { get; set; }

        public int SourceId { get; set; }
        public int SourceCompanyId { get; set; }
        public SourceCompany SourceCompany { get; set; }

        public string UserId { get; set; }
        public string Password { get; set; }
        public string Provider { get; set; }
        public string InitialCatalog { get; set; }
        public string DataSource { get; set; }
        [MaxLength(128)]
        public string SourceSysServerName { get; set; }
        [MaxLength(128)]
        public string SourceSysDatabase { get; set; }
        [MaxLength(128)]
        public string SourceSysUserName { get; set; }
        [MaxLength(128)]
        public string SourceSysPassword { get; set; }
    }

    public class SourceCustomField
    {
        public int SourceId { get; set; }
        public int SourceCompanyId { get; set; }
        public SourceCompany SourceCompany { get; set; }

        public int Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public bool HasValueList { get; set; }
        public string GroupName { get; set; }
        public bool IsNumeric { get; set; }
        public bool PossibleEmailField { get; set; }

        public List<SourceValue> ValueList { get; set; }
    }

    public class SourceDocumentInterface
    {
        public int Id { get; set; }
        [MaxLength(128)]
        public string Type { get; set; }

        public int? SourceId { get; set; }
        public int SourceInterface_Id { get; set; }
        public SourceInterface SourceInterface { get; set; }

        public int CarrierTypeId { get; set; }
        public EnumValue CarrierTypeEnum { get; set; }
        [NotMapped]
        public Carrier SourceCarrier
        {
            get
            {
                return CarrierTypeId.EnumFromDb<Carrier>();
            }
            set
            {
                CarrierTypeId = typeof(Carrier).GetEnumTypeId() * Constants.MAX_ENUM_MEMBERS + (int)value;
            }
        }

        public List<SourceFieldMap> SourceFieldMap { get; set; }

        [MaxLength(128)]
        public string Item1 { get; set; }
        [MaxLength(128)]
        public string Item2 { get; set; }
        [MaxLength(128)]
        public string Item3 { get; set; }

        public List<SourceWriteBackInfo> SourceWritebackInfo { get; set; }
    }

    public class SourceWriteBackInfo
    {
        public int Id { get; set; }

        public bool WritebackFreightEnabled { get; set; }
        public bool WritebackFreightIsInline { get; set; }
        public bool WritebackNotesEnabled { get; set; }
        [MaxLength(4000)]
        public string WritebackNotesNoteType { get; set; }
        public List<SourceValue> OtherWritebackInfo { get; set; }
        public List<SourceWritebackNote> Notes { get; set; }
        public bool? WritebackNotesNotesAfterItems { get; set; }

        public int? SourceId { get; set; }
        public int? SourceCompany_Id { get; set; }
        public SourceCompany SourceCompany { get; set; }
    }

    public class SourceFieldMap
    {
        public int Id { get; set; }

        [MaxLength(128)]
        public string CarrierFieldName { get; set; }
        [MaxLength(128)]
        public string MapType { get; set; }
        [MaxLength(128)]
        public string Target { get; set; }

        public int SourceDocumentInterfaceId { get; set; }
        public SourceDocumentInterface SourceDocumentInterface { get; set; }

        public List<SourceValueTranslation> ValueTranslation { get; set; }
    }

    public class SourceValueTranslation
    {
        public int Id { get; set; }

        public int SourceFieldMapId { get; set; }
        public SourceFieldMap SourceFieldMap { get; set; }

        [MaxLength(256)]
        public string SourceFieldValue { get; set; }
        [MaxLength(256)]
        public string ShipGearFieldValue { get; set; }
    }

    public class SourceWritebackNote
    {
        public int Id { get; set; }
        [MaxLength(128)]
        public string Tag { get; set; }
        [MaxLength(128)]
        public string CarrierFieldName { get; set; }
    }




    /// <summary>
    /// FSI_ValueList.xml
    /// </summary>

    public class SourceField
    {
        public int Id { get; set; }

        public int SourceId { get; set; }
        public Source Source { get; set; }

        public int SourceId1 { get; set; }
        public int SourceInterfaceId { get; set; }
        public SourceInterface SourceInterface { get; set; }

        [MaxLength(128)]
        public string SourceFieldName { get; set; }
        [MaxLength(128)]
        public string Type { get; set; }

        public List<SourceValue> SourceValues { get; set; }
    }

    public class SourceValue
    {
        public int Id { get; set; }
        [MaxLength(128)]
        public string Name { get; set; }
        [MaxLength(256)]
        public string Value { get; set; }
    }

    /// <summary>
    /// UPSWorldShipFieldValues.XML, FedExShipManagerFieldValues.xml
    /// </summary>
    public class CarrierFieldName : ICloneable
    {
        public int Id { get; set; }

        public Workstation Workstation { get; set; }
        public int? WorkstationId { get; set; }

        [MaxLength(64)]
        public string Name { get; set; }

        public int CarrierId { get; set; }
        public EnumValue CarrierEnum { get; set; }
        [NotMapped]
        public Carrier Carrier
        {
            get
            {
                return CarrierId.EnumFromDb<Carrier>();
            }
            set
            {
                CarrierId = typeof(Carrier).GetEnumTypeId() * Constants.MAX_ENUM_MEMBERS + (int)value;
            }
        }
        public List<CarrierFieldValue> CarrierFieldValues { get; set; }

        public object Clone()
        {
            CarrierFieldName ret = new CarrierFieldName()
            {
                Carrier = this.Carrier,
                Name = this.Name,
                Workstation = this.Workstation != null ? (Workstation)this.Workstation.Clone() : null,
                CarrierFieldValues = new List<CarrierFieldValue>()
            };

            foreach (CarrierFieldValue item in this.CarrierFieldValues)
            {
                ret.CarrierFieldValues.Add((CarrierFieldValue)item.Clone());
            }
            return ret;
        }
    }

    public class CarrierFieldValue : ICloneable
    {
        public int Id { get; set; }
        [MaxLength(1024)]
        public string Value { get; set; }

        public int CarrierFieldName_Id { get; set; }
        public CarrierFieldName CarrierFieldName { get; set; }

        public object Clone()
        {
            return new CarrierFieldValue()
            {
                Value = this.Value
            };
        }
    }


    ///
    /// Workstation List
    ///
    public class Workstation : ICloneable
    {
        public int Id { get; set; }
        [MaxLength(128)]
        public string Name { get; set; }
        public DateTime DateAdded { get; set; }

        public object Clone()
        {
            return new Workstation()
            {
                Name = this.Name,
                DateAdded = this.DateAdded
            };
        }
    }


    ///
    /// UpdatesApplied
    ///
    public class UpdateApplied
    {
        public int InternalID { get; set; }

        [MaxLength(50)]
        public string Version { get; set; }
        [MaxLength(200)]
        public string Description { get; set; }
        [MaxLength(50)]
        public string ObjectType { get; set; }
        [MaxLength(50)]
        public string UpdateType { get; set; }
        public DateTime? ProcessDateTime { get; set; }
        public DateTime? UpdateDateTime { get; set; }
        public bool? Applied { get; set; }
        [MaxLength]
        public string Body { get; set; }
        [MaxLength(3)]
        public string CEID { get; set; }
        public int? FsiId { get; set; }
        [MaxLength(50)]
        public string AccountId { get; set; }
        [MaxLength(100)]
        public string Hash { get; set; }
        [Timestamp]
        public Byte[] Tmsp { get; set; }
    }

    ///
    /// DataImport
    /// 
    public class DataImport
    {
        public int Id { get; set; }

        public DateTime ImportDate { get; set; }
        [MaxLength(128)]
        public string ImportLocation { get; set; }
        [MaxLength(64)]
        public string WorkstationName { get; set; }
    }




    /// <summary>
    /// FreightRules.xml
    /// </summary>

    public class FreightRules
    {
        public int Id { get; set; }
        public bool HasRuleOption { get; set; }

        public int? SourceId { get; set; }
        public Source Source { get; set; }
        //public DateTime LastModified { get; set; }

        public List<CalcRules> CalcRules { get; set; }
    }

    public class CalcRules
    {
        public int Id { get; set; }

        public int SourceId { get; set; }
        public int SourceInterfaceId { get; set; }
        public SourceInterface SourceInterface { get; set; }

        [MaxLength(64)]
        public string Carrier { get; set; }
        [MaxLength]
        public string Document { get; set; }
        [MaxLength]
        public string DefaultValue { get; set; }

        public int FreightRulesId { get; set; }
        public FreightRules FreightRules { get; set; }

        public List<Rule> Rule { get; set; }           // Optional
    }

    public class Rule
    {
        public int Id { get; set; }

        public int Sequence { get; set; }
        [MaxLength(128)]
        public string ResultType { get; set; }
        [MaxLength(256)]
        public string Result { get; set; }
        [MaxLength(256)]
        public string WhereType { get; set; }
        public bool StopIfMet { get; set; }

        public int CalcRulesId { get; set; }
        public CalcRules CalcRules { get; set; }

        public List<RuleWhere> Where { get; set; }     // Optional
    }

    public class RuleWhere
    {
        public int Id { get; set; }

        [MaxLength(128)]
        public string ValueSource { get; set; }
        [MaxLength(128)]
        public string Value { get; set; }
        [MaxLength(128)]
        public string ValueType { get; set; }
        [MaxLength(128)]
        public string Operator { get; set; }

        public int RuleId { get; set; }
        public Rule Rule { get; set; }

        public List<RuleValue> RuleValue { get; set; }
    }

    public class RuleValue
    {
        public int Id { get; set; }

        [MaxLength(256)]
        public string Value { get; set; }
    }


    public class Settings
    {
        public int Id { get; set; }
        public string Section { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }

        public int? WorkstationId { get; set; }
        public Workstation Workstation { get; set; }
    }

    public enum XmlFileType : int { XMLCompany = 0 };
}