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
            this.ListPrice = ListPrice;
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
                if (ListPrice > 0 && DiscountPercent > 0)
                    result =  ListPrice * DiscountPercent / 100;
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
                return ListPrice - DiscountAmount;
            }
        }

        public int ID
        {
            get; set;
        }

        public string LargePhotoFilename
        {
            get; set;
        }

        public decimal ListPrice
        {
            get; set;
        }

        public string Name
        {
            get; set;
        }

        public string SummaryDescription
        {
            get; set;
        }

        public string ThumbnaleFileName
        {
            get; set;
        }
    }
}
