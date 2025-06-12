using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using InsuranceWebApp.Models;

namespace InsuranceWebApp.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<InsuranceWebApp.Models.Client> Clients { get; set; } = default!;
        public DbSet<InsuranceWebApp.Models.Insurance> Insurance { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Client>()
                .HasOne(c => c.IdentityUser)
                .WithOne()
                .HasForeignKey<Client>(c => c.IdentityUserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
