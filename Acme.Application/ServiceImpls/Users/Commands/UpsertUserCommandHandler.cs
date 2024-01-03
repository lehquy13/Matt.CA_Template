using Acme.Application.Contracts.Acme.Users.Commands;
using Acme.Application.Contracts.DataTransferObjects.Users;
using Acme.Domain.Acme;
using Acme.Domain.Acme.Users;
using Acme.Domain.Acme.Users.ValueObjects;
using MapsterMapper;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Mediators.Commands;
using Matt.SharedKernel.Domain.Interfaces;

namespace Acme.Application.ServiceImpls.Users.Commands;

internal class UpsertUserCommandHandler(
        IUnitOfWork unitOfWork,
        IAppLogger<UpsertUserCommandHandler> logger,
        IMapper mapper,
        IUserRepository userRepository)
    : CommandHandlerBase<UpsertUserCommand>(unitOfWork, logger)
{
    public override async Task<Result> Handle(UpsertUserCommand command, CancellationToken cancellationToken)
    {
        var user = await userRepository.FindAsync(IdentityGuid.Create(command.UserForUpsertDto.Id));

        if (user is null)
        {
            //Create new user
            user = mapper.Map<User>(command.UserForUpsertDto);

            await userRepository.InsertAsync(user);
        }
        else
        {
            //Update user
            mapper.Map(command.UserForUpsertDto, user);
        }

        if (await UnitOfWork.SaveChangesAsync(cancellationToken) <= 0)
        {
            Logger.LogError(UserErrorMessages.UpsertFail.Description);
            return UserErrorMessages.UpsertFail;
        }

        return Result.Success();
    }
}