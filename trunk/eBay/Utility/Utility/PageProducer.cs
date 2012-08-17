using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using eBay.Service.Core.Soap;
using eBay.Service.Core.Sdk;
using eBay.Service.Call;
using System.Threading;

namespace Utility
{
    public abstract class PageProducer
    {
        public abstract void Execute();
        public abstract int NumPages { get; }
    }

    public class EBayPageProducer : PageProducer
    {
        const int intENTRIES_PER_PAGE = 200;
        private TransactionTypeCollection transactionCollection;
        private int numberOfPages;
        private int pageNumber;
        GetSellerTransactionsCall apiCall;

        public override string ToString()
        {
            string rs = String.Empty;
            if (transactionCollection != null)
            {
                foreach (TransactionType i in transactionCollection)
                {
                    //rs += String.Format("UserID: {0}\tTransactioNID: {1}\tBuyer Name: {2}", i.Buyer.UserID, i.TransactionID, i.Buyer.BuyerInfo.ShippingAddress.Name));
                    rs += String.Format("{0} {1} {2}", i.TransactionID, i.Buyer.UserID, i.Item.Title);
                }
            }
            return rs;
        }

        public override int NumPages
        {
            get
            {
                return numberOfPages;
            }
        }

        public EBayPageProducer()
        {
        }

        public EBayPageProducer(ApiContext ApiContext, int PageNumber)
        {
            pageNumber = PageNumber;
            apiCall = new GetSellerTransactionsCall(ApiContext);
            apiCall.DetailLevelList = new DetailLevelCodeTypeCollection(new DetailLevelCodeType[] { DetailLevelCodeType.ReturnAll });
            apiCall.ApiRequest.OutputSelector = new StringCollection(new string[] { "TransactionID", "PaginationResult", "TransactionArray.Transaction.Buyer.UserID", "TransactionArray.Transaction.Item.Title" });
            apiCall.Pagination = new PaginationType() { EntriesPerPage = intENTRIES_PER_PAGE, PageNumber = PageNumber };
        }

        public override void Execute()
        {
            Log.AddLogInfo(String.Format("Thread {0} started, page {1}", Thread.CurrentThread.ManagedThreadId, pageNumber));
            apiCall.Execute();
            Log.AddLogInfo(String.Format("Getting item list - START, page {0}", pageNumber));
            transactionCollection = apiCall.GetSellerTransactions(new TimeFilter() { TimeFrom = new DateTime(2012, 8, 1), TimeTo = new DateTime(2012, 8, 30) });
            numberOfPages = apiCall.PaginationResult.TotalNumberOfPages;
            Log.AddLogInfo(String.Format("Getting item list - SUCCESS, page {0}", pageNumber));
            Log.AddLogInfo(String.Format("Thread {0} done, page {1}", Thread.CurrentThread.ManagedThreadId, pageNumber));
        }
    }

    public class EBayPageProducerEx : PageProducer
    {
        const int intENTRIES_PER_PAGE = 200;
        private OrderTypeCollection orderCollection;
        private int numberOfPages;
        private int pageNumber;
        GetOrdersCall apiCall;

        public override string ToString()
        {
            string rs = String.Empty;
            if (orderCollection != null)
            {
                foreach (TransactionType i in orderCollection)
                {
                    //rs += String.Format("UserID: {0}\tTransactioNID: {1}\tBuyer Name: {2}", i.Buyer.UserID, i.TransactionID, i.Buyer.BuyerInfo.ShippingAddress.Name));
                    rs += String.Format("{0} {1} {2}", i.TransactionID, i.Buyer.UserID, i.Item.Title);
                }
            }
            return rs;
        }

        public override int NumPages
        {
            get
            {
                return numberOfPages;
            }
        }

        public EBayPageProducerEx()
        {
        }

        public EBayPageProducerEx(ApiContext ApiContext, int PageNumber)
        {
            pageNumber = PageNumber;
            apiCall = new GetOrdersCall(ApiContext);
            apiCall.DetailLevelList = new DetailLevelCodeTypeCollection(new DetailLevelCodeType[] { DetailLevelCodeType.ReturnAll });
            apiCall.ApiRequest.OutputSelector = new StringCollection(new string[] { "TransactionID", "PaginationResult", "TransactionArray.Transaction.Buyer.UserID", "TransactionArray.Transaction.Item.Title" });
            apiCall.Pagination = new PaginationType() { EntriesPerPage = intENTRIES_PER_PAGE, PageNumber = PageNumber };
        }

        public override void Execute()
        {
            Log.AddLogInfo(String.Format("Thread {0} started, page {1}", Thread.CurrentThread.ManagedThreadId, pageNumber));
            apiCall.Execute();
            Log.AddLogInfo(String.Format("Getting item list - START, page {0}", pageNumber));
            orderCollection = apiCall.GetOrders(
                new TimeFilter() { TimeFrom = new DateTime(2012, 8, 1), TimeTo = new DateTime(2012, 8, 30) },
                TradingRoleCodeType.Buyer,
                OrderStatusCodeType.Active);
            numberOfPages = apiCall.PaginationResult.TotalNumberOfPages;
            Log.AddLogInfo(String.Format("Getting item list - SUCCESS, page {0}", pageNumber));
            Log.AddLogInfo(String.Format("Thread {0} done, page {1}", Thread.CurrentThread.ManagedThreadId, pageNumber));
        }
    }
}
