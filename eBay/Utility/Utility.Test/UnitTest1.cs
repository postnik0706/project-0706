using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using eBay.Service.Core.Sdk;
using FakeItEasy;
using eBay.Service.Core.Soap;

namespace Utility.Test
{
    [TestClass]
    public class UnitTest1
    {
        //[TestMethod]
        public void TestMethod1()
        {
            Utility.eBayClass.Run();
            //ThreadedRequest r = new ThreadedRequest(null, new TestPageObjectFactory());
            //Console.WriteLine(r.Pages.Count);
        }

        [TestMethod]
        public void GetOrders_Committed_SingleItem_OrderStatus_Active()
        {
            // Arrange
            IGetOrdersCall getOrdersCall = A.Fake<IGetOrdersCall>(x => x.OnFakeCreated(
                t => t.ApiContext.ApiLogManager = new ApiLogManager()));

            A.CallTo(() => getOrdersCall.GetOrders(
                A<TimeFilter>.Ignored,
                A<TradingRoleCodeType>.Ignored,
                A<OrderStatusCodeType>.Ignored)).Returns(
                    new OrderTypeCollection()
                    {
                        new OrderType()
                        {
                            OrderStatus = OrderStatusCodeType.Active,
                            TransactionArray = new TransactionTypeCollection()
                            {
                                new TransactionType()
                                {
                                    Item = new ItemType()
                                    {
                                        
                                    },
                                    ShippingDetails = new ShippingDetailsType()
                                    {
                                        
                                    }
                                }
                            }
                        }
                    });

            // Act
            List<Transaction> rs = eBayClass.GetOrders(getOrdersCall,
                DateTime.Now, DateTime.Now);

            // Assert
            rs[0].OrderStatus = OrderStatusCodeType.Active;
        }
    }
}