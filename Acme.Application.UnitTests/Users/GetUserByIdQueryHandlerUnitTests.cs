using Acme.Application.ServiceImpls.Administrator.Users.Queries;
using Acme.Domain.Acme.Users;
using Acme.TestSetup;
using FluentAssertions;
using Moq;
using Xunit;

namespace Acme.Application.UnitTests.Users;

public class GetUserByIdQueryHandlerUnitTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock = new();
    private readonly GetUserByIdQueryHandler _sut;

    public GetUserByIdQueryHandlerUnitTests()
    {
        _sut = new GetUserByIdQueryHandler(_userRepositoryMock.Object);
    }

    [Fact]
    public async Task GetUserByIdHandler_WhenUserExists_ShouldReturnUser()
    {
        // Arrange
        _userRepositoryMock
            .Setup(x => x.GetByIdAsync(TestData.UserData.UserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(TestData.UserData.ValidUser);

        var query = new GetUserByIdQuery(TestData.UserData.UserId.Value);

        // Act
        var result = await _sut.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();

        var user = result.Value;

        user.Id.Should().Be(TestData.UserData.UserId.Value);
        user.FullName.Should().Be($"{TestData.UserData.FirstName} {TestData.UserData.LastName}");
        user.Email.Should().Be(TestData.UserData.Mail);
        user.Gender.Should().Be(TestData.UserData.UserGender.ToString());
        user.BirthYear.Should().Be(TestData.UserData.BirthYear);
    }

    [Fact]
    public async Task GetUserByIdHandler_WhenUserNotExists_ShouldReturnError()
    {
        // Arrange
        _userRepositoryMock
            .Setup(x => x.GetByIdAsync(TestData.UserData.UserId, CancellationToken.None))
            .ReturnsAsync((User)null!);

        var query = new GetUserByIdQuery(TestData.UserData.UserId.Value);

        // Act
        var result = await _sut.Handle(query, CancellationToken.None);

        // Assert
        result.IsFailed.Should().BeTrue();
        result.Error.Code.Should().NotBeEmpty();
    }
}