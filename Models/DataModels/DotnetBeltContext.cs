using System;
using Microsoft.EntityFrameworkCore;

namespace DotNet_Belt.Models
{
    public class DotnetBeltContext: DbContext
    {
       public DotnetBeltContext(DbContextOptions options): base(options){}

       public DbSet<User> Users { get; set; }
       public DbSet<DojoActivity> Activities { get; set; }
       public DbSet<UserActivity> UserActivity { get; set; }
    }
}