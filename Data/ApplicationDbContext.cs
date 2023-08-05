using assignment5.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace assignment5.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<UsersInfo> SalesLead { get; set; }

        public DbSet<ApplicationUser> ApplicationUser { get; set; }

        public DbSet<UsersInfo> UsersInfos { get; set; }
    }
}
