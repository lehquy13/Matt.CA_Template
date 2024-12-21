using Acme.Domain.Acme.Users;
using Acme.Domain.Acme.Users.ValueObjects;
using Acme.Domain.Commons.User;
using FluentValidation;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Mediators.Commands;
using Matt.SharedKernel.Domain.Interfaces;

namespace Acme.Application.ServiceImpls.Administrator.Users.Commands;

public record UpdateUserCommand(
    Guid Id,
    string FirstName,
    string LastName,
    int BirthYear,
    string Avatar,
    string Description,
    string City,
    string District,
    string DetailAddress,
    Gender Gender
) : ICommandRequest;

public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .WithMessage("First name is required.")
            .MinimumLength(User.MinNameLength)
            .MaximumLength(User.MaxNameLength)
            .WithMessage("First name must not exceed 50 characters.");

        RuleFor(x => x.LastName)
            .NotEmpty()
            .WithMessage("Last name is required.")
            .MinimumLength(User.MinNameLength)
            .MaximumLength(User.MaxNameLength)
            .WithMessage("Last name must not exceed 50 characters.");

        RuleFor(x => x.BirthYear)
            .InclusiveBetween(1900, DateTime.Now.Year - 16)
            .WithMessage("Invalid birth year.");
    }
}

public class UpdateUserCommandHandler(
    IUserRepository userRepository,
    IUnitOfWork unitOfWork
) : CommandHandlerBase<UpdateUserCommand>(unitOfWork)
{
    public override async Task<Result> Handle(UpdateUserCommand command, CancellationToken cancellationToken)
    {
        var address = Address.Create(command.City, command.District, command.DetailAddress);

        if (address.IsFailed) return Result.Fail(address.Error);

        var user = await userRepository.GetByIdAsync(UserId.Create(command.Id), cancellationToken);

        if (user is null) return Result.Fail("User not found.");

        var actionResult = user.Update(
            command.FirstName,
            command.LastName,
            command.Gender,
            command.BirthYear,
            address.Value,
            command.Description,
            command.Avatar
        );

        if (actionResult.IsFailed) return Result.Fail(actionResult.Error);

        await UnitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}