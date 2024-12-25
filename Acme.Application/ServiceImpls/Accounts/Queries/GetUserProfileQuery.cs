﻿using Acme.Application.Contracts.Users;
using Acme.Domain.Acme.Users;
using Acme.Domain.Acme.Users.ValueObjects;
using MapsterMapper;
using Matt.SharedKernel.Results;
using Matt.SharedKernel.Application.Contracts.Interfaces;
using Matt.SharedKernel.Application.Contracts.Interfaces.Infrastructures;
using Matt.SharedKernel.Application.Mediators.Queries;

namespace Acme.Application.ServiceImpls.Accounts.Queries;

public record GetUserProfileQuery : IQueryRequest<UserProfileDto>, IAuthorizationRequired;

public class GetUserProfileQueryHandler(
    IUserRepository userRepository,
    ICurrentUserService currentUserService,
    IMapper mapper
) : QueryHandlerBase<GetUserProfileQuery, UserProfileDto>
{
    public override async Task<Result<UserProfileDto>> Handle(
        GetUserProfileQuery getAllUserQuery,
        CancellationToken cancellationToken)
    {
        var customer = await userRepository.GetByIdAsync(
            UserId.Create(currentUserService.UserId),
            cancellationToken);

        return customer is null
            ? Result.Fail(AccountServiceErrorConstants.NonExistUserError)
            : mapper.Map<UserProfileDto>(customer);
    }
}