using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetflixReviewSericeAPI.Interfaces;
using NetflixReviewSericeAPI.Models;

namespace NetflixReviewSericeAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ShowsController : ControllerBase
    {
        private readonly ILogger<ShowsController> logger;
        private readonly IShowsService showsService;
        public ShowsController(ILogger<ShowsController> logger, IShowsService showsService)
        {
            this.logger = logger;
            this.showsService = showsService;
        }
        
        [HttpGet]
        public APIResponse Get()
        {
            try
            {
                var result = showsService.GetAll();

                if(result == null)
                {
                    throw new Exception("Service response is null.");
                }

                return new APIResponse { ResponseStatusCode = ResponseStatusCode.Success, Data = result };
            }
            catch (Exception ex) {
                logger.LogError(ex.Message);
                return new APIResponse { ResponseStatusCode = ResponseStatusCode.Error, Message = $"Failed to get shows! :: {ex.Message}" };
            }
        }
    }
}
