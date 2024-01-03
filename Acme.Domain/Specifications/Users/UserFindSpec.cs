using Acme.Domain.Acme.Users.Identities;
using Acme.Domain.Acme.Users.ValueObjects;
using Matt.SharedKernel.Domain.Specifications;

namespace Acme.Domain.Specifications.Users;

public class UserFindSpec : FindSpecificationBase<IdentityUser, IdentityGuid>
{
    public UserFindSpec(IdentityGuid id) : base(id)
    {
        IncludeStrings.Add(nameof(IdentityUser.IdentityRole));
        IncludeStrings.Add(nameof(IdentityUser.User));
    }
}