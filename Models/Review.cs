using System;
using System.Collections.Generic;

namespace NetflixReviewSericeAPI.Models
{
    public class Review
    {
        public Guid ReviewID { get; set; }
        public int Rating { get; set; }
        public string Description { get; set; }
        public string ShowID { get; set; }
        public DateTime DateCreated { get; set; }

        internal IEnumerable<Review> ToList()
        {
            throw new NotImplementedException();
        }
    }
}
