using System.Collections.Generic;
using System.Linq;
using NetflixReviewSericeAPI.Interfaces;
using NetflixReviewSericeAPI.Models;
using NetflixReviewSericeAPI.NetflixAPI;

namespace NetflixReviewSericeAPI.Services
{
    public class ShowsService : IShowsService
    {
        private readonly INetflixAPIService netflixAPIService;
        private readonly IReviewService reviewService;

        public ShowsService(INetflixAPIService netflixAPIService, IReviewService reviewService)
        {
            this.netflixAPIService = netflixAPIService;
            this.reviewService = reviewService;
        }

        public IEnumerable<Show> GetAll()
        {
            var tokenResponse = netflixAPIService.GetToken();

            if(tokenResponse == null)
            {
                return null;
            }

            var shows = netflixAPIService.GetShows(tokenResponse);

            if (shows == null || shows.Data == null || shows.Data.ToList().Count() == 0)
            {
                return null;
            }

            var showsWithReviews = reviewService.GetReviews(shows.Data);

            return showsWithReviews;
        }
    }
}
