
namespace NetflixReviewSericeAPI.Models
{
    public class JWTAuthentication
    {
        public string Authorization { get; set; }
        public string GrantType { get; set; }
        public string Scope { get; set; }
        public string ContentType { get; set; }
        public string Endpoint { get; set; }
        public string BaseUrl { get; set; }
    }
}
