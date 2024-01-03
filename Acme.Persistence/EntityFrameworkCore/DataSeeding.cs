using Acme.Domain.Acme.Users.Identities;

namespace Acme.Persistence.EntityFrameworkCore;

public class DataSeeding
{
    internal static void SeedData(AppDbContext dbContext)
    {
        if (!dbContext.IdentityRoles.Any())
        {
            var roles = new List<IdentityRole>
            {
                IdentityRole.Create("User"),
                IdentityRole.Create("Admin")
            };

            var admin = IdentityUser.Create(
                "admin",
                "admin@matt.com",
                "123456",
                "123456789",
                roles.First().Id
            );

            dbContext.IdentityRoles.AddRange(roles);
            dbContext.IdentityUsers.Add(admin);
            dbContext.SaveChanges();
        }
    }
}