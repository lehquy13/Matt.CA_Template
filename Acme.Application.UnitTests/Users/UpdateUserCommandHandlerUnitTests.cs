using Acme.Application.ServiceImpls.Administrator.Users.Commands;
using Acme.Domain.Acme.Users;
using Acme.Domain.Acme.Users.ValueObjects;
using Acme.Domain.Commons.User;
using FluentAssertions;
using Matt.SharedKernel.Domain.Interfaces;
using Moq;
using Xunit;

namespace Acme.Application.UnitTests.Users;

public class UpdateUserCommandHandlerUnitTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock = new();
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();

    private const string FirstName = "John";
    private const string LastName = "Doe";
    private const Gender UserGender = Gender.Female;
    private const Role UserRole = Role.BaseUser;
    private const string Mail = "johnd@mail.com";
    private const string PhoneNumber = "0123456789";
    private const int BirthYear = 1991;
    private const string City = "Hanoi";
    private const string District = "Ba Dinh";
    private const string Street = "123 Hoang Hoa Tham Street";

    private readonly User _validUser;

    private readonly UpdateUserCommandHandler _handler;

    public UpdateUserCommandHandlerUnitTests()
    {
        _handler = new UpdateUserCommandHandler(_userRepositoryMock.Object, _unitOfWorkMock.Object);

        _validUser = User.Create(
            UserId.Create(),
            FirstName,
            LastName,
            UserGender,
            BirthYear,
            Address.Create(City, District, Street).Value,
            string.Empty,
            null,
            Mail,
            PhoneNumber,
            UserRole).Value;
    }

    [Fact]
    public async Task UpdateUserHandler_WhenWithValidData_ShouldUpdateUser()
    {
        // Arrange
        var id = UserId.Create();
        const string updatedFirstName = "Jane";
        const string updatedLastName = "Smith";
        const string updatedCity = "New York";
        const string updatedDistrict = "Manhattan";
        const string updatedStreet = "456 Broadway Street";
        var updatedAddress = Address.Create(updatedCity, updatedDistrict, updatedStreet).Value;

        var command = new UpdateUserCommand(
            id.Value,
            updatedFirstName,
            updatedLastName,
            BirthYear,
            string.Empty,
            string.Empty,
            updatedCity,
            updatedDistrict,
            updatedStreet,
            UserGender
        );

        _userRepositoryMock.Setup(x => x.GetByIdAsync(
            It.Is<UserId>(userId => userId.Value == id.Value),
            CancellationToken.None)
        ).ReturnsAsync(_validUser);

        _unitOfWorkMock.Setup(x => x.SaveChangesAsync(CancellationToken.None)).ReturnsAsync(1);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();

        _userRepositoryMock.Verify(x => x.GetByIdAsync(It.IsAny<UserId>(), CancellationToken.None), Times.Once);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(CancellationToken.None), Times.Once);

        _validUser.FirstName.Should().Be(updatedFirstName);
        _validUser.LastName.Should().Be(updatedLastName);
        _validUser.Address.Should().Be(updatedAddress);
    }

    [Fact]
    public async Task UpdateUserHandler_WhenWithInvalidData_ShouldReturnError()
    {
        // Arrange
        var id = UserId.Create();
        var invalidFirstName = string.Empty; // Invalid because it's empty
        var invalidLastName = string.Empty; // Invalid because it's empty
        var invalidCity = string.Empty; // Invalid because it's empty
        var invalidDistrict = string.Empty; // Invalid because it's empty
        var invalidStreet = string.Empty; // Invalid because it's empty

        var command = new UpdateUserCommand(
            id.Value,
            invalidFirstName,
            invalidLastName,
            BirthYear,
            string.Empty,
            string.Empty,
            invalidCity,
            invalidDistrict,
            invalidStreet,
            UserGender
        );

        _userRepositoryMock.Setup(x => x.GetByIdAsync(
            It.Is<UserId>(userId => userId.Value == id.Value),
            CancellationToken.None)
        ).ReturnsAsync(_validUser);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeFalse();
    }
}