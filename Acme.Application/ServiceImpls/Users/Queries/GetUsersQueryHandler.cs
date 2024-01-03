using Acme.Application.Contracts.Acme.Users.Queries;
using Acme.Application.Contracts.DataTransferObjects.Users;
using Acme.Domain.Acme.Users;
using Acme.Domain.Acme.Users.ValueObjects;
using Acme.Domain.Specifications.Users;
using MapsterMapper;
using Matt.Paginated;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Mediators.Queries;
using Matt.SharedKernel.Domain.Interfaces;
using Matt.SharedKernel.Domain.Interfaces.Repositories;

namespace Acme.Application.ServiceImpls.Users.Queries;

internal class GetUsersQueryHandler(
    IReadOnlyRepository<User, IdentityGuid> userRepository,
    IMapper mapper,
    IAppLogger<GetUsersQueryHandler> logger,
    IUnitOfWork unitOfWork)
    : QueryHandlerBase<GetUsersQuery, PaginatedList<UserForListDto>>(unitOfWork, logger, mapper)
{
    public override async Task<Result<PaginatedList<UserForListDto>>> Handle(GetUsersQuery query,
        CancellationToken cancellationToken)
    {
        try
        {
            var userSpec = new
                UserListQuerySpec(
                    query.UserFilterParams.PageIndex,
                    query.UserFilterParams.PageSize);
            var totalCount = await userRepository.CountAsync(userSpec, cancellationToken);
            var users = await userRepository.GetListAsync(userSpec, cancellationToken);
            var usersForListDto = Mapper.Map<List<UserForListDto>>(users);

            return PaginatedList<UserForListDto>
                .Create(
                    usersForListDto,
                    query.UserFilterParams.PageIndex,
                    query.UserFilterParams.PageSize,
                    totalCount
                );
        }
        catch (Exception e)
        {
            Logger.LogError("", e.InnerException?.Message ?? e.Message);
            return e;
        }
    }
}