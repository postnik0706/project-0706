using System;
using System.Collections.Generic;
using System.Text;
using eBay.Service.Core.Soap;

namespace Utility
{
    public class Transaction
    {
        public string OrderID { get; set; }
        
        public string ItemID { get; set; }

        public int SellingManagerRecordNumber { get; set; }

        public DateTime CreatedTime { get; set; }

        public string TransactionId { get; set; }

        public OrderStatusCodeType OrderStatus { get; set; }
    }
}
