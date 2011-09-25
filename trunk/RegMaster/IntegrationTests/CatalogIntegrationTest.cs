using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RegMaster.Data.DataAccess;
using RegMaster.Data;

namespace IntegrationTests
{
    [TestClass]
    public class CatalogIntegrationTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            SqlCatalogRepository rep = new SqlCatalogRepository();

            IQueryable<Category> qry = rep.GetCategories();
            Assert.IsNotNull(qry);

            IList<Category> catList = (from c in qry
                                       where c.ID == 1
                                       select c).ToList<Category>();

            Assert.AreEqual(1, catList.Count);
            Assert.AreEqual("Category 1", catList[0].Name);
        }
    }
}
