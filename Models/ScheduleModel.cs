using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sheargenius_backend.Models
{
    public class ScheduleModel
    {
        public int Id {get;set;}
        public string? Username {get;set;}
        public string[]? Days {get;set;}
        public string[]? Times {get;set;}
    }
}