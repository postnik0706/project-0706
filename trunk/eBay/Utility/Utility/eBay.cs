using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using eBay.Service.Core.Sdk;
using eBay.Service.Call;
using System.Configuration;
using eBay.Service.Core.Soap;

namespace Utility
{
    public class eBay
    {
        public static void AddLogInfo(string Info)
        {
            Debug.WriteLine(
                String.Format("{0}:\t{1}", DateTime.Now.ToString(), Info));
        }
        
        public static ItemType BuildItem()
        {
            ItemType item = new ItemType();

            // Title
            item.Title = "Gadget G100";
            item.Description = "My test gadget";

            item.ListingType = ListingTypeCodeType.Chinese;

            // listing price
            item.Currency = CurrencyCodeType.USD;
            item.StartPrice = new AmountType() { Value = 15.99, currencyID = CurrencyCodeType.USD };

            // listing duration
            item.ListingDuration = "Days_3";

            // item location and country
            item.Location = "Farmington";
            item.Country = CountryCodeType.US;

            // listing category, games
            item.PrimaryCategory = new CategoryType() { CategoryID = "139973" };

            // item quantity
            item.Quantity = 1200;

            // payment methods
            item.PaymentMethods = new BuyerPaymentMethodCodeTypeCollection(
                new BuyerPaymentMethodCodeType[] { BuyerPaymentMethodCodeType.PayPal }
            );
            item.PayPalEmailAddress = "me@ebay.com";

            // item condition, new
            item.ConditionID = 1000;

            // handling time is required
            item.DispatchTimeMax = 5;

            item.ShippingDetails = BuildShippingDetailsType();

            item.ProductListingDetails = new ProductListingDetailsType()
            {
                UPC = "0027541000085",      // water
                ProductDetailsURL = "http://alex.tenzee.com",
                DetailsURL = "http://alex.tenzee.com"
            };

            return item;
        }

        public static ShippingDetailsType BuildShippingDetailsType()
        {
            ShippingDetailsType sd = new ShippingDetailsType();

            sd.ApplyShippingDiscount = true;

            sd.ShippingType = ShippingTypeCodeType.Flat;

            sd.ShippingServiceOptions = new ShippingServiceOptionsTypeCollection(
                new ShippingServiceOptionsType[] {
                    new ShippingServiceOptionsType()
                    {
                        ShippingService = ShippingServiceCodeType.ShippingMethodStandard.ToString(),
                        ShippingServiceAdditionalCost = new AmountType() { Value = 0.01, currencyID = CurrencyCodeType.USD },
                        ShippingServiceCost = new AmountType() { Value = 1.99, currencyID = CurrencyCodeType.USD },
                        ShippingServicePriority = 1,
                        ShippingInsuranceCost = new AmountType() { Value = 5.01, currencyID = CurrencyCodeType.USD }
                    }
                }
                );
            return sd;
        }

        public static void Run()
        {
            AddLogInfo(
                String.Format("{0}:\tStarting...", DateTime.Now.ToString()));

            ApiContext apiContext = new ApiContext();
            apiContext.SoapApiServerUrl =
                ConfigurationManager.AppSettings["Environment.ApiServerUrl"];
            ApiCredential apiCredential = new ApiCredential();
            apiCredential.eBayToken =
                ConfigurationManager.AppSettings["UserAccount.ApiToken"];
            apiContext.ApiCredential = apiCredential;
            apiContext.Site = global::eBay.Service.Core.Soap.SiteCodeType.US;


            /*******************************************
            Getting server time
            */
            {
                GeteBayOfficialTimeCall apiCall = new GeteBayOfficialTimeCall(apiContext);
                DateTime officialTime = apiCall.GeteBayOfficialTime();

                AddLogInfo(
                    String.Format("Official eBay time: {0}", officialTime));
            }

            /*******************************************
            Adding an item
            */
            ItemType item = BuildItem();

            {
                AddItemCall apiCall = new AddItemCall(apiContext);
                AddLogInfo("Beginning to call eBay, please wait...");

                FeeTypeCollection fees = apiCall.AddItem(item);
                AddLogInfo("Completed the call");

                AddLogInfo("The item was listed successfully");
                AddLogInfo("*************** BEGINNING OF THE LIST");
                foreach (FeeType i in fees)
                {
                    AddLogInfo(String.Format("{0} = {1}", i.Name, i.Fee));
                }
                AddLogInfo("*************** END OF THE LIST");
            }
        }
    }
}
