using Acme.Application.Contracts.Acme.Authentications.Commands;
using Acme.Application.Contracts.DataTransferObjects.Authentications;
using Acme.Domain.Acme.Users.ValueObjects;
using Acme.Domain.DomainServices.Interfaces;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Mediators.Commands;
using Matt.SharedKernel.Domain.Interfaces;

namespace Acme.Application.ServiceImpls.Authentications.Commands;

internal class ChangePasswordCommandHandler(
        IIdentityDomainServices identityDomainServices,
        IUnitOfWork unitOfWork,
        IAppLogger<ChangePasswordCommandHandler> logger)
    : CommandHandlerBase<ChangePasswordCommand>(unitOfWork, logger)
{
    public override async Task<Result> Handle(ChangePasswordCommand command, CancellationToken cancellationToken)
    {
        var identityId = IdentityGuid.Create(new Guid(command.Id));
        var result = await identityDomainServices
            .ChangePasswordAsync(
                identityId,
                command.CurrentPassword,
                command.NewPassword);

        if (!result.IsSuccess)
        {
            var resultToReturn = Result.Fail(AuthenticationErrorMessages.ChangePasswordFail)
                .WithError(result.Error);
            return resultToReturn;
        }

        if (await UnitOfWork.SaveChangesAsync(cancellationToken) <= 0)
        {
            Logger.LogWarning("Change password fail while saving changes");
            return Result.Fail(AuthenticationErrorMessages.ChangePasswordFailWhileSavingChanges);
        }

        return Result.Success();
    }
}