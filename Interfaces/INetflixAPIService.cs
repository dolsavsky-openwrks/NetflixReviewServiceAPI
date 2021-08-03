using NetflixReviewSericeAPI.Models;

namespace NetflixReviewSericeAPI.NetflixAPI
{
    public interface INetflixAPIService
    {
        public TokenResponse GetToken();
        public ShowsPaginatedResponse GetShows(TokenResponse token);
    }
}
