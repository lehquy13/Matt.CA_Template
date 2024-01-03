using Acme.Application.Contracts.DataTransferObjects.Users;
using Matt.SharedKernel.Application.Mediators.Commands;

namespace Acme.Application.Contracts.Acme.Users.Commands;

public record UpsertUserCommand(UserForUpsertDto UserForUpsertDto) : ICommandRequest;