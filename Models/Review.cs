using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetflixReviewSericeAPI.Models
{
    public class Review
    {
        public int Rating { get; set; }
        public string Description { get; set; }
        public string ShowID { get; set; }

        internal IEnumerable<Review> ToList()
        {
            throw new NotImplementedException();
        }
    }
}
