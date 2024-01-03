using Matt.SharedKernel.Application.Mediators.Commands;

namespace Acme.Application.Contracts.Acme.Authentications.Commands;

public record ChangePasswordCommand
(
    string Id,
    string CurrentPassword,
    string NewPassword
) : ICommandRequest;