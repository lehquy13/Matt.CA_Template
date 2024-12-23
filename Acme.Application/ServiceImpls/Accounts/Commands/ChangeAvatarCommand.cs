using Acme.Domain.Acme.Users;
using Acme.Domain.Acme.Users.ValueObjects;
using FluentValidation;
using Matt.SharedKernel.Results;
using Matt.SharedKernel.Application.Contracts.Interfaces;
using Matt.SharedKernel.Application.Contracts.Interfaces.Infrastructures;
using Matt.SharedKernel.Application.Mediators.Commands;
using Matt.SharedKernel.Domain.Interfaces;

namespace Acme.Application.ServiceImpls.Accounts.Commands;

public record ChangeAvatarCommand(string Url) : ICommandRequest, IAuthorizationRequired;

public class ChangeAvatarCommandValidator : AbstractValidator<ChangeAvatarCommand>
{
    public ChangeAvatarCommandValidator()
    {
        RuleFor(x => x.Url)
            .NotEmpty()
            .WithMessage("Please enter a valid image URL.");
    }
}

public class ChangeAvatarCommandHandler(
    IUserRepository userRepository,
    ICurrentUserService currentUserService,
    IUnitOfWork unitOfWork
) : CommandHandlerBase<ChangeAvatarCommand>(unitOfWork)
{
    public override async Task<Result> Handle(ChangeAvatarCommand request, CancellationToken cancellationToken)
    {
        var id = UserId.Create(currentUserService.UserId);
        var account = await userRepository.GetByIdAsync(id, cancellationToken);

        if (account == null)
        {
            return Result.Fail(AccountServiceErrorConstants.NonExistUserError);
        }

        account.SetAvatar(request.Url);

        return await UnitOfWork.SaveChangesAsync(cancellationToken) <= 0
            ? Result.Fail(AccountServiceErrorConstants.FailToUpdateUserErrorWhileSavingChanges)
            : Result.Success();
    }
}