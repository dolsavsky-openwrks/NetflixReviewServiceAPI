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
            var result = cache.Set(review.ShowID, review, memoryCacheEntryOptions);

            return result != null;
        }

        public IEnumerable<Review> GetReviews(IEnumerable<string> showIDs)
        {
            var result = new List<Review>();

            foreach (var id in showIDs)
            {
                var review = cache.Get<Review>(id);

                if(review != null)
                {
                    result.Add(review);
                }
            }

            return result;
        }
    }
}
