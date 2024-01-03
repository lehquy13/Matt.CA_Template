using Acme.Domain.Acme.Users.Identities;
using Matt.SharedKernel.Domain.Specifications;

namespace Acme.Domain.Specifications.Users;

public class UserGetByEmailSpec : SpecificationBase<IdentityUser>
{
    public UserGetByEmailSpec(string email)
    {
        Criteria = user => user.Email == email;
    }
}