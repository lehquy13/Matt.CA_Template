using Acme.Domain.Acme.Users.Identities;
using Acme.Domain.Acme.Users.ValueObjects;
using Acme.Domain.Shared.User;
using Matt.SharedKernel.Domain.Primitives.Auditing;

namespace Acme.Domain.Acme.Users;

public class User : FullAuditedAggregateRoot<IdentityGuid>
{
    public string FirstName { get; private set; } = string.Empty;

    public string LastName { get; private set; } = string.Empty;

    public Gender Gender { get; private set; } = Gender.Male;

    public int BirthYear { get; private set; } = 1960;

    public Address Address { get; private set; } = new();

    public string Description { get; private set; } = string.Empty;

    public string Avatar { get; private set; } = @"default_avatar";
    
    public IdentityUser IdentityUser { get; set; } = null!;
    

    private User()
    {
    }

    internal static User Create(IdentityGuid identityUserId)
    {
        return new User
        {
            Id = identityUserId
        };
    }
}