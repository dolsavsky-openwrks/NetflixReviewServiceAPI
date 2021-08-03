using System.Collections.Generic;

namespace NetflixReviewSericeAPI.Models
{
    public class ReviewsResult
    {
        public string ShowID { get; set; }
        public IEnumerable<Review> Reviews { get; set; }
    }
}
