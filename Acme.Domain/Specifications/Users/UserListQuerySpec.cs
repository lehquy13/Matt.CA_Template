using Acme.Domain.Acme.Users;
using Matt.SharedKernel.Domain.Specifications;

namespace Acme.Domain.Specifications.Users;

public sealed class UserListQuerySpec : GetPaginatedListSpecificationBase<User>
{
    public UserListQuerySpec(
        int pageIndex,
        int pageSize)
        : base(pageIndex, pageSize)
    {
    }
}