﻿using Acme.Application.Contracts.Acme.Authentications.Commands;
using Acme.Application.Contracts.DataTransferObjects.Authentications;
using Acme.Application.Contracts.Interfaces.Infrastructures;
using Acme.Domain.Acme.Users.Identities;
using Acme.Domain.Specifications.Users;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Mediators.Commands;
using Matt.SharedKernel.Domain.Interfaces;

namespace Acme.Application.ServiceImpls.Authentications.Commands;

internal class ForgetPasswordCommandHandler(
        IIdentityRepository identityRepository,
        IEmailSender emailSender,
        IUnitOfWork unitOfWork,
        IAppLogger<ForgetPasswordCommandHandler> logger)
    : CommandHandlerBase<ForgetPasswordCommand>(unitOfWork, logger)
{
    public override async Task<Result> Handle(ForgetPasswordCommand command, CancellationToken cancellationToken)
    {
        var identityUser = await identityRepository.GetAsync(
            new UserGetByEmailSpec(command.Email), cancellationToken);

        if (identityUser is null) return Result.Fail(AuthenticationErrorMessages.UserNotFound);

        identityUser.GenerateOtpCode();

        if (await UnitOfWork.SaveChangesAsync(cancellationToken) <= 0)
            return Result.Fail(AuthenticationErrorMessages.ResetPasswordFail);

        var message = $"Your OTP code is {identityUser.OtpCode.Value}";

#pragma warning disable
        emailSender.SendEmail(command.Email, "Reset password", message);
#pragma warning restore

        return Result.Success();
    }
}