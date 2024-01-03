using Acme.Application.Contracts.Acme.Authentications.Commands;
using Acme.Application.Contracts.DataTransferObjects.Authentications;
using Acme.Application.Contracts.Interfaces.Infrastructures;
using Acme.Domain.DomainServices.Interfaces;
using MapsterMapper;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Mediators.Commands;
using Matt.SharedKernel.Domain.Interfaces;

namespace Acme.Application.ServiceImpls.Authentications.Commands;

internal class RegisterCommandHandler(
        IIdentityDomainServices identityDomainServices,
        IUnitOfWork unitOfWork,
        IAppLogger<ChangePasswordCommandHandler> logger)
    : CommandHandlerBase<RegisterCommand>(unitOfWork, logger)
{
    public override async Task<Result> Handle(RegisterCommand command,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await identityDomainServices.CreateAsync(
                command.Username,
                command.Email,
                command.Password,
                command.PhoneNumber
            );

            if (!result.IsSuccess || result.Value is null)
            {
                var resultToReturn = Result.Fail(AuthenticationErrorMessages.RegisterFail)
                    .WithError(result.Error);
                return resultToReturn;
            }

            if (await UnitOfWork.SaveChangesAsync(cancellationToken) < 0)
            {
                return Result.Fail(AuthenticationErrorMessages.CreateUserFailWhileSavingChanges);
            }

            return Result.Success();
        }
        catch (Exception e)
        {
            Logger.LogError(e.Message);

            return Result.Fail(AuthenticationErrorMessages.RegisterFail)
                .WithError(e);
        }
    }
}