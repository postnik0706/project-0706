using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RegMaster.Data;
using RegMaster.Data.DataAccess;
using RegMaster.Services;

namespace RegMaster.Tests
{
    [TestClass]
    public class CatalogTests
    {
        CatalogService catalogService;

        [TestInitialize]
        public void Setup()
        {
            ICatalogRepository rep = new TestCatalogRepository();
            catalogService = new CatalogService(rep);
        }
        
        #region Product Tests

        [TestMethod]
        public void Product_Should_Have_Name_Description_Category_Price_Discount_Fields()
        {
            Product p = new Product("TestName", "TestDescription", 10, 100, 20);

            Assert.AreEqual("TestName", p.Name);
            Assert.AreEqual("TestDescription", p.Description);
            Assert.AreEqual(10, p.CategoryID);
            Assert.AreEqual(100, p.DiscountPercent);
            Assert.AreEqual(20, p.Price);
        }
		
        [TestMethod]
        public void Product_Discount_Amount_IsValid()
        {
            Product p = new Product();
            p.Price = 100;
            p.DiscountPercent = 40;

            Assert.AreEqual(40, p.DiscountAmount);
        }

        [TestMethod]
        public void Product_Discount_Price_Is_Valid()
        {
            Product p = new Product();
            p.Price = 100;
            p.DiscountPercent = 40;

            Assert.AreEqual(60, p.DiscountPrice);
        }

        #endregion

        #region CatalogRepository
        [TestMethod]
        public void CatalogRepository_Contains_Products()
        {
            ICatalogRepository r = new TestCatalogRepository();
            Assert.IsNotNull(r.GetProducts());
        }

        [TestMethod]
        public void CatalogRepository_Each_Category_Contains_5_Products()
        {
            ICatalogRepository rep = new TestCatalogRepository();

            var cat = rep.GetCategories()
                .Where(x => x.ParentID.HasValue)
                .ToList();
            Assert.AreEqual(10, cat.Count());

            var prod = rep.GetProducts();
            foreach (Category c in cat)
            {
                int prodCount = (from p in prod
                                 where p.CategoryID == c.ID
                                 select p).Count();
                Assert.AreEqual(5, prodCount, String.Format("For category {0}", c.ID));
            }

            Assert.IsNotNull(rep.GetProducts());
        }

        [TestMethod]
        public void CatalogRepository_Has_Category_Filter_ForProducts()
        {
            ICatalogRepository rep = new TestCatalogRepository();

            IList<Product> prod = rep.GetProducts()
                .WithCategory(11)
                .ToList();
            Assert.IsNotNull(prod);
        }

        [TestMethod]
        public void CatalogRepository_ProductFilter_Returns_5_Products_With_Category_11()
        {
            ICatalogRepository rep = new TestCatalogRepository();

            Assert.AreEqual(5, rep.GetProducts()
                .WithCategory(11)
                .Count());
        }

        [TestMethod]
        public void CatalogRepository_Returns_Single_Product_When_Filtered_By_ID_1()
        {
            ICatalogRepository rep = new TestCatalogRepository();

            Assert.AreEqual(1,
                rep.GetProducts()
                    .WithID(1)
                    .ToList().Count());
        }

        #endregion

        #region CatalogService Tests

        [TestMethod]
        public void CatalogRepository_Repository_Is_Not_Null()
        {
            ICatalogRepository rep = new TestCatalogRepository();
            Assert.IsNotNull(rep.GetCategories());
        }

        [TestMethod]
        public void CatalogService_Can_Get_Categories_From_Service()
        {
            IList<Category> cat = catalogService.GetCategories();
            Assert.IsTrue(cat.Count > 0);
        }

        [TestMethod]
        public void CatalogService_Can_Group_ParentCategories()
        {
            IList<Category> cat = catalogService.GetCategories();
            Assert.AreEqual(2, cat.Count);
        }

        [TestMethod]
        public void CatalogService_Can_Group_SubCategories()
        {
            IList<Category> cat = catalogService.GetCategories();
            Assert.AreEqual(5, cat[0].SubCategories.Count());
        }

        [TestMethod]
        public void CatalogService_Returns_5_Products_With_Category_11()
        {
            IList<Product> products = catalogService.GetProductsByCategory(11);
            Assert.AreEqual(5, products.Count);
        }
 
        [TestMethod]
        public void CatalogService_Returns_Single_Product_With_ProductID_1()
        {
            Product p = catalogService.GetProductByID(1);
            Assert.IsNotNull(p);
        }

	    #endregion
    }
}