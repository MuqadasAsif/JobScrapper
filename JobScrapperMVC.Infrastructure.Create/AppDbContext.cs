using JobScrapperMVC.Domain;
using Microsoft.EntityFrameworkCore;

namespace JobScrapperMVC.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext (DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
    }
}
