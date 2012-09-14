using System;
using System.Collections.Generic;
using System.Text;
using eBay.Service.Core.Soap;
using eBay.Service.Call;
using eBay.Service.Core.Sdk;

namespace Utility
{
    public interface IGetOrdersCall
    {
        ApiContext ApiContext { get; }
        DetailLevelCodeTypeCollection DetailLevelList { get; set; }
        GetOrdersRequestType ApiRequest { get; set; }
        PaginationType Pagination { get; set; }
        PaginationResultType PaginationResult { get; }
        
        OrderTypeCollection GetOrders(TimeFilter Filter, TradingRoleCodeType OrderRole, OrderStatusCodeType OrderStatus);
    }

    public class GetOrdersCall_ : IGetOrdersCall
    {
        ApiContext apiContext;
        GetOrdersCall ebayGetOrdersCall;

        public GetOrdersCall_(ApiContext ApiContext)
        {
            apiContext = ApiContext;
            ebayGetOrdersCall = new GetOrdersCall(ApiContext);
        }

        public ApiContext ApiContext
        {
            get
            {
                return apiContext;
            }
        }
        
        public DetailLevelCodeTypeCollection DetailLevelList
        {
            get
            {
                return ebayGetOrdersCall.DetailLevelList;
            }
            set
            {
                ebayGetOrdersCall.DetailLevelList = value;
            }
        }

        public GetOrdersRequestType ApiRequest
        {
            get
            {
                return ebayGetOrdersCall.ApiRequest;
            }
            set
            {
                ebayGetOrdersCall.ApiRequest = value;
            }
        }

        public PaginationType Pagination
        {
            get
            {
                return ebayGetOrdersCall.Pagination;
            }
            set
            {
                ebayGetOrdersCall.Pagination = value;
            }
        }

        public PaginationResultType PaginationResult
        {
            get
            {
                return ebayGetOrdersCall.PaginationResult;
            }
        }

        public OrderTypeCollection GetOrders(TimeFilter Filter, TradingRoleCodeType OrderRole, OrderStatusCodeType OrderStatus)
        {
            return ebayGetOrdersCall.GetOrders(Filter, OrderRole, OrderStatus);
        }
    }
}
