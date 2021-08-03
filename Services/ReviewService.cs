using System.Collections.Generic;
using System.Linq;
using NetflixReviewSericeAPI.Interfaces;
using NetflixReviewSericeAPI.Models;
using NetflixReviewSericeAPI.Repository;

namespace NetflixReviewSericeAPI.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepo reviewRepo;

        public ReviewService(IReviewRepo reviewRepo)
        {
            this.reviewRepo = reviewRepo;
        }

        public bool Add(Review review)
        {
            var result = reviewRepo.AddReview(review);

            return result;
        }

        public IEnumerable<Show> GetReviews(IEnumerable<Show> shows)
        {

            var showIDs = shows.Select(x => x.ID);

            var allReviews = reviewRepo.GetReviews(showIDs);

            if(allReviews != null && allReviews.Count() > 0)
            {
                shows = shows.Select(show => {
                    show.Reviews = allReviews.Where(x => x.ShowID == show.ID);
                    return show;
                });
            }

            return shows;
        }
    }
}
