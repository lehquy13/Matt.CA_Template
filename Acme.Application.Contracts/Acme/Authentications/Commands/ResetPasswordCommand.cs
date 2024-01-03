using Matt.SharedKernel.Application.Mediators.Commands;

namespace Acme.Application.Contracts.Acme.Authentications.Commands;

public record ResetPasswordCommand
(
    string Email,
    string Otp,
    string NewPassword
) : ICommandRequest;