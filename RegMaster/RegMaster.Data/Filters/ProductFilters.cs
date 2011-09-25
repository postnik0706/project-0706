using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RegMaster.Data
{
    public static class ProductFilters
    {
        /// <summary>
        /// Filters the product by category ID
        /// </summary>
        /// <param name="qry"></param>
        /// <param name="categoryID"></param>
        /// <returns></returns>
        public static IQueryable<Product> WithCategory(
            this IQueryable<Product> qry, int categoryID)
        {
            return from p in qry
                   where p.CategoryID == categoryID
                   select p;
        }

        public static IQueryable<Product> WithID(
            this IQueryable<Product> qry,
            int ProductID)
        {
            return from p in qry
                   where p.ID == ProductID
                   select p;
        }
    }
}
