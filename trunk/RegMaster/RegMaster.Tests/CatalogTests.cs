﻿using System;
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
            Assert.AreEqual(20, p.ListPrice);
        }
		
        [TestMethod]
        public void Product_Discount_Amount_IsValid()
        {
            Product p = new Product();
            p.ListPrice = 100;
            p.DiscountPercent = 40;

            Assert.AreEqual(40, p.DiscountAmount);
        }

        [TestMethod]
        public void Product_Discount_Price_Is_Valid()
        {
            Product p = new Product();
            p.ListPrice = 100;
            p.DiscountPercent = 40;

            Assert.AreEqual(60, p.DiscountPrice);
        }
 
	    #endregion

        #region Service Tests

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
 
	    #endregion
    }
}
