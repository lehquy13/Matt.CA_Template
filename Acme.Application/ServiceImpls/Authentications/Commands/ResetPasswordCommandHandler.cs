using Acme.Application.Contracts.Acme.Authentications.Commands;
using Acme.Application.Contracts.DataTransferObjects.Authentications;
using Acme.Application.Contracts.Interfaces.Infrastructures;
using Acme.Domain.DomainServices.Interfaces;
using MapsterMapper;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Mediators.Commands;
using Matt.SharedKernel.Domain.Interfaces;

namespace Acme.Application.ServiceImpls.Authentications.Commands;

internal class ResetPasswordCommandHandler(
        IIdentityDomainServices identityDomainServices,
        IUnitOfWork unitOfWork,
        IAppLogger<ResetPasswordCommandHandler> logger)
    : CommandHandlerBase<ResetPasswordCommand>(unitOfWork, logger)

{
    public override async Task<Result> Handle(ResetPasswordCommand command, CancellationToken cancellationToken)
    {
        var result = await identityDomainServices
            .ResetPasswordAsync(
                command.Email,
                command.NewPassword,
                command.Otp);

        if (!result.IsSuccess)
        {
            return Result.Fail(AuthenticationErrorMessages.ResetPasswordFail).WithError(result.Error);
        }

        if (await UnitOfWork.SaveChangesAsync(cancellationToken) <= 0)
        {
            return AuthenticationErrorMessages.ResetPasswordFailWhileSavingChanges;
        }

        return Result.Success();
    }
}