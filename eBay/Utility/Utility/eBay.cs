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
using eBay.Service.Util;
using System.Web;

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

    public class eBayClass
    {
        public enum SellerOrBuyer
        {
            typeSELLER, typeBUYER
        }

        private static ApiLogger logger = new FileLogger(
            ConfigurationManager.AppSettings["eBayLogger"]);
        public static CallMetricsTable Metrics;
        public static ApiLogManager LogManager;
        //public static ApiLogger Logger { get {return logger; } }

        public static ApiContext GetContext(SellerOrBuyer Who)
        {
            ApiContext apiContext = new ApiContext();
            apiContext.SoapApiServerUrl =
                ConfigurationManager.AppSettings["Environment.ApiServerUrl"];
            ApiCredential apiCredential = new ApiCredential();
            apiCredential.eBayToken =
                ConfigurationManager.AppSettings[
                Who == SellerOrBuyer.typeSELLER ? "UserAccount.ApiToken_Seller" : "UserAccount.ApiToken_Buyer"];
            apiCredential.ApiAccount.Application = ConfigurationManager.AppSettings["AppID"];
            apiCredential.ApiAccount.Developer = ConfigurationManager.AppSettings["DevID"];
            apiCredential.ApiAccount.Certificate = ConfigurationManager.AppSettings["CertID"];
            apiCredential.eBayAccount.UserName = "";
            apiCredential.eBayAccount.Password = "";
            
            apiContext.ApiCredential = apiCredential;
            
            apiContext.Site = global::eBay.Service.Core.Soap.SiteCodeType.US;

            Metrics = new CallMetricsTable();
            apiContext.EnableMetrics = true;
            apiContext.CallMetricsTable = Metrics;

            LogManager = new ApiLogManager();
            LogManager.EnableLogging = true;
            
            LogManager.ApiLoggerList.Add(logger);
            LogManager.ApiLoggerList[0].LogApiMessages = true;
            LogManager.ApiLoggerList[0].LogExceptions = true;
            LogManager.ApiLoggerList[0].LogInformations = true;
            apiContext.ApiLogManager = LogManager;

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
            apiContext.ApiLogManager.RecordMessage("Beginning to call eBay, please wait...", MessageType.Information, MessageSeverity.Informational);

            try
            {
                FeeTypeCollection fees = apiCall.AddItem(item);
                apiContext.ApiLogManager.RecordMessage("Completed the call", MessageType.Information, MessageSeverity.Informational);

                apiContext.ApiLogManager.RecordMessage("The item was listed successfully", MessageType.Information, MessageSeverity.Informational);
                apiContext.ApiLogManager.RecordMessage("*************** BEGINNING OF THE LIST", MessageType.Information, MessageSeverity.Informational);
                foreach (FeeType i in fees)
                {
                    apiContext.ApiLogManager.RecordMessage(String.Format("{0} = {1}", i.Name, i.Fee.Value), MessageType.Information, MessageSeverity.Informational);
                }
                apiContext.ApiLogManager.RecordMessage("*************** END OF THE LIST", MessageType.Information, MessageSeverity.Informational);
            }
            catch (Exception e)
            {
                apiContext.ApiLogManager.RecordMessage(e.Message, MessageType.Information, MessageSeverity.Informational);
                throw;
            }
        }

        public static ItemType[] GetSellerList(ApiContext apiContext)
        {
            GetSellerListCall apiCall = new GetSellerListCall(apiContext);
            apiCall.DetailLevelList.Add(DetailLevelCodeType.ReturnAll);
            apiCall.Pagination = new PaginationType() { EntriesPerPage = 200, PageNumber = 1 };
            apiCall.EndTimeFrom = new DateTime(2012, 9, 1);
            apiCall.EndTimeTo = new DateTime(2012, 9, 20);
            ItemTypeCollection items = apiCall.GetSellerList();

            

            List<ItemType> result = new List<ItemType>();
            foreach (ItemType i in items)
            {
                if (i.ListingType == ListingTypeCodeType.FixedPriceItem)
                {
                    result.Add(i);
                    apiContext.ApiLogManager.RecordMessage(String.Format("{0}: {1}", i.ItemID, i.Description), MessageType.Information, MessageSeverity.Informational);
                }
            }
            return result.ToArray();
        }
        
        public static void PlaceOffer(ApiContext apiContext, string ItemID, double AValue)
        {
            PlaceOfferCall apiCall = new PlaceOfferCall(apiContext);
            apiCall.Offer = new OfferType()
            {
                Action = BidActionCodeType.Purchase,
                Quantity = 1,
                MaxBid = new AmountType() { currencyID = CurrencyCodeType.USD, Value = AValue }
            };
            apiCall.AbstractRequest.EndUserIP = "71.234.110.72";
            apiCall.ItemID = ItemID;
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
//            apiCall.ApiRequest.OutputSelector = new StringCollection(new string[] { "TransactionID", "PaginationResult", "TransactionArray.Transaction.Buyer.UserID", "TransactionArray.Transaction.Item.Title" });
            apiCall.Pagination = new PaginationType() { EntriesPerPage = 200, PageNumber = Page };

            apiCall.Execute();
            apiContext.ApiLogManager.RecordMessage(String.Format("Getting item list - START, page {0}", Page), MessageType.Information, MessageSeverity.Informational);
            TransactionTypeCollection items = apiCall.GetSellerTransactions(new TimeFilter() { 
                TimeFrom = new DateTime(2012, 8, 14, 12, 0, 0), 
                TimeTo = new DateTime(2012, 8, 14, 23, 59, 59) });

            apiContext.ApiLogManager.RecordMessage(String.Format("Getting item list - SUCCESS, page {0}", Page), MessageType.Information, MessageSeverity.Informational);

            var ordered = from TransactionType i in items
                          orderby i.CreatedDate
                          select new { i.Buyer.UserID, i.TransactionID, i.Buyer.BuyerInfo.ShippingAddress.Name, i.CreatedDate, i.ShippingDetails.SellingManagerSalesRecordNumber };
            
            foreach (var i in ordered)
            {
                apiContext.ApiLogManager.RecordMessage(String.Format("UserID: {0}\tTransactioNID: {1}\tBuyer Name: {2}\tCreation Time: {3}\tSellingManagerSalesRecordNumber: {4}",
                    i.UserID, i.TransactionID, i.Name, i.CreatedDate, i.SellingManagerSalesRecordNumber), MessageType.Information, MessageSeverity.Informational);
            }
            return apiCall.PaginationResult.TotalNumberOfPages;
        }

        // Test API
        // https://developer.ebay.com/DevZone/build-test/test-tool/default.aspx
        
        public static string CreateSessionID(ApiContext apiContext)
        {
            /*AuthenticationEntryType oAuthEntryType = new AuthenticationEntryType();
            oAuthEntryType.TokenReturnMethod = TokenReturnMethodCodeType.FetchToken;
            SetReturnURLCall oSetReturnURLCall = new SetReturnURLCall(SDKApiContext);
            if (bUseeBayTestSite)
                m_oApiContext.SignInUrl = EBAYSIGNURL_USERID_SANDBOX + sUserID + "&runame=" + RUNAME + "&sid=" + SID;
            else
                m_oApiContext.SignInUrl = EBAYSIGNURL_USERID + sUserID + "&runame=" + RUNAME + "&sid=" + SID;
            m_oApiContext.Version = VERSION;
            oSetReturnURLCall.AuthenticationEntry = oAuthEntryType;
            System.Diagnostics.Process.Start(m_oApiContext.SignInUrl); */
            
            GetSessionIDCall sessionId = new GetSessionIDCall(apiContext);
            sessionId.RuName = ConfigurationManager.AppSettings["RuName"];
            sessionId.ApiContext.ApiCredential.ApiAccount.Application = ConfigurationManager.AppSettings["AppID"];
            sessionId.ApiContext.ApiCredential.ApiAccount.Developer = ConfigurationManager.AppSettings["DevID"];
            sessionId.ApiContext.ApiCredential.ApiAccount.Certificate = ConfigurationManager.AppSettings["CertID"];
            sessionId.Execute();
            apiContext.ApiLogManager.RecordMessage(String.Format("*** Session ID: {0}", sessionId.SessionID));
            string uri = String.Format("https://signin.sandbox.ebay.com/ws/eBayISAPI.dll?SignIn&RuName={0}&SessID={1}",
                ConfigurationManager.AppSettings["RuName"], sessionId.SessionID);
            apiContext.ApiLogManager.RecordMessage(String.Format("*** URL: {0}", uri));
            
            uri = String.Format("https://signin.sandbox.ebay.com/ws/eBayISAPI.dll?SignIn&RuName={0}&SessID={1}",
                ConfigurationManager.AppSettings["RuName"], HttpUtility.UrlEncode(sessionId.SessionID));
            apiContext.ApiLogManager.RecordMessage(String.Format("*** URL encoded: {0}", uri));
            return sessionId.SessionID;
        }


        public static string GetTokenFromeBay(ApiContext apiContext, string SessionId)
        {
            /*string token = string.Empty; 
            try
            {
                FetchTokenCall oFetchTokenCall = new FetchTokenCall(SDKApiContext);
                oFetchTokenCall.SecretID = SID;
                oFetchTokenCall.UserID = userID;
                oFetchTokenCall.CallRetry = _CreateAndInitCallRetryObject();
                oFetchTokenCall.EnableCompression = ENABLECOMPRESS;
                token = oFetchTokenCall.FetchToken(SID);
            }
            catch
            {
                throw;
            }
            return token;*/

            FetchTokenCall tokenCall = new FetchTokenCall(apiContext);
            tokenCall.SessionID = SessionId;
            
            tokenCall.Execute();
            return tokenCall.eBayToken;
        }

        public static int GetItemList_GetOrders(ApiContext apiContext, int Page)
        {
            GetOrdersCall apiCall = new GetOrdersCall(apiContext);
            
            apiCall.DetailLevelList = new DetailLevelCodeTypeCollection(new DetailLevelCodeType[] { DetailLevelCodeType.ReturnAll });
            //apiCall.ApiRequest.OutputSelector = new StringCollection(new string[] { "TransactionID", "PaginationResult", "TransactionArray.Transaction.Buyer.UserID", "TransactionArray.Transaction.Item.Title" });
            apiCall.Pagination = new PaginationType() { EntriesPerPage = 200, PageNumber = Page };

            //apiCall.Execute();
            apiContext.ApiLogManager.RecordMessage(String.Format("Getting item list - START, page {0}", 1));

            GeteBayOfficialTimeCall timeCall = new GeteBayOfficialTimeCall(apiContext);
            TimeSpan timeDiff = DateTime.Now - timeCall.GeteBayOfficialTime();
            
            TimeFilter createTime = new TimeFilter() {
                TimeFrom = new DateTime(2012, 9, 1, 12, 00, 0).Subtract(timeDiff), 
                TimeTo = new DateTime(2012, 9, 8, 13, 36, 59).Subtract(timeDiff) };
            OrderTypeCollection items = apiCall.GetOrders(createTime,
                TradingRoleCodeType.Seller, OrderStatusCodeType.Active);
            apiContext.ApiLogManager.RecordMessage(String.Format("Getting item list - SUCCESS, page {0}", 1));

            foreach (OrderType i in items)
            {
                //Log.AddLogInfo(String.Format("UserID: {0}\tTransactioNID: {1}\tBuyer Name: {2}", i.Buyer.UserID, i.TransactionID, i.Buyer.BuyerInfo.ShippingAddress.Name));
                apiContext.ApiLogManager.RecordMessage(String.Format("TransactionID {0}\tBuyer Name{1}\tCreated on {2}",
                    i.OrderID, i.BuyerUserID, i.CreatedTime));
            }
            return apiCall.PaginationResult.TotalNumberOfPages;
        }

        public static void GetItemList(ApiContext apiContext)
        {
            ThreadedRequest req = new ThreadedRequest(apiContext, new EBayPageObjectFactory());
            foreach (EBayPageProducer item in req.Pages)
            {
                apiContext.ApiLogManager.RecordMessage(item.ToString(), MessageType.Information, MessageSeverity.Informational);
            }
        }

        public static void Run()
        {
            ApiContext apiCtxSeller = GetContext(SellerOrBuyer.typeSELLER);
            
            //logManager.RecordMessage(String.Format("{0}:\tStarting...", DateTime.Now.ToString()), MessageType.Exception, MessageSeverity.Informational);
            logger.RecordMessage(
                String.Format("{0}:\tStarting...", DateTime.Now.ToString()), MessageSeverity.Informational);

            /*******************************************
            Getting server time
            */
            /*{
                GeteBayOfficialTimeCall apiCall = new GeteBayOfficialTimeCall(apiCtxSeller);
                DateTime officialTime = apiCall.GeteBayOfficialTime();

                apiContext.ApiLogManager.RecordMessage(
                    String.Format("Official eBay time: {0}", officialTime), MessageType.Information, MessageSeverity.Informational);
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
            for (int i = 0; i < 200; i++)
            {
                ApiContext apiCtxBuyer = GetContext(SellerOrBuyer.typeBUYER);
                apiCtxBuyer.ApiLogManager.RecordMessage(String.Format("Buying cycle... {0} - START", i), MessageType.Information, MessageSeverity.Informational);
                PlaceOffer(apiCtxBuyer, "110102377760", 9.99);
                apiCtxBuyer.ApiLogManager.RecordMessage(String.Format("Buying cycle... {0} - SUCCESS", i), MessageType.Information, MessageSeverity.Informational);
            }

            //GetItemList(apiCtxSeller, 1);

            //GetItemList_GetOrders(apiCtxSeller, 1);

            /*
            string sessionId = CreateSessionID(apiCtxSeller);
            apiCtxSeller.ApiLogManager.RecordMessage(String.Format("*** SessionID received: {0}", sessionId));
            
             
            */
            /*
            string token = GetTokenFromeBay(apiCtxSeller, "s+IBAA**a58702051390a471d22382d3fffffa0a");
            apiCtxSeller.ApiLogManager.RecordMessage(String.Format("*** Token received: {0}", token));
            */

            Metrics.GenerateReport(apiCtxSeller.ApiLogManager.ApiLoggerList[0]);
        }
    }
}