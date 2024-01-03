using Acme.Domain.Acme.Users.ValueObjects;
using Matt.SharedKernel.Domain.Primitives;

namespace Acme.Domain.Acme.Users.Identities;

/// <summary>
/// Currently not used.
/// </summary>
public class IdentityUserRole : Entity<Guid>
{
    public IdentityGuid UserId { get; set; } = null!;

    public IdentityGuid RoleId { get; set; } = null!;
}