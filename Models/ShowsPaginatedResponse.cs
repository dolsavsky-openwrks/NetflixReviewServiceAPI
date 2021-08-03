using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetflixReviewSericeAPI.Models
{
    public class ShowsPaginatedResponse
    {
        public IEnumerable<Show> Data { get; set; }
        public Links Links { get; set; }
        public Pagination Pagination { get; set; }
    }
}
