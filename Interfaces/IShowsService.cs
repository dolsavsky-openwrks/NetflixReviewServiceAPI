using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NetflixReviewSericeAPI.Models;

namespace NetflixReviewSericeAPI.Interfaces
{
    public interface IShowsService
    {
        public IEnumerable<Show> GetAll();
    }
}
