using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using eBay.Service.Core.Sdk;
using eBay.Service.Call;
using System.Configuration;
using eBay.Service.Core.Soap;
using System.Threading;

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

    public static class Log
    {
        public static void AddLogInfo(string Info)
        {
            Debug.WriteLine(
                String.Format("{0}:\t{1}", DateTime.Now.ToString(), Info));
        }
    }

    public class eBay
    {
        public enum SellerOrBuyer
        {
            typeSELLER, typeBUYER
        }

        private static ApiContext GetContext(SellerOrBuyer Who)
        {
            ApiContext apiContext = new ApiContext();
            apiContext.SoapApiServerUrl =
                ConfigurationManager.AppSettings["Environment.ApiServerUrl"];
            ApiCredential apiCredential = new ApiCredential();
            apiCredential.eBayToken =
                ConfigurationManager.AppSettings[
                Who == SellerOrBuyer.typeSELLER ? "UserAccount.ApiToken_Seller" : "UserAccount.ApiToken_Buyer"];
            apiContext.ApiCredential = apiCredential;
            apiContext.Site = global::eBay.Service.Core.Soap.SiteCodeType.US;
            return apiContext;
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
            item.PrimaryCategory = new CategoryType() { CategoryID = AItem.Category };

            // item quantity
            item.Quantity = 300;

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
            Log.AddLogInfo("Beginning to call eBay, please wait...");

            try
            {
                FeeTypeCollection fees = apiCall.AddItem(item);
                Log.AddLogInfo("Completed the call");

                Log.AddLogInfo("The item was listed successfully");
                Log.AddLogInfo("*************** BEGINNING OF THE LIST");
                foreach (FeeType i in fees)
                {
                    Log.AddLogInfo(String.Format("{0} = {1}", i.Name, i.Fee.Value));
                }
                Log.AddLogInfo("*************** END OF THE LIST");
            }
            catch (Exception e)
            {
                Log.AddLogInfo(e.Message);
                throw;
            }
        }

        public static ItemType[] GetSellerList(ApiContext apiContext)
        {
            GetSellerListCall apiCall = new GetSellerListCall(apiContext);
            apiCall.DetailLevelList.Add(DetailLevelCodeType.ReturnAll);
            apiCall.Pagination = new PaginationType() { EntriesPerPage = 200, PageNumber = 1 };
            apiCall.EndTimeFrom = new DateTime(2012, 8, 1);
            apiCall.EndTimeTo = new DateTime(2012, 8, 20);
            ItemTypeCollection items = apiCall.GetSellerList();
            
            List<ItemType> result = new List<ItemType>();
            foreach (ItemType i in items)
            {
                if (i.ListingType == ListingTypeCodeType.FixedPriceItem)
                {
                    result.Add(i);
                    Log.AddLogInfo(String.Format("{0}: {1}", i.ItemID, i.Description));
                }
            }
            return result.ToArray();
        }
        
        public static void PlaceOffer(ApiContext apiContext, ItemType Item)
        {
            PlaceOfferCall apiCall = new PlaceOfferCall(apiContext);
            apiCall.Offer = new OfferType()
            {
                Action = BidActionCodeType.Purchase,
                Quantity = 1,
                MaxBid = new AmountType() { currencyID = CurrencyCodeType.USD, Value = Item.StartPrice.Value }
            };
            apiCall.AbstractRequest.EndUserIP = "71.234.110.72";
            apiCall.ItemID = Item.ItemID;
            apiCall.Execute();
            if (apiCall.HasError)
            {
                throw new Exception("Error in PlaceOffer");
            }
        }

        public static void MarkAsPaymentReceived(ApiContext apiContext)
        {
            ReviseCheckoutStatusCall apiCall = new ReviseCheckoutStatusCall(apiContext);
            //apiCall.ItemID = 
        }

        public static int GetItemList(ApiContext apiContext, int Page)
        {
            GetSellerTransactionsCall apiCall = new GetSellerTransactionsCall(apiContext);
            apiCall.DetailLevelList = new DetailLevelCodeTypeCollection(new DetailLevelCodeType[] { DetailLevelCodeType.ReturnAll });
            apiCall.ApiRequest.OutputSelector = new StringCollection(new string[] { "TransactionID", "PaginationResult", "TransactionArray.Transaction.Buyer.UserID", "TransactionArray.Transaction.Item.Title" });
            apiCall.Pagination = new PaginationType() { EntriesPerPage = 200, PageNumber = 1 };

            apiCall.Execute();
            Log.AddLogInfo(String.Format("Getting item list - START, page {0}", 1));
            TransactionTypeCollection items = apiCall.GetSellerTransactions(new TimeFilter() { TimeFrom = new DateTime(2012, 8, 1), TimeTo = new DateTime(2012, 8, 30) });
            Log.AddLogInfo(String.Format("Getting item list - SUCCESS, page {0}", 1));

            foreach (TransactionType i in items)
            {
                //Log.AddLogInfo(String.Format("UserID: {0}\tTransactioNID: {1}\tBuyer Name: {2}", i.Buyer.UserID, i.TransactionID, i.Buyer.BuyerInfo.ShippingAddress.Name));
                Log.AddLogInfo(String.Format("{0} {1} {2}", i.TransactionID, i.Buyer.UserID, i.Item.Title));
            }
            return apiCall.PaginationResult.TotalNumberOfPages;
        }

        public static void GetItemList(ApiContext apiContext)
        {
            ThreadedRequest req = new ThreadedRequest(apiContext, new EBayPageObjectFactory());
            foreach (EBayPageProducer item in req.Pages)
            {
                Log.AddLogInfo(item.ToString());
            }
        }

        public static void Run()
        {
            Log.AddLogInfo(
                String.Format("{0}:\tStarting...", DateTime.Now.ToString()));

            ApiContext apiCtxSeller = GetContext(SellerOrBuyer.typeSELLER);

            /*******************************************
            Getting server time
            */
            /*{
                GeteBayOfficialTimeCall apiCall = new GeteBayOfficialTimeCall(apiCtxSeller);
                DateTime officialTime = apiCall.GeteBayOfficialTime();

                Log.AddLogInfo(
                    String.Format("Official eBay time: {0}", officialTime));
            }*/

            /*******************************************
            Adding an item
            */
            /*AddItem(apiCtxSeller, BuildItem(BuildItems()[0]));
            AddItem(apiCtxSeller, BuildItem(BuildItems()[1]));*/
            
            
            /*******************************************
            Getting Seller list
            */
            //ItemType[] items = GetSellerList(apiCtxSeller);

            
            /*******************************************
            Placing offers
            */
            /*for (int i = 0; i < 200; i++)
            {
                Log.AddLogInfo(String.Format("Buying cycle... {0} - START", i));
                ApiContext apiCtxBuyer = GetContext(SellerOrBuyer.typeBUYER);
                PlaceOffer(apiCtxBuyer, items[4]);
                Log.AddLogInfo(String.Format("Buying cycle... {0} - SUCCESS", i));
            }*/

            GetItemList(apiCtxSeller);
        }
    }
}