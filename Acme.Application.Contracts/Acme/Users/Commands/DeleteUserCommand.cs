using Matt.SharedKernel.Application.Mediators.Commands;

namespace Acme.Application.Contracts.Acme.Users.Commands;

public record DeleteUserCommand(Guid Id) : ICommandRequest;