// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global\

using Acme.Application.Contracts.Users;
using Acme.Domain.Acme.Users;
using Acme.Domain.Acme.Users.ValueObjects;
using FluentValidation;
using MapsterMapper;
using Matt.SharedKernel.Results;
using Matt.SharedKernel.Application.Contracts.Interfaces;
using Matt.SharedKernel.Application.Contracts.Interfaces.Infrastructures;
using Matt.SharedKernel.Application.Mediators.Commands;
using Matt.SharedKernel.Domain.Interfaces;

namespace Acme.Application.ServiceImpls.Accounts.Commands;

public record UpdateBasicProfileCommand(UserProfileUpdateDto UserProfileUpdateDto)
    : ICommandRequest, IAuthorizationRequired;

public class UpdateBasicProfileCommandValidator : AbstractValidator<UpdateBasicProfileCommand>
{
    public UpdateBasicProfileCommandValidator()
    {
        RuleFor(x => x.UserProfileUpdateDto).NotNull();
        RuleFor(x => x.UserProfileUpdateDto).SetValidator(new UserProfileCreateUpdateDtoValidator());
    }
}

public class UpdateUserProfileCommandHandler(
    IUserRepository userRepository,
    ICurrentUserService currentUserService,
    IUnitOfWork unitOfWork,
    IMapper mapper
) : CommandHandlerBase<UpdateBasicProfileCommand>(unitOfWork)
{
    public override async Task<Result> Handle(UpdateBasicProfileCommand command,
        CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(
            UserId.Create(currentUserService.UserId),
            cancellationToken);

        if (user is not null)
        {
            mapper.Map(command.UserProfileUpdateDto, user);

            return await UnitOfWork.SaveChangesAsync(cancellationToken) <= 0
                ? Result.Fail(AccountServiceErrorConstants.FailToUpdateUserErrorWhileSavingChanges)
                : Result.Success();
        }

        //Create new user
        user = mapper.Map<User>(command.UserProfileUpdateDto);

        userRepository.Insert(user, cancellationToken);

        return await UnitOfWork.SaveChangesAsync(cancellationToken) <= 0
            ? Result.Fail(AccountServiceErrorConstants.FailToCreateUserErrorWhileSavingChanges)
            : Result.Success();
    }
}