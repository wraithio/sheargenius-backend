using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.Identity;

namespace sheargenius_backend.Models
{
    public class CommentModel
    {
        public int Id {get;set;}
        public string? username {get;set;}
        public string? comment {get;set;}
    }
}