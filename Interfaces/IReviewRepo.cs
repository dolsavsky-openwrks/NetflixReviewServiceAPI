using System.Collections.Generic;
using NetflixReviewSericeAPI.Models;

namespace NetflixReviewSericeAPI.Repository
{
    public interface IReviewRepo
    {
        public IEnumerable<ReviewsResult> GetReviews(IEnumerable<string> showIDs);
        public bool AddReview(Review review);
    }
}
