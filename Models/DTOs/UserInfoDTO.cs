using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sheargenius_backend.Models.DTOs
{
    public class UserInfoDTO
    {
        public int Id {get;set;}
        public string? Username {get;set;}
        public string? Password {get;set;}
        public string? Date {get;set;}
        public string? AccountType {get;set;}
        public string? Name {get;set;}
        public string? Bio {get;set;}
        public string? Email {get;set;}
        public int Rating {get;set;}
        public int RatingCount {get;set;}
        public string[]? Followers{get;set;}
        public string[]? Following{get;set;}
        public int FollowerCount {get;set;}
        public int FollowingCount {get;set;}
        public string? SecurityQuestion {get;set;}
        public string? SecurityAnswer {get;set;}
        public string? ShopName {get;set;}
        public string? Address {get;set;}
        public string? City {get;set;}
        public string? State {get;set;}
        public string? ZIP {get;set;}
        public string? Pfp {get;set;}
        public bool IsDeleted {get;set;}
    }
}