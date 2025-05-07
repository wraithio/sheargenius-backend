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
        public string[]? MondayTimes {get;set;}
        public string[]? TuesdayTimes {get;set;}
        public string[]? WednesdayTimes {get;set;}
        public string[]? ThursdayTimes {get;set;}
        public string[]? FridayTimes {get;set;}
        public string[]? SaturdayTimes {get;set;}
        public string[]? SundayTimes {get;set;}

    }
}