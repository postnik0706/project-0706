using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RegMaster.SqlRepository;

namespace RegMaster.Data.DataAccess
{
    public class SqlCatalogRepository : ICatalogRepository
    {
        public IQueryable<Category> GetCategories()
        {
            DB ctx = new DB();

            return from cat in ctx.Categories
                   select new Category
                   {
                       ID = cat.CategoryID,
                       Name = cat.CategoryName,
                       ParentID = cat.ParentID ?? 0
                   };
        }

        public IQueryable<Product> GetProducts()
        {
            throw new NotImplementedException();
        }
    }
}
