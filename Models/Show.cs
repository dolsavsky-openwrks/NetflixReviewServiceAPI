﻿using System;
using System.Collections.Generic;

namespace NetflixReviewSericeAPI.Models
{
    public class Show
    {
        public string ID { get; set; }
        public string Type { get; set; }
        public string Title { get; set; }
        public string Director { get; set; }
        public string Cast { get; set; }
        public string Country { get; set; }
        public DateTime DateAdded { get; set; }
        public int ReleaseYear { get; set; }
        public string Rating { get; set; }
        public string Duration { get; set; }
        public string ListedIn { get; set; }
        public string Description { get; set; }
        public IEnumerable<Review> Reviews { get; set; }
    }
}
