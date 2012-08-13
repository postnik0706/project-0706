using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using eBay.Service.Core.Sdk;

namespace Utility
{
    public abstract class PageObjectFactory
    {
        public abstract PageProducer CreatePageObject(ApiContext ApiContext, int PageNumber);
    }

    public class EBayPageObjectFactory : PageObjectFactory
    {
        public override PageProducer CreatePageObject(ApiContext ApiContext, int PageNumber)
        {
            return new EBayPageProducer(ApiContext, PageNumber);
        }
    }

}
