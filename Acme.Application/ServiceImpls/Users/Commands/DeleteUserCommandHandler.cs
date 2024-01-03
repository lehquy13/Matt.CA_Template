using Acme.Application.Contracts.Acme.Users.Commands;
using Acme.Application.Contracts.DataTransferObjects.Users;
using Acme.Domain.Acme;
using Acme.Domain.Acme.Users;
using Acme.Domain.Acme.Users.ValueObjects;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Mediators.Commands;
using Matt.SharedKernel.Domain.Interfaces;

namespace Acme.Application.ServiceImpls.Users.Commands;

internal class DeleteUserCommandHandler(
        IUnitOfWork unitOfWork,
        IAppLogger<DeleteUserCommandHandler> logger,
        IUserRepository userRepository)
    : CommandHandlerBase<DeleteUserCommand>(unitOfWork, logger)
{
    public override async Task<Result> Handle(DeleteUserCommand command, CancellationToken cancellationToken)
    {
        await userRepository.RemoveAsync(IdentityGuid.Create(command.Id));

        if (await UnitOfWork.SaveChangesAsync(cancellationToken) <= 0)
        {
            Logger.LogError("{Message}", UserErrorMessages.DeleteFailWithException);
            return Result.Fail(UserErrorMessages.DeleteFailWithException);
        }

        return Result.Success();
    }
}