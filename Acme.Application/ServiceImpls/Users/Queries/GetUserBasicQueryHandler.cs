using Acme.Application.Contracts.Acme.Users.Queries;
using Acme.Application.Contracts.DataTransferObjects.Users;
using Acme.Domain.Acme.Users.Identities;
using Acme.Domain.Acme.Users.ValueObjects;
using Acme.Domain.Specifications.Users;
using MapsterMapper;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Mediators.Queries;
using Matt.SharedKernel.Domain.Interfaces;

namespace Acme.Application.ServiceImpls.Users.Queries;

internal class GetUserBasicQueryHandler(
        IIdentityRepository identityRepository,
        IMapper mapper,
        IAppLogger<GetUserBasicQueryHandler> logger,
        IUnitOfWork unitOfWork)
    : QueryHandlerBase<GetUserBasicQuery, UserForBasicDto>(unitOfWork, logger, mapper)
{
    public override async Task<Result<UserForBasicDto>> Handle(GetUserBasicQuery query,
        CancellationToken cancellationToken)
    {
        try
        {
            var user = await identityRepository.GetAsync(
                new UserFindSpec(IdentityGuid.Create(query.Id)),
                cancellationToken);

            if (user is null)
            {
                Logger.LogError("{Message} with Id {id}", UserErrorMessages.UserNotFound.Description, query.Id);
                return Result.Fail(UserErrorMessages.UserNotFound);
            }

            var userForDetailDto = Mapper.Map<UserForBasicDto>(user.User);

            return userForDetailDto;
        }
        catch (Exception e)
        {
            Logger.LogError(e.Message);
            return e;
        }
    }
}