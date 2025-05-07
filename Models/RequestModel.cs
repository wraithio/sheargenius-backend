using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sheargenius_backend.Models
{
    public class RequestModel
    {
        public int Id {get;set;}
        public string? Username {get;set;}
        public string? BarberName {get;set;}
        public string? Day {get;set;}
        public string? Time {get;set;}
        public bool isAccepted {get;set;}
        

    }
}