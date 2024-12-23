using System.ComponentModel.DataAnnotations;
using Acme.Domain.Acme.Users.ValueObjects;
using Acme.Domain.Commons.User;
using Matt.SharedKernel.Results;
using Matt.SharedKernel;
using Matt.SharedKernel.Domain.Interfaces;
using Matt.SharedKernel.Domain.Primitives.Auditing;
using Role = Acme.Domain.Commons.User.Role;

namespace Acme.Domain.Acme.Users;

public class User : FullAuditedAggregateRoot<UserId>
{
    public const int MaxBirthYear = 1990;
    public const int MinAge = 16;

    public const int MinDescriptionLength = 64;
    public const int MaxDescriptionLength = 256;

    public const int MaxPhoneNumberLength = 13;

    public const int MaxNameLength = 15;
    public const int MinNameLength = 2;

    public string FirstName { get; private set; } = null!;
    public string LastName { get; private set; } = null!;
    public Gender Gender { get; private set; } = Gender.Male;
    public int BirthYear { get; private set; } = 1990;
    public Address Address { get; private set; } = null!;
    public string Description { get; private set; } = null!;
    public string Avatar { get; private set; } = null!;
    public string Email { get; private set; } = null!;
    public string PhoneNumber { get; private set; } = null!;
    public Role Role { get; private set; } = Role.BaseUser;

    public const string DefaultAvatar = "https://res.cloudinary.com/dhehywasc/image/upload/v1697006256/male/male0.png";

    private User()
    {
    }

    public static Result<User> Create(
        UserId identityUserId,
        string firstName,
        string lastName,
        Gender gender,
        int birthYear,
        Address address,
        string description,
        string? avatar,
        string email,
        string phoneNumber,
        Role role)
    {
        var result = DomainValidation.Sequentially(
            () => IsBirthYearValid(birthYear) ? Result.Success() : DomainErrors.Users.InvalidBirthYear,
            () => IsNameValid(firstName) ? Result.Success() : DomainErrors.Users.FirstNameIsRequired,
            () => IsNameValid(lastName) ? Result.Success() : DomainErrors.Users.LastNameIsRequired,
            () => IsEmailValid(email) ? Result.Success() : DomainErrors.Users.EmailIsRequired,
            () => IsPhoneNumberValid(phoneNumber) ? Result.Success() : DomainErrors.Users.InvalidPhoneNumber
        );

        if (result.IsFailed) return result.Error;

        return new User
        {
            Id = identityUserId,
            FirstName = firstName,
            LastName = lastName,
            Gender = gender,
            BirthYear = birthYear,
            Address = address,
            Description = description,
            Avatar = string.IsNullOrWhiteSpace(avatar) ? DefaultAvatar : avatar,
            Email = email,
            PhoneNumber = phoneNumber,
            Role = role
        };
    }

    public Result Update(
        string firstName,
        string lastName,
        Gender gender,
        int birthYear,
        Address address,
        string description,
        string? avatar)
    {
        var result = DomainValidation.Sequentially(
            () => IsBirthYearValid(birthYear) ? Result.Success() : DomainErrors.Users.InvalidBirthYear,
            () => IsNameValid(firstName) ? Result.Success() : DomainErrors.Users.FirstNameIsRequired,
            () => IsNameValid(lastName) ? Result.Success() : DomainErrors.Users.LastNameIsRequired
        );

        if (result.IsFailed) return result.Error;

        FirstName = firstName;
        LastName = lastName;
        Gender = gender;
        BirthYear = birthYear;
        Address = address;
        Description = description;
        Avatar = string.IsNullOrWhiteSpace(Avatar) || string.IsNullOrWhiteSpace(avatar) ? DefaultAvatar : avatar;

        return Result.Success();
    }

    public string GetFullName() => $"{FirstName} {LastName}";

    public void SetAvatar(string avatarUrl) => Avatar = avatarUrl;

    private static bool IsPhoneNumberValid(string phoneNumber) =>
        phoneNumber.Length is <= MaxPhoneNumberLength &&
        (phoneNumber[0] == '+' && phoneNumber[..1].All(char.IsDigit)
         || phoneNumber.All(char.IsDigit));

    private static bool IsEmailValid(string email) => new EmailAddressAttribute().IsValid(email);

    private static bool IsBirthYearValid(int birthYear) =>
        birthYear >= MaxBirthYear && birthYear <= DateTime.Now.Year - MinAge;

    private static bool IsNameValid(string name) => name.Length is <= MaxNameLength and >= MinNameLength;

    public void Deactivate()
    {
        IsDeleted = true;

        DomainEvents.Add(new UserDeactivatedDomainEvent(this));
    }
}

// ReSharper disable NotAccessedPositionalProperty.Global
public record UserDeactivatedDomainEvent(User User) : IDomainEvent; // TODO: remove the account
// ReSharper restore NotAccessedPositionalProperty.Global
