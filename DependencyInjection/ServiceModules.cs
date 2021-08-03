using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NetflixReviewSericeAPI.Interfaces;
using NetflixReviewSericeAPI.NetflixAPI;
using NetflixReviewSericeAPI.Repository;
using NetflixReviewSericeAPI.Services;
using RestSharp;

namespace NetflixReviewSericeAPI.DependencyInjection
{
    public static class ServiceModules
    {

        public static IServiceCollection AddServiceModules(this IServiceCollection services, IConfiguration config)
        {
            services.AddScoped<IShowsService, ShowsService>();
            services.AddScoped<IReviewService, ReviewService>();
            services.AddScoped<IRestClient, RestClient>();
            services.AddScoped<INetflixAPIService, NetflixAPIService>();
            services.AddScoped<IReviewRepo, ReviewRepo>();
            
            //services.AddSingleton<ILoggingService, LoggingService>();

            return services;
        }
    }
}
