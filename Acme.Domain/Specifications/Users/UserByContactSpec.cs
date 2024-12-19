using Acme.Domain.Acme.Users;
using Matt.SharedKernel.Domain.Specifications;

namespace Acme.Domain.Specifications.Users;

public class UserByContactSpec : SpecificationBase<User>
{
    public UserByContactSpec(string contact)
    {
        Criteria = u => u.PhoneNumber == contact;
    }
}