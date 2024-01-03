using Acme.Domain.Acme.Users.ValueObjects;
using Matt.SharedKernel.Domain.Interfaces.Repositories;

namespace Acme.Domain.Acme.Users.Identities;

public interface IIdentityRepository : IRepository<IdentityUser, IdentityGuid>
{
    Task<IdentityUser?> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken = default);
    Task<IdentityUser?> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken = default);

    Task<IdentityRole?> GetRolesAsync(IdentityGuid userId, CancellationToken cancellationToken = default);
    Task<IdentityUser?> CheckExistingAccount(string email, string userName);
}