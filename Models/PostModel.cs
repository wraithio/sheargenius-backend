using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sheargenius_backend.Models
{
    public class PostModel
    {
        public int Id {get; set;}
        public int UserId {get;set;}
        public string? PublisherName {get;set;}
        public string? Date {get;set;}
        public string? Caption {get;set;}
        public string? Image {get;set;}
        public int Likes {get;set;}
        public string? Category {get;set;}
        public bool IsPublished {get;set;}
        public bool IsDeleted {get;set;}
        public List<CommentModel>? Comments {get;set;} 
    }
}