using Matt.SharedKernel.Application.Mediators.Commands;

namespace Acme.Application.Contracts.Acme.Authentications.Commands;

public record RegisterCommand
(
    string Username,
    string Email,
    string Password,
    string PhoneNumber
) : ICommandRequest;