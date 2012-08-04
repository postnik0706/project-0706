using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using eBay.Service.Core.Sdk;
using eBay.Service.Call;
using System.Configuration;
using eBay.Service.Core.Soap;

namespace Utility
{
    public class eBay
    {
        public static ItemType BuildItem()
        {
            ItemType item = new ItemType();

            // Title
            item.Title = "Gadget G100";
            item.Description = "My test gadget";

            item.ListingType = ListingTypeCodeType.Express;

            // listing price
            item.Currency = CurrencyCodeType.USD;
            item.StartPrice = new AmountType() { Value = 15.99, currencyID = CurrencyCodeType.USD };

            // listing duration
            item.ListingDuration = "Days_255";

            // item location and country
            item.Location = "Farmington";
            item.Country = CountryCodeType.US;

            // listing category, games
            item.PrimaryCategory = new CategoryType() { CategoryID = "139973" };

            // item quantity
            item.Quantity = 1200;

            // payment methods
            item.PaymentMethods = new BuyerPaymentMethodCodeTypeCollection(
                new BuyerPaymentMethodCodeType[] { BuyerPaymentMethodCodeType.CashInPerson }
            );

            // item condition, new
            item.ConditionID = 1000;

            // handling time is required
            item.DispatchTimeMax = 5;

            return item;
        }

        public static void Run()
        {
            Debug.WriteLine(
                String.Format("{0}:\tStarting...", DateTime.Now.ToString()));

            ApiContext apiContext = new ApiContext();
            apiContext.SoapApiServerUrl =
                ConfigurationManager.AppSettings["Environment.ApiServerUrl"];
            ApiCredential apiCredential = new ApiCredential();
            apiCredential.eBayToken =
                ConfigurationManager.AppSettings["UserAccount.ApiToken"];
            apiContext.ApiCredential = apiCredential;
            apiContext.Site = global::eBay.Service.Core.Soap.SiteCodeType.US;


            /*******************************************
            Getting server time
            */

            GeteBayOfficialTimeCall apiCall = new GeteBayOfficialTimeCall(apiContext);
            DateTime officialTime = apiCall.GeteBayOfficialTime();

            Debug.WriteLine(
                String.Format("Official eBay time: {0}", officialTime));
        }
    }
}
