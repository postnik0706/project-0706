using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace VTechnologies.ShipGear.Entities
{
    public class Constants
    {
        public const int MAX_ENUM_MEMBERS = 256;
    }

    [DbEnum(0)]
    public enum Carrier { [EnumName(Name = "Unknown")]Unknown, [EnumName(Name = "FedEx", Subtype = (int)Carrier.FDX)] FDX, [EnumName(Name = "UPS", Subtype = (int)Carrier.UPS)] UPS };
    [DbEnum(1)]
    public enum ServiceType
    {
        [EnumName(Name = "Unknown", Description = "Unknown")]
        Unknown,
        [EnumName(Name = "FedEx Priority Overnight®", Subtype = (int)Carrier.FDX)]
        FDXPriorityOvernight,
        [EnumName(Name = "FedEx Standard Overnight®", Subtype = (int)Carrier.FDX)]
        FDXStandardOvernight,
        [EnumName(Name = "FedEx 2Day®", Subtype = (int)Carrier.FDX)]
        FDX2Day,
        [EnumName(Name = "FedEx First Overnight®", Subtype = (int)Carrier.FDX)]
        FDXFirstOvernight,
        [EnumName(Name = "FedEx Express Saver®", Subtype = (int)Carrier.FDX)]
        FDXExpressSaver,
        [EnumName(Name = "FedEx Ground®", Subtype = (int)Carrier.FDX)]
        FDXGroundService,
        [EnumName(Name = "FedEx Home Delivery®", Subtype = (int)Carrier.FDX)]
        FDXHomeDelivery,
        [EnumName(Name = "FedEx 1Day® Freight", Subtype = (int)Carrier.FDX)]
        FDX1DayFreight,
        [EnumName(Name = "FedEx 2Day® Freight", Subtype = (int)Carrier.FDX)]
        FDX2DayFreight,
        [EnumName(Name = "FedEx 3Day® Freight", Subtype = (int)Carrier.FDX)]
        FDX3DayFreight,
        [EnumName(Name = "FedEx International Priority®", Subtype = (int)Carrier.FDX)]
        FDXIntlPriorityOvernight,
        [EnumName(Name = "FedEx International Economy®", Subtype = (int)Carrier.FDX)]
        FDXIntlEconomy,
        [EnumName(Name = "FedEx International Priority® Freight", Subtype = (int)Carrier.FDX)]
        FDXIntlPriorityFreight,
        [EnumName(Name = "FedEx International Economy® Freight", Subtype = (int)Carrier.FDX)]
        FDXIntlEconomyFreight,
        [EnumName(Name = "FedEx International First®", Subtype = (int)Carrier.FDX)]
        FDXIntlFirstOvernight,
        [EnumName(Name = "FedEx Ground®", Subtype = (int)Carrier.FDX)]
        FDXIntlGround,
        [EnumName(Name = "FedEx 2Day®A.M.", Subtype = (int)Carrier.FDX)]
        FDX2DayAM,

        [EnumName(Name = "Next Day Air", Subtype = (int)Carrier.UPS)]
        UPSNextDayAir,
        [EnumName(Name = "2nd Day Air", Subtype = (int)Carrier.UPS)]
        UPS2ndDayAir,
        [EnumName(Name = "Ground", Subtype = (int)Carrier.UPS)]
        UPSGround,
        [EnumName(Name = "Standard", Subtype = (int)Carrier.UPS)]
        UPSStandard,
        [EnumName(Name = "Worldwide Express", Subtype = (int)Carrier.UPS)]
        UPSWorldwideExpress,
        [EnumName(Name = "3 Day Select", Subtype = (int)Carrier.UPS)]
        UPS3DaySelect,
        [EnumName(Name = "Next Day Air Saver", Subtype = (int)Carrier.UPS)]
        UPSNextDayAirSaver,
        [EnumName(Name = "Worldwide Express Plus", Subtype = (int)Carrier.UPS)]
        UPSWorldwideExpressPlus,
        [EnumName(Name = "Worldwide Expedited", Subtype = (int)Carrier.UPS)]
        UPSWorldwideExpedited,
        [EnumName(Name = "Next Day Air Early AM", Subtype = (int)Carrier.UPS)]
        UPSNextDayAirEarlyAM,
        [EnumName(Name = "2nd Day Air AM", Subtype = (int)Carrier.UPS)]
        UPS2ndDayAirAM,
        [EnumName(Name = "WorldWide Saver", Subtype = (int)Carrier.UPS)]
        UPSWorldwideSaver,
        [EnumName(Name = "Freight LTL", Subtype = (int)Carrier.UPS)]
        UPSFreightLTL,
        [EnumName(Name = "Freight LTL-Guaranteed", Subtype = (int)Carrier.UPS)]
        UPSFreightLTLGuaranteed,
        [EnumName(Name = "Priority Mail Innovations", Subtype = (int)Carrier.UPS)]
        UPSPriorityMailInnovations,
        [EnumName(Name = "Economy Mail Innovations", Subtype = (int)Carrier.UPS)]
        UPSEconomyMailInnovations,
        [EnumName(Name = "First Class Mail", Subtype = (int)Carrier.UPS)]
        UPSFirstClassMail,
        [EnumName(Name = "Priority Mail", Subtype = (int)Carrier.UPS)]
        UPSPriorityMail,
        [EnumName(Name = "Expedited Mail Innovations", Subtype = (int)Carrier.UPS)]
        UPSMailInnovations,
        [EnumName(Name = "SurePost Less than 1 lb", Subtype = (int)Carrier.UPS)]
        UPSSurePostLessThan1L,
        [EnumName(Name = "SurePost 1 lb or Greater", Subtype = (int)Carrier.UPS)]
        UPSSurePost1LOrGreater,
        [EnumName(Name = "SurePost Bound Printed Matter", Subtype = (int)Carrier.UPS)]
        UPSSurePostBoundPrinted,
        [EnumName(Name = "SurePost Media", Subtype = (int)Carrier.UPS)]
        UPSSurePostMedia
    };

    [DbEnum(3)]
    public enum NotificationRecipientType
    {
        Unknown,
        Recipient1,
        Recipient2,
        Recipient3,
        Recipient4,
        Recipient5,
        Sender,
        Recipient,
        Other1,
        Other2
    }

    [DbEnum(4)]
    public enum PaymentType
    {
        Unknown,
        Prepaid, BillRecipient, Bill3rdParty, Collect
    };

    [DbEnum(5)]
    public enum PackageType
    {
        Unknown,
        CustomerPackaging,
        Pak,
        Box,
        Tube,
        Envelope,
        Box10kg,
        Box25kg,

        //ups
        BPMFlats,
        BPMParcel,
        Irregulars,
        Machinables,
        MediaMail,
        ParcelPost,
        StandartFlats,
        Priority,
        FirstClass
    };

    [DbEnum(6)]
    public enum WeightUOMType
    {
        Unknown, LBS, KGS
    };

    [DbEnum(7)]
    public enum DimUOMType
    {
        Unknown, Inches, Centimeters
    };

    [DbEnum(8)]
    public enum ChargeType
    {
        Unknown,

        // FDX
        Net,
        Gross,
        ListNet,
        ListGross,
        TotalDiscount,
        DeclaredValueSurcharge,
        FuelSurcharge,
        DeliveryAreaSurcharge,
        InsideDeliverySurcharge,
        InsidePickupSurcharge,
        PriorityAlertSurcharge,
        ResidentialRuralSurcharge,
        ResidentialSurcharge,
        SaturdayDeliverySurcharge,
        ShipmentNotificationSurcharge,
        TotalSurcharge,
        AppointmentDeliverySurcharge,
        DateCertainDeliverySurcharge,
        EveningDeliverySurcharge,
        SignatureServiceSurcharge,
        NonStandardContainerSurcharge,
        CODSurcharge,
        SaturdayPickupSurcharge,
        OutOfPickupAreaSurcharge,
        OutOfDeliveryAreaSurcharge,
        OffshoreSurcharge,
        AlaskaSurcharge,
        HawaiiSurcharge,
        DangerousGoodHazardousSurcharge,
        ChargesSignatureOptions,
        List_HandlingCharge,
        Customer_HandlingCharge,
        SpecialServices_HandlingCharge,
        SpecialServices_VariableHandlingCharge,
        List_Total_Customer_Charge,
        Customer_Total_Customer_Charge,
        CarbonNeutralFee,
        ShipNotificationFee,
        PackageNotificationFee,

        // UPS Shipment
        ShipperCostPlusHandlingFee,                 // UPSWorldShipUnit / GetShipmentsData
        AmountBilledToShipperPlusHandlingFee,
        TotalOptionsCost,
        TotalServiceCost,

        Freight,                                    // UPSWorldShipUnit / GetDataFromCalBillingTable
        ShipmentTotalCost,
        FreightBilledToRecipient,
        AmountBilledToShipper,
        AmountBilledToRecipient,
        AmountBilledToThirdParty,

        ReturnServiceFee,
        ReturnServiceTotalBilledToSender,

        TotalShipmentCharge,

        // UPS Package
        HandlingCharge,
        AdditionalHandlingFee,
        HazardousMaterialsFee,
        CODFee,
        DeliveryConfirmationFee,
        InsuranceFee,
        VerbalConfirmationFee,
        FlexibleParcelInsuranceFee,
        ShipperReleaseFee,
    };

    [DbEnum(9)]
    public enum AlcoholPackagingType
    {
        Unknown,
        Barrel, Bottle, Case, Carton, Other
    };

    [DbEnum(10)]
    public enum HandlingChargeType
    {
        Unknown,
        Fixed,
        Variable,
        FixedPerPackage
    };

    [DbEnum(11)]
    public enum VariableHandlingChargeType
    {
        Unknown,
        ShippingCharges,
        ShippingSurcharges,
        ListShippingCharges,
        ListShippingSurcharges
    };

    [DbEnum(12)]
    public enum SignatureOptionsType
    {
        Unknown,
        NoSignatureOption, DeliverWithoutSignature,
        Indirect, Direct, Adult
    };

    [DbEnum(13)]
    public enum CODCollectionType
    {
        Unknown,
        None, AnyPaymentType, GuaranteedFunds,
        Currency, Secured, Unsecured
    };

    [DbEnum(14)]
    public enum CODChargeType
    {
        Unknown,
        None, CODCharge, ShippingCharge,
        TotalCharge, OrderChargeTotal, OrderChargeNet
    };

    [DbEnum(15)]
    public enum AlcoholType
    {
        atUnknown,
        atBeer, atWine, atDistill, atAle, atLightWine
    }

    [DbEnum(16)]
    public enum HomeDeliveryType
    {
        hdtUnknown,
        hdtDateCertain, hdtEvening, hdtAppointment
    }

    [DbEnum(17)]
    public enum BillingType
    {
        Unknown,
        Shipper,
        Receiver,
        Consignee,
        Thirdparty,

        Prepaid,
        PrepaidThirdParty,
        FreightCollect,
        ConsigneeBilled,
        FreeOnBoard,
        FreeOnBoardThirdParty,
        ShippingDutyTax,
        ShippingDutyTaxThirdParty,
        DeliveryDutyPaid,
        DeliveryDutyPaidThirdParty,
        CostAndFreightThirdParty,
        CostAndFreight
    };

    [DbEnum(18)]
    public enum InsuranceType
    {
        Basic,
        Expanded,
        TimeInTransit,
        DeclaredValue
    }

    [DbEnum(19)]
    public enum AmountIncreaseDecreaseType
    {
        Unknown,
        Increase,
        Decrease
    }

    [DbEnum(20)]
    public enum ReturnServiceType
    {
        Unknown,
        PrintAndMail,
        PrintReturnLabel,
        ElectronicReturnLabel,
        OneAttempt,
        ThreeAttempts
    }

    [DbEnum(21)]
    public enum ExchangeCollectCurrencyType
    {
        Unknown,
        AUD,
        GBP,
        CAD,
        DKK,
        EUR,
        HKD,
        JPY,
        MXN,
        NOK,
        SGD,
        CHF,
        USD
    }

    [DbEnum(22)]
    public enum NotificationMethodType
    {
        Unknown,
        Fax,
        Email
    }

    [DbEnum(23)]
    public enum TaxIdType
    {
        Unknown,
        DUNS,
        EIN,
        SSN
    }

    [DbEnum(24)]
    public enum EndorsementType
    {
        NotDefined,
        AddressServiceRequested,
        ChangeServiceRequested,
        ForwardServiceRequested,
        ReturnServiceRequested
    }

    [DbEnum(25)]
    public enum ChargeKind
    {
        Freight,
        Surcharge,
        Discount,
        Option,
        NA
    }

    [Table("EnumType", Schema = "sg")]
    public class EnumType
    {
        public int Id { get; set; }
        [MaxLength(60)]
        public string Name { get; set; }
        public List<EnumValue> EnumValues { get; set; }
    }

    [Table("EnumValue", Schema = "sg")]
    public class EnumValue
    {
        public int Id { get; set; }
        [MaxLength(64)]
        public string Value { get; set; }

        [MaxLength(52)]
        public string Name { get; set; }
        [MaxLength(120)]
        public string Description { get; set; }
        public int Subtype { get; set; }
    }

    [AttributeUsage(AttributeTargets.Enum)]
    public class DbEnumAttribute : Attribute
    {
        private int typeId;

        public DbEnumAttribute(int TypeId)
        {
            typeId = TypeId;
        }

        public int DbTypeId
        {
            get { return typeId; }
        }
    }

    [AttributeUsage(AttributeTargets.Field)]
    public class EnumNameAttribute : Attribute
    {
        private string name;
        private string description;
        private int subtype;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        public int Subtype
        {
            get { return subtype; }
            set { subtype = value; }
        }
    }

    public static class AttributeUtilities
    {
        public static int GetEnumTypeId(this Type Attribute)
        {
            Attribute attr = (Attribute)((from v in Attribute.GetCustomAttributes(false)
                                          where v is DbEnumAttribute
                                          select v).FirstOrDefault());
            return (attr as DbEnumAttribute).DbTypeId;
        }

        public static EnumNameAttribute GetEnumTypeNameAttribute<TAttribute>(int Value)
        {
            var type = typeof(TAttribute);
            var name = Enum.GetName(type, Enum.GetValues(type).GetValue(Value));

            return (EnumNameAttribute)(type.GetField(name)
                .GetCustomAttributes(false)
                .FirstOrDefault(t => t is EnumNameAttribute));
        }

        public static string GetEnumTypeName<T>(int Value)
        {
            EnumNameAttribute r = GetEnumTypeNameAttribute<T>(Value);
            if (r != null)
                return (r as EnumNameAttribute).Name;
            else
                return null;
        }

        public static string GetEnumTypeDescription<T>(int Value)
        {
            EnumNameAttribute r = GetEnumTypeNameAttribute<T>(Value);
            if (r != null)
                return (r as EnumNameAttribute).Description;
            else
                return null;
        }

        public static int GetEnumTypeSubtype<T>(int Value)
        {
            EnumNameAttribute r = GetEnumTypeNameAttribute<T>(Value);
            if (r != null)
                return (r as EnumNameAttribute).Subtype;
            else
                return 0;
        }

        private static int SafeGetEnumBase(Type AType, int Value)
        {
            return AType.GetEnumTypeId() == 0 ?
                Value
                : Value % (AType.GetEnumTypeId() * Constants.MAX_ENUM_MEMBERS);
        }

        public static T EnumToEnum<T>(this int Input)
        {
            return (T)Enum.Parse(typeof(T), Input.ToString());
        }

        /// <summary>
        /// Gets Db encoded enum, returns typed enum.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Input"></param>
        /// <returns></returns>
        public static T EnumFromDb<T>(this int Input)
        {
            return (T)Enum.Parse(typeof(T), (SafeGetEnumBase(typeof(T), Input)).ToString());
        }

        public static int EnumToDb<T>(this Enum Input)
        {
            return typeof(T).GetEnumTypeId() * Constants.MAX_ENUM_MEMBERS + Convert.ToInt32(Input);
        }
    }
}
