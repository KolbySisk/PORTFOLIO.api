
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PORTFOLIO.api.Models;

namespace PORTFOLIO.api.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Stuff> Stuff { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Secret> Secrets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Stuff>().ToTable("Stuff");
            modelBuilder.Entity<Contact>().ToTable("Contact");
            modelBuilder.Entity<Secret>().ToTable("Secret");

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<ApplicationUser> ApplicationUser { get; set; }
    }
}
