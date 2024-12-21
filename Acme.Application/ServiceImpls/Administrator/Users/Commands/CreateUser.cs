using Acme.Domain.Acme;
using Acme.Domain.Acme.Users;
using Acme.Domain.Acme.Users.ValueObjects;
using Acme.Domain.Commons.User;
using FluentValidation;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Mediators.Commands;
using Matt.SharedKernel.Domain.Interfaces;

namespace Acme.Application.ServiceImpls.Administrator.Users.Commands;

public record CreateUserCommand(
    string Username,
    string Email,
    string PhoneNumber,
    string Password,
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

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty()
            .MinimumLength(6)
            .MaximumLength(20)
            .Matches("^[a-zA-Z0-9_]+$") // Only letters, numbers, and underscores
            .WithMessage(
                "Username must be between 3 and 50 characters long, and can only contain letters, numbers, and underscores.");

        RuleFor(x => x.Email)
            .EmailAddress()
            .WithMessage("Please enter a valid email address.");

        RuleFor(x => x.PhoneNumber)
            .Matches(@"^\d+$") // Matches a sequence of digits
            .WithMessage("Phone number must only contain digits.");

        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(8)
            .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$")
            .WithMessage(
                "Password must be at least 8 characters long and contain at least one lowercase letter, one uppercase letter, one digit, and one special character.");
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

public class CreateUserCommandHandler(
    IIdentityService identityService,
    IUserRepository userRepository,
    IUnitOfWork unitOfWork
) : CommandHandlerBase<CreateUserCommand>(unitOfWork)
{
    public override async Task<Result> Handle(CreateUserCommand command, CancellationToken cancellationToken)
    {
        var address = Address.Create(command.City, command.District, command.DetailAddress);

        if (address.IsFailed) return Result.Fail(address.Error);

        var user = await userRepository.GetByEmailAsync(command.Email, cancellationToken);

        if (user is not null)
        {
            return Result.Fail(DomainErrors.Users.UserAlreadyExist);
        }

        var result = await identityService.CreateAsync(
            command.Username,
            command.FirstName,
            command.LastName,
            command.Gender,
            command.BirthYear,
            address.Value,
            command.Description,
            command.Avatar,
            command.Email,
            command.PhoneNumber);

        if (!result.IsSuccess) return Result.Fail(result.Error);

        userRepository.Insert(result.Value, cancellationToken);

        await UnitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}