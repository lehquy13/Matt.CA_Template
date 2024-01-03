using Acme.Application.Contracts.DataTransferObjects.Users;
using Matt.Paginated;
using Matt.SharedKernel.Application.Mediators.Queries;

namespace Acme.Application.Contracts.Acme.Users.Queries;

public record GetUsersQuery(PaginatedParams UserFilterParams) : IQueryRequest<PaginatedList<UserForListDto>>;