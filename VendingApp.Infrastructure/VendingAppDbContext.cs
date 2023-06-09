using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VendingApp.Domain;

namespace VendingApp.Infrastructure
{
    public sealed class VendingAppDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<ApplicationUser> ApplicationUsers { get; set; } = null!;
        public DbSet<Coin> Coins { get; set; } = null!;
        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<VendingMachine> VendingMachines { get; set; } = null!;

        public VendingAppDbContext(DbContextOptions<VendingAppDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

    }
}