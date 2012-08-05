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
    public class Item
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string Category { get; set; }

        public string UPC { get; set; }

        public string ISBN { get; set; }
    }

    public class eBay
    {
        public static void AddLogInfo(string Info)
        {
            Debug.WriteLine(
                String.Format("{0}:\t{1}", DateTime.Now.ToString(), Info));
        }

        public static Item[] BuildItems()
        {
            return new Item[] {
                new Item() { Title = "The Affair by Lee Child",
                    Description = "Novell by Lee Child",
                    Price = 5.89M,
                    Category = "267",
                    ISBN = "9780440246305"
                },
                new Item() { Title = "Gadget G200",
                    Description = "Another test gadget",
                    Price = 15.99M,
                    Category = "139973",
                    UPC = "094634957724"
                }
            };
        }

        public static ItemType BuildItem(Item AItem)
        {
            ItemType item = new ItemType();

            // Title
            item.Title = AItem.Title;
            item.Description = AItem.Description;

            item.ListingType = ListingTypeCodeType.FixedPriceItem;

            // listing price
            item.Currency = CurrencyCodeType.USD;
            item.StartPrice = new AmountType() { Value = (Double)AItem.Price, currencyID = CurrencyCodeType.USD };

            // listing duration
            item.ListingDuration = "Days_3";

            // item location and country
            item.Location = "Farmington";
            item.Country = CountryCodeType.US;

            // listing category
            item.PrimaryCategory = new CategoryType() { CategoryID = AItem.Category };  // books

            // item quantity
            item.Quantity = 200;

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
                UPC = AItem.UPC,
                ISBN = AItem.ISBN,
                ProductDetailsURL = "http://alex.tenzee.com",
                DetailsURL = "http://alex.tenzee.com"
            };

            // Return policy
            item.ReturnPolicy = new ReturnPolicyType();
            item.ReturnPolicy.ReturnsAcceptedOption = "ReturnsAccepted";

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

        private static void AddItem(ApiContext apiContext, ItemType item)
        {
            AddItemCall apiCall = new AddItemCall(apiContext);
            AddLogInfo("Beginning to call eBay, please wait...");

            try
            {
                FeeTypeCollection fees = apiCall.AddItem(item);
                AddLogInfo("Completed the call");

                AddLogInfo("The item was listed successfully");
                AddLogInfo("*************** BEGINNING OF THE LIST");
                foreach (FeeType i in fees)
                {
                    AddLogInfo(String.Format("{0} = {1}", i.Name, i.Fee.Value));
                }
                AddLogInfo("*************** END OF THE LIST");
            }
            catch (Exception e)
            {
                AddLogInfo(e.Message);
                throw;
            }
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

            /*{
                GetCategoriesCall apiCall = new GetCategoriesCall(apiContext);

                apiCall.EnableCompression = true;

                apiCall.DetailLevelList.Add(DetailLevelCodeType.ReturnAll);
                apiCall.ViewAllNodes = true;

                apiCall.LevelLimit = 4;

                CategoryTypeCollection cats = apiCall.GetCategories();
                foreach (CategoryType i in cats)
                {
                    AddLogInfo(String.Format("{0} = {1}", i.CategoryID, i.CategoryName));
                }
            }*/


            /*******************************************
            Adding an item
            */
            AddItem(apiContext, BuildItem(BuildItems()[0]));
            AddItem(apiContext, BuildItem(BuildItems()[1]));
        }
    }
}
