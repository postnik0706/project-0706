using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RegMaster.Data
{
    public class Category
    {
        public int ID
        {
            get; set;
        }

        public string Name
        {
            get; set;
        }

        public int? ParentID
        {
            get; set;
        }

        public IList<Category> SubCategories
        {
            get; set;
        }
    }
}
