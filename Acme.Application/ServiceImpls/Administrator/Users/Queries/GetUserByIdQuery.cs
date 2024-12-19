using Acme.Application.Contracts.Users;
using Acme.Domain.Acme.Users;
using Acme.Domain.Acme.Users.ValueObjects;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Contracts.Interfaces;
using Matt.SharedKernel.Application.Mediators.Queries;

namespace Acme.Application.ServiceImpls.Administrator.Users.Queries;

public record GetUserByIdQuery(Guid Id) : IQueryRequest<UserDetailDto>, IAuthorizationRequired;

public class GetUserByIdQueryHandler(
    IUserRepository userRepository
) : QueryHandlerBase<GetUserByIdQuery, UserDetailDto>
{
    public override async Task<Result<UserDetailDto>> Handle(GetUserByIdQuery getAllUserQuery,
        CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(UserId.Create(getAllUserQuery.Id), cancellationToken);

        if (user is null)
        {
            return Result.Fail(UserAppServiceError.UserNotFound);
        }

        var userDetailDto = new UserDetailDto(
            user.Id.Value,
            user.GetFullName(),
            user.Email,
            user.Gender.ToString(),
            user.BirthYear);

        return userDetailDto;
    }
}