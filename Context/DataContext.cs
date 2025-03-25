using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using sheargenius_backend.Models;
using Microsoft.EntityFrameworkCore;


namespace sheargenius_backend.Context
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {
            
        }

        public DbSet<UserModel> Users {get;set;}
        public DbSet<PostModel> Posts {get;set;}
    }
}