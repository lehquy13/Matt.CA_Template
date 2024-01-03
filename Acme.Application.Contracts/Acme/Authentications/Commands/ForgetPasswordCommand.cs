using Matt.SharedKernel.Application.Mediators.Commands;

namespace Acme.Application.Contracts.Acme.Authentications.Commands;

public record ForgetPasswordCommand
(
    string Email
) : ICommandRequest;