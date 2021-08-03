using System;
using RestSharp;
using System.Text.Json;
using NetflixReviewSericeAPI.Models;
using Microsoft.Extensions.Options;
using NetflixReviewSericeAPI.Models.AppSettings;

namespace NetflixReviewSericeAPI.NetflixAPI
{
    public class NetflixAPIService : INetflixAPIService
    {
        protected IRestRequest restRequest { get; private set; }
        protected IRestClient restClient { get; private set; }
        private JWTAuthentication jwtAuthentication;
        private Shows shows;

        public NetflixAPIService(IRestClient restClient, IOptions<JWTAuthentication> jwtAuthentication,
            IOptions<Shows> shows)
        {
            this.restClient = restClient;
            this.jwtAuthentication = jwtAuthentication.Value;
            this.shows = shows.Value;
        }

        public TokenResponse GetToken() {
           
            restClient.BaseUrl = new Uri(jwtAuthentication.BaseUrl);
            restRequest = new RestRequest(jwtAuthentication.Endpoint, Method.POST);
            restRequest.AddHeader("Authorization", jwtAuthentication.Authorization);
            restRequest.AddHeader("Content-Type", jwtAuthentication.ContentType);

            restRequest.AddParameter("grant_type", jwtAuthentication.GrantType);
            restRequest.AddParameter("scope", jwtAuthentication.Scope);

            var response = restClient.Execute(restRequest);

            return JsonSerializer.Deserialize<TokenResponse>(response?.Content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public ShowsPaginatedResponse GetShows(TokenResponse token)
        {
            restClient.BaseUrl = new Uri(shows.BaseUrl);

            restRequest = new RestRequest(shows.Endpoint, Method.GET);
            restRequest.AddHeader("Authorization", $"{token.Token_type} {token.Access_token}");
            var response = restClient.Execute(restRequest);

            return JsonSerializer.Deserialize<ShowsPaginatedResponse>(response?.Content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
    }
}
