using System;
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
            review.ReviewID = Guid.NewGuid();
            review.DateCreated = DateTime.Now;
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
                    var reviews = allReviews.FirstOrDefault(x => x.ShowID == show.ID);
                    if(reviews != null)
                    {
                        show.Reviews = reviews.Reviews;
                    }
                    return show;
                });
            }

            return shows;
        }
    }
}
