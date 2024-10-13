using Microsoft.EntityFrameworkCore;
using NetDevloperTask.Models;

namespace NetDevloperTask.Data
{
    public class BusinessCardDbContext : DbContext
    {
        public BusinessCardDbContext(DbContextOptions<BusinessCardDbContext> options)
            : base(options)
        {
        }

        public DbSet<BusinessCard> BusinessCards { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BusinessCard>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Email).IsRequired();
                entity.Property(e => e.Phone).IsRequired();
                entity.Property(e => e.Address).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Photo).HasMaxLength(1048576); // 1MB
                entity.Property(e => e.DateOfBirth).IsRequired().HasColumnType("date");
            });
        }
    }
}
