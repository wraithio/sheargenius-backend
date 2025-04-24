using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sheargenius_backend.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        public string? Username { get; set; }
        public string? Salt { get; set; }
        public string? Hash { get; set; }
        public string? Date { get; set; }
        public string? AccountType { get; set; }
        public int Rating { get; set; }
        public string[]? RatingCount { get; set; }
        public string[]? Followers { get; set; }
        public string[]? Following { get; set; }
        public int[]? Likes { get; set; }
        public string? SecurityQuestion { get; set; }
        public string? SecurityAnswer { get; set; }
        public string? Name { get; set; }
        public string? Bio { get; set; }
        public string? Email { get; set; }
        public string? ShopName { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? ZIP { get; set; }
        public string? Pfp { get; set; }
        public bool IsDeleted { get; set; }
    }
}