using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetflixReviewSericeAPI.Interfaces;
using NetflixReviewSericeAPI.Models;

namespace NetflixReviewSericeAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReviewController : Controller
    {
        private readonly IReviewService reviewService;
        private readonly ILogger<ReviewController> logger;

        public ReviewController(IReviewService reviewService, ILogger<ReviewController> logger)
        {
            this.reviewService = reviewService;
            this.logger = logger;
        }

        [HttpPost]
        public APIResponse Post([FromBody] Review request)
        {
            try
            {
                if(request.Rating < 1 || request.Rating > 5)
                {
                    throw new Exception("Rating is out of range. Rating can be 1-5.");
                }
                if(string.IsNullOrWhiteSpace(request.Description) || request.Description.Length > 255)
                {
                    throw new Exception("Description must be between 1 and 255 characters.");
                }
                if (string.IsNullOrWhiteSpace(request.ShowID)) 
                {
                    throw new Exception("Show ID is required.");
                }

                var result = reviewService.Add(request);

                if (!result)
                {
                    throw new Exception("Service response is false.");
                }

                return new APIResponse { ResponseStatusCode = ResponseStatusCode.Success, Data = result, Message = "Review was added successfully." };
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, request.Description, request.Rating, request.ShowID);
                return new APIResponse { ResponseStatusCode = ResponseStatusCode.Error, Message = $"Failed to add review! :: {ex.Message}" };
            }
        }
    }
}
