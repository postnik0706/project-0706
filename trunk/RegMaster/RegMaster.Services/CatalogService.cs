using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RegMaster.Data;
using RegMaster.Data.DataAccess;

namespace RegMaster.Services
{
    public class CatalogService
    {
        ICatalogRepository _repository = null;

        /// <summary>
        /// Creates a catalog service based on the passed in repository
        /// </summary>
        /// <param name="repository"></param>
        public CatalogService(ICatalogRepository repository)
        {
            _repository = repository;
            if (_repository == null)
                throw new InvalidOperationException("Repository cannot be null");
        }

        /// <summary>
        /// Get Categories and Subcategories from the DB
        /// </summary>
        /// <returns></returns>
        public IList<Category> GetCategories()
        {
            IList<Category> cat = _repository.GetCategories().ToList();

            var parents = (from c in cat
                          where (!c.ParentID.HasValue)
                          select c).ToList();

            parents.ForEach(p =>
                {
                    p.SubCategories = (
                        from subs in cat
                        where subs.ParentID == p.ID
                        select subs).ToList();
                });
            
            return parents;
        }
    }
}
