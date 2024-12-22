using Acme.Domain.Commons.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Acme.Infrastructure.Persistence.EntityFrameworkCore;

public class AuthDbContext(DbContextOptions<AuthDbContext> options)
    : IdentityDbContext<IdentityUser, IdentityRole, string>(options)
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.EnableSensitiveDataLogging();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<IdentityRole>(builder =>
        {
            var id = 1;

            foreach (var role in EnumProvider.Roles)
            {
                builder.HasData(
                    new IdentityRole
                    {
                        Id = id++.ToString(),
                        Name = role,
                        NormalizedName = role.ToUpper()
                    }
                );
            }
        });
    }
}

public class IdentityDbContextFactory : IDesignTimeDbContextFactory<AuthDbContext>
{
    public AuthDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AuthDbContext>();
        optionsBuilder.UseSqlServer(
            "Server=(localdb)\\MSSQLLocalDB; Database=ca_dev_identity_1; Trusted_Connection=True;MultipleActiveResultSets=true"
        );

        return new AuthDbContext(optionsBuilder.Options);
    }
}