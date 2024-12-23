using Acme.Domain.Acme;
using Acme.Domain.Acme.Users;
using Acme.Domain.Acme.Users.ValueObjects;
using Matt.SharedKernel.Results;
using Matt.SharedKernel.Application.Mediators.Commands;
using Matt.SharedKernel.Domain.Interfaces;

namespace Acme.Application.ServiceImpls.Administrator.Users.Commands;

public record DeleteUserCommand(Guid Id) : ICommandRequest;

internal class DeleteUserCommandHandler(
    IUnitOfWork unitOfWork,
    IUserRepository userRepository
) : CommandHandlerBase<DeleteUserCommand>(unitOfWork)
{
    public override async Task<Result> Handle(DeleteUserCommand command, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(UserId.Create(command.Id), cancellationToken);

        if (user is null) return Result.Fail(DomainErrors.Users.NotFound);

        user.Deactivate();

        await UnitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}