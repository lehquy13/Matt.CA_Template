using Acme.Domain.Acme.Users;
using Acme.Domain.Acme.Users.Identities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Acme.Persistence.EntityFrameworkCore;

internal class AppDbContext(DbContextOptions<AppDbContext> options)
    : DbContext(options)
{
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<IdentityUser> IdentityUsers { get; set; } = null!;
    public DbSet<IdentityRole> IdentityRoles { get; set; } = null!;

    //public DbSet<IdentityUserRole> IdentityUserRoles { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}

//using to support adding migration
internal class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

        optionsBuilder.UseSqlServer(
            "Server=(LocalDb)\\MSSQLLocalDB;Database=ACP_MultiTenant;Trusted_Connection=True;TrustServerCertificate=True"
        );

        return new AppDbContext(optionsBuilder.Options);
    }
}