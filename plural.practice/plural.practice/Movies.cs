using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;

namespace plural.practice
{
    public class Movie
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public DateTime ReleaseDate { get; set; }

        public virtual ICollection<Review> Reviews { get; set; }
    }

    public class Review
    {
        public int ReviewID { get; set; }
        public string Summary { get; set; }
        public int Rating { get; set; }
        public string Body { get; set; }
        public string Reviewer { get; set; }
    }

    public class MovieReviews : DbContext
    {
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Review> Reviews { get; set; }
    }
}
