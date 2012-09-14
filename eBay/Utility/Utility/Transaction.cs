using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utility
{
    public class Transaction
    {
        public string TransactionId { get; set; }

        public string OrderID { get; set; }

        public string ItemID { get; set; }

        public int SellingManagerRecordNumber { get; set; }

        public DateTime CreatedTime { get; set; }

        public eBay.Service.Core.Soap.OrderStatusCodeType OrderStatus { get; set; }
    }
}
