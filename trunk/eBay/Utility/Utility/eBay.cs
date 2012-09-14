using System;
using System.Collections.Generic;
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

        public StringCollection PictureURL { get; set; }
    }

    public class eBayClass
    {
        public enum SellerOrBuyer
        {
            typeSELLER, typeBUYER
        }

        private static ApiLogger logger = new FileLogger(ConfigurationManager.AppSettings["eBayLogger"]);
        public static CallMetricsTable Metrics;
        public static ApiLogManager LogManager;

        private static ApiContext m_SellerContext = null;
        public static ApiContext SellerContext
        {
            get
            {
                if (m_SellerContext == null)
                    m_SellerContext = eBayClass.GetContext(eBayClass.SellerOrBuyer.typeSELLER);
                return m_SellerContext;
            }
        }

        private static ApiContext m_BuyerContext = null;
        public static ApiContext BuyerContext
        {
            get
            {
                if (m_BuyerContext == null)
                    m_BuyerContext = eBayClass.GetContext(eBayClass.SellerOrBuyer.typeBUYER);
                return m_BuyerContext;
            }
        }

        private static DateTime? m_EBayDate = null;
        private static TimeSpan m_TimeDiff;
        public static DateTime? EBayDate
        {
            get
            {
                m_EBayDate = GetEBayDate(SellerContext, out m_TimeDiff);
                return m_EBayDate.Value;
            }
        }

        public static TimeSpan TimeDiff
        {
            get
            {
                if (!m_EBayDate.HasValue)
                    m_EBayDate = GetEBayDate(SellerContext, out m_TimeDiff);
                return m_TimeDiff;
            }
        }

        private static AutoResetEvent logFileAccess;
        public static AutoResetEvent LogFileAccess
        {
            get {
                return logFileAccess;
            }
        }

        private static AutoResetEvent logFileReader;
        public static AutoResetEvent LogFileReader { get { return logFileReader; } }

        static eBayClass()
        {
            logFileAccess = new AutoResetEvent(false);
        }

        public static void StartLogMonitor()
        {
            logFileReader = new AutoResetEvent(false);
        }

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
                new Item() { Title = "Matrioshka How Cool",
                    Description = "By Russian Folklore",
                    Price = 56.99M,
                    Category = "160872",
                    PictureURL = new  StringCollection() { "http://ourrocks.info/alex/images/matrioshka.JPG" },
                    UPC = "1234567890"
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
                ProductDetailsURL = "http://alex.tenzee.com",
                DetailsURL = "http://alex.tenzee.com"
            };

            if ((AItem.PictureURL != null) && (AItem.PictureURL.Count > 0))
                item.ProductListingDetails.StockPhotoURL = AItem.PictureURL[0];

            item.ProductListingDetails.ListIfNoProductSpecified = true;
            item.ProductListingDetails.ListIfNoProduct = true;

            if (AItem.UPC != null)
                item.ProductListingDetails.UPC = AItem.UPC;
            if (AItem.ISBN != null)
                item.ProductListingDetails.ISBN = AItem.ISBN;

            if (AItem.PictureURL != null)
            {
                item.PictureDetails = new PictureDetailsType();
                item.PictureDetails.PictureURL = AItem.PictureURL;
            }

            // Return policy
            item.ReturnPolicy = new ReturnPolicyType();
            item.ReturnPolicy.ReturnsAcceptedOption = "ReturnsAccepted";

            return item;
        }

        public static ShippingDetailsType BuildShippingDetailsType()
        {
            ShippingDetailsType sd = new ShippingDetailsType();

            /*sd.InternationalShippingServiceOption = new InternationalShippingServiceOptionsTypeCollection(
                new InternationalShippingServiceOptionsType[] {
                    new InternationalShippingServiceOptionsType()
                    {
                        ShippingService = ShippingTypeCodeType.Flat.ToString(),
                        ShippingServiceAdditionalCost = new AmountType() { Value = 3.8, currencyID = CurrencyCodeType.USD },
                        ShippingServiceCost = new AmountType() { Value = 3.8, currencyID = CurrencyCodeType.USD},
                        ShippingServicePriority = 1,
                        ShippingInsuranceCost = new AmountType() { Value = 5.02, currencyID = CurrencyCodeType.USD }
                    }
                });
            */
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

        private static void AddItem(ApiContext apiContext)
        {
            AddItemCall apiCall = new AddItemCall(apiContext);
            apiContext.ApiLogManager.RecordMessage("Beginning to call eBay, please wait...", MessageType.Information, MessageSeverity.Informational);

            try
            {
                ItemType item = new ItemType();

                item.ProductListingDetails = new ProductListingDetailsType()
                {
                    UPC = "610214624192",
                    DetailsURL = "http://www.google.com",
                    IncludePrefilledItemInformation = true,
                    IncludePrefilledItemInformationSpecified = true,
                    IncludeStockPhotoURL = true,
                    IncludeStockPhotoURLSpecified = true,
                    StockPhotoURL = "http://i.ebayimg.com/02/!!eFW3!wBGM~$(KGrHqF,!lcE1F1IqYejBNUs+NoIJg~~_7.JPG?set_id=89040003C1"
                };


                item.Title = "Dell Streak 7 16GB, Wi-Fi + 4G (T-Mobile), 7in - Black";
                item.Description = "Cool smartphone";
                item.ListingType = ListingTypeCodeType.FixedPriceItem;
                item.ConditionDisplayName = "New";

                // listing price
                item.Currency = CurrencyCodeType.USD;
                item.StartPrice = new AmountType() { Value = (Double)161.83, currencyID = CurrencyCodeType.USD };

                // listing duration
                item.ListingDuration = "Days_10";

                // item location and country
                item.Location = "Farmington";
                item.Country = CountryCodeType.US;

                // listing category
                item.PrimaryCategory = new CategoryType() { CategoryID = "171485" };

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

                // Return policy
                item.ReturnPolicy = new ReturnPolicyType();
                item.ReturnPolicy.ReturnsAcceptedOption = "ReturnsAccepted";

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

            foreach (TransactionType i in items)
            {
                apiContext.ApiLogManager.RecordMessage(String.Format("UserID: {0}\tTransactioNID: {1}\tBuyer Name: {2}\tCreation Time: {3}\tSellingManagerSalesRecordNumber: {4}",
                    i.Buyer.UserID, i.TransactionID, i.Buyer.BuyerInfo.ShippingAddress.Name, i.CreatedDate, i.ShippingDetails.SellingManagerSalesRecordNumber), MessageType.Information, MessageSeverity.Informational);
            }
            return apiCall.PaginationResult.TotalNumberOfPages;
        }

        // Test API
        // https://developer.ebay.com/DevZone/build-test/test-tool/default.aspx
        
        public static string CreateSessionID(ApiContext apiContext)
        {
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
            FetchTokenCall tokenCall = new FetchTokenCall(apiContext);
            tokenCall.SessionID = SessionId;
            
            tokenCall.Execute();
            return tokenCall.eBayToken;
        }

        public static DateTime GetEBayDate(ApiContext apiContext, out TimeSpan TimeDiff)
        {
            LogManager.RecordMessage("Starting a GeteBayOfficialTime call");
            Blink();

            Stopwatch s = Stopwatch.StartNew();
            GeteBayOfficialTimeCall timeCall = new GeteBayOfficialTimeCall(apiContext);
            DateTime eBayDate = timeCall.GeteBayOfficialTime();
            TimeDiff = DateTime.Now - eBayDate;
            eBayClass.Metrics.GenerateReport(eBayClass.LogManager.ApiLoggerList[0]);
            s.Stop();
            Blink();
            
            eBayClass.LogManager.RecordMessage("Done; ms: " + s.ElapsedMilliseconds.ToString());
            Blink();
            return eBayDate;
        }

        private static void Blink()
        {
            logFileAccess.Set();
            if (logFileReader != null)
                logFileReader.WaitOne();
        }
        
        public static int GetNumberOfItems(ApiContext apiContext,
            DateTime DateFrom, DateTime DateTo, bool Active = true, bool Completed = true)
        {
            eBayClass.LogManager.RecordMessage("Starting a GetNumberOfItems call");
            Blink();

            Stopwatch s = Stopwatch.StartNew();
            GetOrdersCall_ apiCall = new GetOrdersCall_(apiContext);
            Blink();

            apiCall.DetailLevelList = new DetailLevelCodeTypeCollection(new DetailLevelCodeType[] { DetailLevelCodeType.ReturnAll });
            apiCall.ApiRequest.OutputSelector = new StringCollection() {"TransactionID", "PaginationResult"};
            apiCall.Pagination = new PaginationType() { EntriesPerPage = 100, PageNumber = 1 };
            TimeFilter createTime = new TimeFilter()
            {
                TimeFrom = DateFrom.Subtract(TimeDiff),
                TimeTo = DateTo.Subtract(TimeDiff)
            };
            OrderStatusCodeType filterStatus = OrderStatusCodeType.All;
            if (Active && !Completed)
                filterStatus = OrderStatusCodeType.Active;
            else if (Completed && !Active)
                filterStatus = OrderStatusCodeType.Completed;

            OrderTypeCollection items = apiCall.GetOrders(createTime,
                TradingRoleCodeType.Seller, filterStatus);
            apiContext.ApiLogManager.RecordMessage(String.Format("Getting item list - SUCCESS, page {0}", 1));
            Blink();

            eBayClass.Metrics.GenerateReport(eBayClass.LogManager.ApiLoggerList[0]);
            eBayClass.LogManager.RecordMessage("Done; ms: " + s.ElapsedMilliseconds.ToString());
            Blink();

            return apiCall.PaginationResult.TotalNumberOfEntries;
        }

        public static List<Transaction> GetOrders(IGetOrdersCall AGetOrdersCall,
            DateTime DateFrom, DateTime DateTo, bool Active = true, bool Completed = true,
            bool MinimumOutput = false)
        {
            AGetOrdersCall.ApiContext.ApiLogManager.RecordMessage("Starting a GetOrders call " + (MinimumOutput ? "with minimum output" : "with normal output"));
            Blink();

            Stopwatch s = Stopwatch.StartNew();
            AGetOrdersCall.DetailLevelList = new DetailLevelCodeTypeCollection(new DetailLevelCodeType[] { DetailLevelCodeType.ReturnAll });
            AGetOrdersCall.ApiContext.ApiLogManager.RecordMessage(String.Format("Getting item list - START, page {0}", 1));
            Blink();

            TimeFilter createTime = new TimeFilter()
            {
                TimeFrom = DateFrom.Subtract(TimeDiff),
                TimeTo = DateTo.Subtract(TimeDiff)
            };

            if (MinimumOutput)
                AGetOrdersCall.ApiRequest.OutputSelector = new StringCollection(new string[] { "TransactionID", "PaginationResult", "SellingManagerSalesRecordNumber", "ItemID", "CreatedTime" });
            
            OrderStatusCodeType filterStatus = OrderStatusCodeType.All;
            if (Active && !Completed)
                filterStatus = OrderStatusCodeType.Active;
            else if (Completed && !Active)
                filterStatus = OrderStatusCodeType.Completed;

            int Page = 1;
            List<Transaction> res = new List<Transaction>();
            do
            {
                AGetOrdersCall.Pagination = new PaginationType() { EntriesPerPage = 200, PageNumber = Page };
                OrderTypeCollection items = AGetOrdersCall.GetOrders(createTime,
                    TradingRoleCodeType.Seller, filterStatus);
                Blink();

                AGetOrdersCall.ApiContext.ApiLogManager.RecordMessage(String.Format("Getting item list - SUCCESS, page {0}", Page));
                Blink();

                foreach (OrderType i in items)
                {
                    foreach (TransactionType j in i.TransactionArray)
                    {
                        res.Add(new Transaction() { OrderID=i.OrderID, TransactionId=j.TransactionID,
                            ItemID=j.Item.ItemID, SellingManagerRecordNumber=j.ShippingDetails.SellingManagerSalesRecordNumber,
                            CreatedTime=i.CreatedTime, OrderStatus=i.OrderStatus });

                        AGetOrdersCall.ApiContext.ApiLogManager.RecordMessage(String.Format("TransactionID {0}\tItem ID{1}\tCreated on {2}",
                            i.OrderID, j.Item.ItemID, i.CreatedTime));
                    }
                }
                Blink();
                
                Page++;
            } while (Page <= AGetOrdersCall.PaginationResult.TotalNumberOfPages);

            eBayClass.Metrics.GenerateReport(eBayClass.LogManager.ApiLoggerList[0]);
            eBayClass.LogManager.RecordMessage("Done; ms: " + s.ElapsedMilliseconds.ToString());
            Blink();

            return res;
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
            AddItem(apiCtxSeller, BuildItem(BuildItems()[0]));
            AddItem(apiCtxSeller, BuildItem(BuildItems()[1]));
            AddItem(apiCtxSeller);
            
            
            /*******************************************
            Getting Seller list
            */
            //ItemType[] items = GetSellerList(apiCtxSeller);

            
            

            
            /*******************************************
            Placing offers
            */
            /*for (int i = 0; i < 200; i++)
            {
                ApiContext apiCtxBuyer = GetContext(SellerOrBuyer.typeBUYER);
                apiCtxBuyer.ApiLogManager.RecordMessage(String.Format("Buying cycle... {0} - START", i), MessageType.Information, MessageSeverity.Informational);
                PlaceOffer(apiCtxBuyer, "110102377760", 9.99);
                apiCtxBuyer.ApiLogManager.RecordMessage(String.Format("Buying cycle... {0} - SUCCESS", i), MessageType.Information, MessageSeverity.Informational);
            }
            */
            //GetItemList(apiCtxSeller, 1);

            /*GetItemList_GetOrders(apiCtxSeller, new DateTime(2012, 9, 1),
                new DateTime(2012, 9, 20));*/

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

        public static void GetOrderTransactions(ApiContext apiContext, string ItemID, string[] transactionIds)
        {
            Stopwatch s = Stopwatch.StartNew();
            GetOrderTransactionsCall apiCall = new GetOrderTransactionsCall(apiContext);

            apiCall.DetailLevelList = new DetailLevelCodeTypeCollection(new DetailLevelCodeType[] { DetailLevelCodeType.ReturnAll });
            apiContext.ApiLogManager.RecordMessage(String.Format("Getting item list - START, page {0}", 1));
            Blink();

            ItemTransactionIDTypeCollection inp = new ItemTransactionIDTypeCollection();
            foreach (var i in transactionIds)
	        {
                inp.Add(new ItemTransactionIDType() { TransactionID = i, ItemID = ItemID });
        	}
            Blink();

            OrderTypeCollection items = apiCall.GetOrderTransactions(inp);

            foreach (OrderType i in items)
            {
                foreach (TransactionType j in i.TransactionArray)
                {
                    apiContext.ApiLogManager.RecordMessage(String.Format("TransactionID {0}\tItem ID{1}\tCreated on {2}\tItem Title{3}",
                        i.OrderID, j.Item.ItemID, i.CreatedTime, j.Item.Title));
                }
            }
            Blink();

            apiContext.ApiLogManager.RecordMessage("Getting item list - SUCCESS");
            eBayClass.Metrics.GenerateReport(eBayClass.LogManager.ApiLoggerList[0]);
            eBayClass.LogManager.RecordMessage("Done; ms: " + s.ElapsedMilliseconds.ToString());
            Blink();
        }

        public static void GetItemTransactions(ApiContext apiContext, string ItemID)
        {
            Stopwatch s = Stopwatch.StartNew();
            GetItemTransactionsCall apiCall = new GetItemTransactionsCall(apiContext);
            Blink();

            apiCall.DetailLevelList = new DetailLevelCodeTypeCollection(new DetailLevelCodeType[] { DetailLevelCodeType.ReturnAll });
            apiCall.IncludeContainingOrder = true;
            apiCall.IncludeFinalValueFee = true;
            apiCall.Pagination = new PaginationType() { EntriesPerPage = 100, PageNumber = 1 };
            apiContext.ApiLogManager.RecordMessage(String.Format("Getting item transactions - START, page {0}", 1));
            //apiCall.ApiRequest.OutputSelector = new StringCollection(new string[] { "PaginationResult", "OrderID", "OrderLineItemID", "SellingManagerSalesRecordNumber", "TransactionArray.Transaction.Buyer.UserID", "Item.ItemID", "TransactionID", "TransactionArray.Transaction.QuantityPurchased"});

            int Page = 1;
            do
            {
                TransactionTypeCollection items = apiCall.GetItemTransactions(ItemID, new TimeFilter() { TimeFrom = DateTime.Now.Subtract(TimeDiff).Subtract(new TimeSpan(29, 0, 0, 0)),
                    TimeTo = DateTime.Now.Subtract(TimeDiff) } );
                apiContext.ApiLogManager.RecordMessage(String.Format("Getting item list - SUCCESS, page {0}", Page));
                Blink();

                foreach (TransactionType i in items)
                {
                    apiContext.ApiLogManager.RecordMessage(String.Format("TransactionID {0}\tItemID {1}\tItem Title{2}",
                        i.ContainingOrder != null ? i.ContainingOrder.OrderID : i.OrderLineItemID, apiCall.ItemID, apiCall.Item.Title));
                }
                Blink();

                Page++;
            } while (Page <= apiCall.PaginationResult.TotalNumberOfPages);

            apiContext.ApiLogManager.RecordMessage("Getting Transactions - SUCCESS");
            eBayClass.Metrics.GenerateReport(eBayClass.LogManager.ApiLoggerList[0]);
            eBayClass.LogManager.RecordMessage("Done; ms: " + s.ElapsedMilliseconds.ToString());
            Blink();
        }

        public static void GetSellingManagerSaleRecord(ApiContext apiContext, string p)
        {
            Stopwatch s = Stopwatch.StartNew();
            GetSellingManagerSoldListingsCall apiCall = new GetSellingManagerSoldListingsCall(apiContext);
            Blink();

            apiCall.DetailLevelList = new DetailLevelCodeTypeCollection(new DetailLevelCodeType[] { DetailLevelCodeType.ReturnAll });
            apiCall.Pagination = new PaginationType() { EntriesPerPage = 100, PageNumber = 1 };
            apiContext.ApiLogManager.RecordMessage(String.Format("Getting Selling Manager Sale Record - START, page {0}", 1));
            apiCall.ApiRequest.OutputSelector = new StringCollection(new string[] { "PaginationResult", "OrderID", "OrderLineItemID", "SellingManagerSalesRecordNumber", "Item.ItemID", "TransactionID", "TransactionArray.Transaction.QuantityPurchased"});

            int Page = 1;
            do
            {
                SellingManagerSoldOrderTypeCollection items = apiCall.GetSellingManagerSoldListings(
                    new SellingManagerSearchType() { SearchType = SellingManagerSearchTypeCodeType.SaleRecordID, SearchValue = p }, 0, null, false,
                    SellingManagerSoldListingsSortTypeCodeType.SalesRecordID, SortOrderCodeType.Ascending,
                    new PaginationType() { EntriesPerPage = 100, PageNumber = 1 }, null);

                apiContext.ApiLogManager.RecordMessage(String.Format("Getting item list - SUCCESS, page {0}", Page));
                Blink();

                foreach (SellingManagerSoldOrderType i in items)
                {
                    apiContext.ApiLogManager.RecordMessage(String.Format("TransactionID {0}", i.SaleRecordID));
                }
                Blink();

                Page++;
            }
            while (Page <= apiCall.PaginationResult.TotalNumberOfPages);

            apiContext.ApiLogManager.RecordMessage("Getting Transactions - SUCCESS");
            eBayClass.Metrics.GenerateReport(eBayClass.LogManager.ApiLoggerList[0]);
            eBayClass.LogManager.RecordMessage("Done; ms: " + s.ElapsedMilliseconds.ToString());
            Blink();
        }

        public static void GetOrderTransactions(ApiContext apiContext, string ItemID, string TransactionID)
        {
            Stopwatch s = Stopwatch.StartNew();
            GetOrderTransactionsCall apiCall = new GetOrderTransactionsCall(apiContext);
            apiContext.ApiLogManager.RecordMessage(String.Format("Getting Order transactions - START, page {0}", 1));
            Blink();

            ItemTransactionIDTypeCollection inp = new ItemTransactionIDTypeCollection() { new ItemTransactionIDType() { OrderLineItemID = ItemID+"-"+TransactionID } };
            OrderTypeCollection items = apiCall.GetOrderTransactions(inp);
            Blink();

            foreach (OrderType i in items)
            {
                foreach (TransactionType j in i.TransactionArray)
                {
                    apiContext.ApiLogManager.RecordMessage(String.Format("TransactionID {0}\tItem ID{1}\tCreated on {2}\tItem Title{3}",
                        i.OrderID, j.Item.ItemID, i.CreatedTime, j.Item.Title));
                }
            }
            Blink();

            apiContext.ApiLogManager.RecordMessage("Getting item list - SUCCESS");
            eBayClass.Metrics.GenerateReport(eBayClass.LogManager.ApiLoggerList[0]);
            eBayClass.LogManager.RecordMessage("Done; ms: " + s.ElapsedMilliseconds.ToString());
            Blink();
       }
    }
}