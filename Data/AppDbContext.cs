using Microsoft.EntityFrameworkCore;
using SimpleDeviceAPI.Models;

namespace SimpleDeviceAPI.Data;

public sealed class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Device> Devices => Set<Device>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Device>(e =>
        {
            e.ToTable("devices");
            e.HasKey(x => x.Id);

            e.Property(x => x.Name)
                .HasMaxLength(200)
                .IsRequired();

            e.Property(x => x.Status)
                .HasMaxLength(32)
                .IsRequired();

            e.HasIndex(x => x.Name);
            e.HasIndex(x => x.Status);
        });
    }
}
