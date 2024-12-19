using Acme.Application.ServiceImpls.Administrator.Users.Commands;
using Acme.Domain.Commons.User;
using FluentAssertions;
using Xunit;

namespace Acme.Application.UnitTests.Users;

public class UpsertUserCommandValidatorUnitTests
{
    private const string UserName = "JohnDoe";
    private const string FirstName = "John";
    private const string LastName = "Doe";
    private const string Password = "1q2w3E**";
    private const Gender UserGender = Gender.Female;
    private const string Mail = "johnd@mail.com";
    private const string PhoneNumber = "0123456789";
    private const int BirthYear = 1991;
    private const string City = "Hanoi";
    private const string District = "Ba Dinh";
    private const string Street = "123 Hoang Hoa Tham Street";

    private readonly UpsertUserCommandValidator _validator = new();


    [Fact]
    public void ValidateUpsertUserCommand_WhenValid_ShouldPass()
    {
        // Arrange
        var command = new UpsertUserCommand(
            UserName,
            Mail,
            PhoneNumber,
            Password,
            FirstName,
            LastName,
            BirthYear,
            string.Empty,
            string.Empty,
            City,
            District,
            Street,
            UserGender);

        // Act
        var validationResult = _validator.Validate(command);

        // Assert
        validationResult.IsValid.Should().BeTrue();
    }

    [Fact]
    public void ValidateUpsertUserCommand_WhenUserNameIsInvalid_ShouldFail()
    {
        // Arrange
        const string invalidUserName = "John Doe";

        var command = new UpsertUserCommand(
            invalidUserName,
            Mail,
            PhoneNumber,
            Password,
            FirstName,
            LastName,
            BirthYear,
            string.Empty,
            string.Empty,
            City,
            District,
            Street,
            UserGender);

        // Act
        var validationResult = _validator.Validate(command);

        // Assert
        validationResult.IsValid.Should().BeFalse();
        validationResult.Errors.Should().Contain(e => e.PropertyName == nameof(UpsertUserCommand.Username));
    }

    [Fact]
    public void ValidateUpsertUserCommand_WhenUsernameLengthLessThan6_ShouldFail()
    {
        // Arrange
        var command = new UpsertUserCommand(
            "user",
            Mail,
            PhoneNumber,
            Password,
            FirstName,
            LastName,
            BirthYear,
            string.Empty,
            string.Empty,
            City,
            District,
            Street,
            UserGender);

        // Act
        var validationResult = _validator.Validate(command);

        // Assert
        validationResult.IsValid.Should().BeFalse();
        validationResult.Errors.Should().Contain(e => e.PropertyName == nameof(UpsertUserCommand.Username));
    }

    [Fact]
    public void ValidateUpsertUserCommand_WhenUsernameLengthGreaterThan20_ShouldFail()
    {
        // Arrange
        var command = new UpsertUserCommand(
            new string('a', 21),
            Mail,
            PhoneNumber,
            Password,
            FirstName,
            LastName,
            BirthYear,
            string.Empty,
            string.Empty,
            City,
            District,
            Street,
            UserGender);

        // Act
        var validationResult = _validator.Validate(command);

        // Assert
        validationResult.IsValid.Should().BeFalse();
        validationResult.Errors.Should().Contain(e => e.PropertyName == nameof(UpsertUserCommand.Username));
    }

    [Fact]
    public void ValidateUpsertUserCommand_WhenUsernameDoesNotMatchRegex_ShouldFail()
    {
        // Arrange
        var command = new UpsertUserCommand(
            "invalid username",
            Mail,
            PhoneNumber,
            Password,
            FirstName,
            LastName,
            BirthYear,
            string.Empty,
            string.Empty,
            City,
            District,
            Street,
            UserGender);

        // Act
        var validationResult = _validator.Validate(command);

        // Assert
        validationResult.IsValid.Should().BeFalse();
        validationResult.Errors.Should().Contain(e => e.PropertyName == nameof(UpsertUserCommand.Username));
    }

    [Fact]
    public void ValidateUpsertUserCommand_WhenEmailIsInvalid_ShouldFail()
    {
        // Arrange
        var command = new UpsertUserCommand(
            UserName,
            "invalid-email",
            PhoneNumber,
            Password,
            FirstName,
            LastName,
            BirthYear,
            string.Empty,
            string.Empty,
            City,
            District,
            Street,
            UserGender);

        // Act
        var validationResult = _validator.Validate(command);

        // Assert
        validationResult.IsValid.Should().BeFalse();
        validationResult.Errors.Should().Contain(e => e.PropertyName == nameof(UpsertUserCommand.Email));
    }

    [Fact]
    public void ValidateUpsertUserCommand_WhenPhoneNumberDoesNotMatchRegex_ShouldFail()
    {
        // Arrange
        var command = new UpsertUserCommand(
            UserName,
            Mail,
            "invalid-phone",
            Password,
            FirstName,
            LastName,
            BirthYear,
            string.Empty,
            string.Empty,
            City,
            District,
            Street,
            UserGender);

        // Act
        var validationResult = _validator.Validate(command);

        // Assert
        validationResult.IsValid.Should().BeFalse();
        validationResult.Errors.Should().Contain(e => e.PropertyName == nameof(UpsertUserCommand.PhoneNumber));
    }

    [Fact]
    public void ValidateUpsertUserCommand_WhenPasswordLengthLessThan8_ShouldFail()
    {
        // Arrange
        var command = new UpsertUserCommand(
            UserName,
            Mail,
            PhoneNumber,
            "Pass1!",
            FirstName,
            LastName,
            BirthYear,
            string.Empty,
            string.Empty,
            City,
            District,
            Street,
            UserGender);

        // Act
        var validationResult = _validator.Validate(command);

        // Assert
        validationResult.IsValid.Should().BeFalse();
        validationResult.Errors.Should().Contain(e => e.PropertyName == nameof(UpsertUserCommand.Password));
    }

    [Fact]
    public void ValidateUpsertUserCommand_WhenPasswordDoesNotMatchRegex_ShouldFail()
    {
        // Arrange
        var command = new UpsertUserCommand(
            UserName,
            Mail,
            PhoneNumber,
            "password",
            FirstName,
            LastName,
            BirthYear,
            string.Empty,
            string.Empty,
            City,
            District,
            Street,
            UserGender);

        // Act
        var validationResult = _validator.Validate(command);

        // Assert
        validationResult.IsValid.Should().BeFalse();
        validationResult.Errors.Should().Contain(e => e.PropertyName == nameof(UpsertUserCommand.Password));
    }

    [Fact]
    public void ValidateUpsertUserCommand_WhenFirstNameIsEmpty_ShouldFail()
    {
        // Arrange
        var command = new UpsertUserCommand(
            UserName,
            Mail,
            PhoneNumber,
            Password,
            "",
            LastName,
            BirthYear,
            string.Empty,
            string.Empty,
            City,
            District,
            Street,
            UserGender);

        // Act
        var validationResult = _validator.Validate(command);

        // Assert
        validationResult.IsValid.Should().BeFalse();
        validationResult.Errors.Should().Contain(e => e.PropertyName == nameof(UpsertUserCommand.FirstName));
    }

    [Fact]
    public void ValidateUpsertUserCommand_WhenFirstNameLengthLessThanMin_ShouldFail()
    {
        // Arrange
        var command = new UpsertUserCommand(
            UserName,
            Mail,
            PhoneNumber,
            Password,
            "J",
            LastName,
            BirthYear,
            string.Empty,
            string.Empty,
            City,
            District,
            Street,
            UserGender);

        // Act
        var validationResult = _validator.Validate(command);

        // Assert
        validationResult.IsValid.Should().BeFalse();
        validationResult.Errors.Should().Contain(e => e.PropertyName == nameof(UpsertUserCommand.FirstName));
    }

    [Fact]
    public void ValidateUpsertUserCommand_WhenFirstNameLengthGreaterThanMax_ShouldFail()
    {
        // Arrange
        var command = new UpsertUserCommand(
            UserName,
            Mail,
            PhoneNumber,
            Password,
            new string('a', 21),
            LastName,
            BirthYear,
            string.Empty,
            string.Empty,
            City,
            District,
            Street,
            UserGender);

        // Act
        var validationResult = _validator.Validate(command);

        // Assert
        validationResult.IsValid.Should().BeFalse();
        validationResult.Errors.Should().Contain(e => e.PropertyName == nameof(UpsertUserCommand.FirstName));
    }

    [Fact]
    public void ValidateUpsertUserCommand_WhenLastNameIsEmpty_ShouldFail()
    {
        // Arrange
        var command = new UpsertUserCommand(
            UserName,
            Mail,
            PhoneNumber,
            Password,
            FirstName,
            "",
            BirthYear,
            string.Empty,
            string.Empty,
            City,
            District,
            Street,
            UserGender);

        // Act
        var validationResult = _validator.Validate(command);

        // Assert
        validationResult.IsValid.Should().BeFalse();
        validationResult.Errors.Should().Contain(e => e.PropertyName == nameof(UpsertUserCommand.LastName));
    }

    [Fact]
    public void ValidateUpsertUserCommand_WhenLastNameLengthLessThanMin_ShouldFail()
    {
        // Arrange
        var command = new UpsertUserCommand(
            UserName,
            Mail,
            PhoneNumber,
            Password,
            FirstName,
            "D",
            BirthYear,
            string.Empty,
            string.Empty,
            City,
            District,
            Street,
            UserGender);

        // Act
        var validationResult = _validator.Validate(command);

        // Assert
        validationResult.IsValid.Should().BeFalse();
        validationResult.Errors.Should().Contain(e => e.PropertyName == nameof(UpsertUserCommand.LastName));
    }

    [Fact]
    public void ValidateUpsertUserCommand_WhenLastNameLengthGreaterThan20_ShouldFail()
    {
        // Arrange
        var command = new UpsertUserCommand(
            UserName,
            Mail,
            PhoneNumber,
            Password,
            FirstName,
            new string('a', 21),
            BirthYear,
            string.Empty,
            string.Empty,
            City,
            District,
            Street,
            UserGender);

        // Act
        var validationResult = _validator.Validate(command);

        // Assert
        validationResult.IsValid.Should().BeFalse();
        validationResult.Errors.Should().Contain(e => e.PropertyName == nameof(UpsertUserCommand.LastName));
    }

    [Fact]
    public void ValidateUpsertUserCommand_WhenBirthYearNotInRange_ShouldFail()
    {
        // Arrange
        var command = new UpsertUserCommand(
            UserName,
            Mail,
            PhoneNumber,
            Password,
            FirstName,
            LastName,
            1800,
            string.Empty,
            string.Empty,
            City,
            District,
            Street,
            UserGender);

        // Act
        var validationResult = _validator.Validate(command);

        // Assert
        validationResult.IsValid.Should().BeFalse();
        validationResult.Errors.Should().Contain(e => e.PropertyName == nameof(UpsertUserCommand.BirthYear));
    }
}