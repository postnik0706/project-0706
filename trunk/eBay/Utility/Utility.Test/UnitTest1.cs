using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using eBay.Service.Core.Sdk;

namespace Utility.Test
{
    class TestPageProducer : Utility.PageProducer
    {
        string output = "";

        public override int NumPages
        {
            get
            {
                return 5;
            }
        }

        public TestPageProducer()
        {
        }

        public TestPageProducer(ApiContext ApiContext, int PageNumber)
        {
            pageNumber = PageNumber;
        }

        public override string ToString()
        {
            return output;
        }

        public override void Execute(Object obj)
        {
            output = "Page " + pageNumber.ToString();
        }

        public int pageNumber { get; set; }
    }

    class TestPageObjectFactory : PageObjectFactory
    {
        public override PageProducer CreatePageObject(ApiContext ApiContext, int PageNumber)
        {
            return new TestPageProducer(ApiContext, PageNumber);
        }
    }

    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            Utility.eBayClass.Run();
            //ThreadedRequest r = new ThreadedRequest(null, new TestPageObjectFactory());
            //Console.WriteLine(r.Pages.Count);
        }

        [TestMethod]
        public void TestMoq()
        {
        }
    }
}
