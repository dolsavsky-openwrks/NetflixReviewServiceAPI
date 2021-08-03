using System;
using System.Collections.Generic;
using Microsoft.Extensions.Caching.Memory;
using NetflixReviewSericeAPI.Models;

namespace NetflixReviewSericeAPI.Repository
{
    public class ReviewRepo : IReviewRepo
    {
        private readonly IMemoryCache cache;
        private readonly MemoryCacheEntryOptions memoryCacheEntryOptions;
        public ReviewRepo(IMemoryCache cache)
        {
            this.cache = cache;
            memoryCacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(60));
        }
        public bool AddReview(Review review)
        {
            List<Review> reviews = cache.Get<List<Review>>(review.ShowID); ;

            if (reviews != null)
                reviews.Add(review);
            else
            {
                reviews = new List<Review>();
                reviews.Add(review);
            }

            var result = cache.Set(review.ShowID, reviews, memoryCacheEntryOptions);

            return result != null;
        }

        public IEnumerable<ReviewsResult> GetReviews(IEnumerable<string> showIDs)
        {
            var result = new List<ReviewsResult>();

            foreach (var id in showIDs)
            {
                var reviews = cache.Get<List<Review>>(id);

                if (reviews != null)
                {
                    result.Add(new ReviewsResult { 
                        Reviews = reviews,
                        ShowID = id
                    });
                }
            }

            return result;
        }
    }
}
