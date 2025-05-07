using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sheargenius_backend.Models
{
    public class RatingModel
    {
        public int Rating { get; set; }
        public string? Username { get; set; }
        public string? UserToRate { get; set; }
    }
}