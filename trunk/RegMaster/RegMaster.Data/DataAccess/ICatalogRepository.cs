using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RegMaster.Data.DataAccess
{
    public interface ICatalogRepository
    {
        IQueryable<Category> GetCategories();
    }
}
