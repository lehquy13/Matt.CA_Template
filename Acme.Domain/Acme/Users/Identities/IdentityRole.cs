using Matt.SharedKernel.Domain.Primitives;

namespace Acme.Domain.Acme.Users.Identities;

public class IdentityRole : AggregateRoot<Guid>
{
    public string Name { get; set; } = string.Empty;

    private IdentityRole()
    {
    }
    
    public static IdentityRole Create(string name)
    {
        return new IdentityRole
        {
            Id = Guid.NewGuid(),
            Name = name
        };
    }
}