using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RegMaster.Data.DataAccess;
using RegMaster.Data;

namespace RegMaster.Tests
{
    public class TestCatalogRepository : ICatalogRepository
    {
        public IQueryable<Data.Category> GetCategories()
        {
            IList<Category> result = new List<Category>();

            for (int i = 0; i < 2; i++)
            {
                Category cat = new Category();
                cat.ID = i * 6;
                cat.Name = String.Format("Parent {0}", i);
                cat.ParentID = null;
                result.Add(cat);

                for (int j = 1; j <= 5; j++)
                {
                    Category sub = new Category();
                    sub.ID = cat.ID + j;
                    sub.ParentID = cat.ID;
                    sub.Name = String.Format("Sub {0}", j);
                    result.Add(sub);
                }
            }

            return result.AsQueryable();
        }

        public IQueryable<Product> GetProducts()
        {
            IList<Product> result = new List<Product>();
            int uniqueProductID = 1;

            var cat = GetCategories()
                .Where(x => x.ParentID.HasValue)
                .ToList();

            foreach (Category c in cat)
            {
                for (int i = 0; i < 5; i++)
                {
                    Product p = new Product();
                    p.Name = String.Format("Product {0}", i);
                    p.ID = uniqueProductID;
                    p.ListPrice = 5.68M;
                    p.Description = "Test Description";

                    p.CategoryID = c.ID;
                    uniqueProductID++;
                    result.Add(p);
                }
            }

            return result.AsQueryable();
        }
    }
}
