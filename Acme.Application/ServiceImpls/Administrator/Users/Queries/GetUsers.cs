using Acme.Application.Contracts.Users;
using Acme.Application.Interfaces;
using Matt.Paginated;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Contracts.Interfaces;
using Matt.SharedKernel.Application.Mediators.Queries;
using Microsoft.EntityFrameworkCore;

namespace Acme.Application.ServiceImpls.Administrator.Users.Queries;

public record GetUsersQuery(PaginatedParams PaginatedParams)
    : IQueryRequest<PaginatedList<UserDto>>, IAuthorizationRequired;

public class GetUsersQueryHandler(IReadDbContext userRepository)
    : QueryHandlerBase<GetUsersQuery, PaginatedList<UserDto>>
{
    public override async Task<Result<PaginatedList<UserDto>>> Handle(
        GetUsersQuery getUsersQuery,
        CancellationToken cancellationToken)
    {
        var totalUsers = await userRepository.Users.CountAsync(cancellationToken);

        var users = await userRepository.Users
            .Skip((getUsersQuery.PaginatedParams.PageIndex - 1) * getUsersQuery.PaginatedParams.PageSize)
            .Take(getUsersQuery.PaginatedParams.PageSize)
            .ToListAsync(cancellationToken);

        var userDtos = users.Select(x => new UserDto(x.Id.Value, x.GetFullName(), x.Email)).ToList();

        var paginatedList = PaginatedList<UserDto>.Create(
            userDtos,
            getUsersQuery.PaginatedParams.PageIndex,
            getUsersQuery.PaginatedParams.PageSize,
            totalUsers);

        return paginatedList;
    }
}