using System.Collections.Generic;
using NetflixReviewSericeAPI.Models;

namespace NetflixReviewSericeAPI.Interfaces
{
    public interface IReviewService
    {
        public bool Add(Review review);
        public IEnumerable<Show> GetReviews(IEnumerable<Show> shows);
    }
}
