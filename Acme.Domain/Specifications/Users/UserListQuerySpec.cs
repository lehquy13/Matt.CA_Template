using Acme.Domain.Acme.Users;
using Matt.SharedKernel.Domain.Specifications;

namespace Acme.Domain.Specifications.Users;

public sealed class UserListQuerySpec(
    int pageIndex,
    int pageSize) : GetPaginatedListSpecificationBase<User>(pageIndex, pageSize);