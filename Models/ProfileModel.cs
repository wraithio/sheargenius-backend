using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sheargenius_backend.Models
{
    public class ProfileModel
    {
        public UserDTO User;
        public string? Name {get;set;}
        public string? Bio {get;set;}
        public string? Email {get;set;}
        public string? ShopName {get;set;}
        public string? Address {get;set;}
        public string? City {get;set;}
        public string? State {get;set;}
        public string? ZIP {get;set;}
    }
}