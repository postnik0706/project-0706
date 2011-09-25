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
                cat.ID = i;
                cat.Name = String.Format("Parent {0}", i);
                cat.ParentID = null;
                result.Add(cat);

                for (int j = 0; j < 5; j++)
                {
                    Category sub = new Category();
                    sub.ID = j;
                    sub.ParentID = cat.ID;
                    sub.Name = String.Format("Sub {0}", j);
                    result.Add(sub);
                }
            }

            return result.AsQueryable();
        }
    }
}
