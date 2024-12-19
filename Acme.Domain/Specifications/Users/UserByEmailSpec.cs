using Acme.Domain.Acme.Users;
using Matt.SharedKernel.Domain.Specifications;

namespace Acme.Domain.Specifications.Users;

public class UserByEmailSpec : SpecificationBase<User>
{
    public UserByEmailSpec(string email)
    {
        Criteria = u => u.Email == email;
    }
}