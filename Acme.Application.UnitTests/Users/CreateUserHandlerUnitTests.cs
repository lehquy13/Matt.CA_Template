﻿using Acme.Application.ServiceImpls.Administrator.Users.Commands;
using Acme.Domain.Acme.Users;
using Acme.Domain.Acme.Users.ValueObjects;
using Acme.Domain.Commons.User;
using FluentAssertions;
using Matt.ResultObject;
using Matt.SharedKernel.Domain.Interfaces;
using Moq;
using Xunit;

namespace Acme.Application.UnitTests.Users;

public class CreateUserHandlerUnitTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock = new();
    private readonly Mock<IIdentityService> _identityServiceMock = new();
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();

    private const string UserName = "JohnDoe";
    private const string FirstName = "John";
    private const string LastName = "Doe";
    private const string Password = "1q2w3E**";
    private const Gender UserGender = Gender.Female;
    private const Role UserRole = Role.BaseUser;
    private const string Mail = "johnd@mail.com";
    private const string PhoneNumber = "0123456789";
    private const int BirthYear = 1991;
    private const string City = "Hanoi";
    private const string District = "Ba Dinh";
    private const string Street = "123 Hoang Hoa Tham Street";

    private readonly User _validUser;

    private readonly CreateUserCommandHandler _handler;

    public CreateUserHandlerUnitTests()
    {
        _handler = new CreateUserCommandHandler(_identityServiceMock.Object, _userRepositoryMock.Object,
            _unitOfWorkMock.Object);

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
    public async Task CreateUserHandler_WhenWithValidData_ShouldReturnUser()
    {
        // Arrange
        var command = new CreateUserCommand(UserName, Mail, PhoneNumber, Password, FirstName, LastName, BirthYear,
            string.Empty, string.Empty, City, District, Street, UserGender);

        _identityServiceMock.Setup(x => x.CreateAsync(
                command.Username,
                command.FirstName,
                command.LastName,
                command.Gender,
                command.BirthYear,
                It.IsAny<Address>(),
                string.Empty,
                string.Empty,
                command.Email,
                command.PhoneNumber,
                UserRole))
            .ReturnsAsync(_validUser);

        _unitOfWorkMock.Setup(x => x.SaveChangesAsync(CancellationToken.None)).ReturnsAsync(1);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();

        _userRepositoryMock.Verify(x => x.Insert(_validUser, CancellationToken.None), Times.Once);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task CreateUserHandler_WhenWithInvalidAddress_ShouldReturnError()
    {
        // Arrange
        var emptyCity = string.Empty;
        var command = new CreateUserCommand(UserName, Mail, PhoneNumber, Password, FirstName, LastName, BirthYear,
            string.Empty, string.Empty, emptyCity, District, Street, UserGender);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public async Task CreateUserHandler_WhenIdentityServiceFail_ShouldReturnError()
    {
        // Arrange
        const string resultMessage = "Error";
        var command = new CreateUserCommand(UserName, Mail, PhoneNumber, Password, FirstName, LastName, BirthYear,
            string.Empty, string.Empty, City, District, Street, UserGender);

        _identityServiceMock.Setup(x => x.CreateAsync(
                command.Username,
                command.FirstName,
                command.LastName,
                command.Gender,
                command.BirthYear,
                It.IsAny<Address>(),
                string.Empty,
                string.Empty,
                command.Email,
                command.PhoneNumber,
                UserRole))
            .ReturnsAsync(Result.Fail(resultMessage));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeFalse();
        result.Error.Description.Should().Be(resultMessage);
    }
}