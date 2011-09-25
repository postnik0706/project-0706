using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RegMaster.Data
{
    public class Product
    {
        public Product(string Name, string Description, int CategoryID, Decimal DiscountPercent, int ListPrice)
        {
            // TODO: Complete member initialization
            this.Name = Name;
            this.Description = Description;
            this.CategoryID = CategoryID;
            this.DiscountPercent = DiscountPercent;
            this.Price = ListPrice;
        }

        public Product()
        {
            // TODO: Complete member initialization
        }
        public int CategoryID
        {
            get; set;
        }

        public String Description
        {
            get; set;
        }

        /// <summary>
        /// Returns the discount amount based on DiscountPercent
        /// </summary>
        public Decimal DiscountAmount
        {
            get
            {
                decimal result = 0;
                if (Price > 0 && DiscountPercent > 0)
                    result =  Price * DiscountPercent / 100;
                return result;
            }
        }

        public decimal DiscountPercent
        {
            get; set;
        }

        /// <summary>
        /// Returns the discount price, based on DiscountAmount and Percent
        /// </summary>
        public decimal DiscountPrice
        {
            get
            {
                return Price - DiscountAmount;
            }
        }

        public int ID
        {
            get; set;
        }

        public decimal Price
        {
            get; set;
        }

        public string Name
        {
            get; set;
        }

        public int LocalizedDescriptions
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public List<ProductReview> Reviews
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }
    }
}
