using Acme.Domain.Acme.Users.Identities;
using Matt.SharedKernel.Domain.Specifications;

namespace Acme.Domain.Specifications.Identities;

public class IdentityRoleFindSpec(Guid id) : FindSpecificationBase<IdentityRole, Guid>(id)
{
}