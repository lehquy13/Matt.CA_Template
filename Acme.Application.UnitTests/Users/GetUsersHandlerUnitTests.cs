using Acme.Application.Interfaces;
using Acme.Application.ServiceImpls.Administrator.Users.Queries;
using Acme.Domain.Acme.Users;
using Acme.TestSetup;
using FluentAssertions;
using Matt.Paginated;
using Moq;
using Moq.EntityFrameworkCore;
using Xunit;

namespace Acme.Application.UnitTests.Users;

public class GetUsersHandlerUnitTests
{
    private readonly Mock<IReadDbContext> _readDbContextMock = new();
    private readonly GetUsersQueryHandler _sut;

    private const int PageIndex = 1;

    public GetUsersHandlerUnitTests()
    {
        _sut = new GetUsersQueryHandler(_readDbContextMock.Object);
    }

    [Fact]
    public async Task GetUsersHandler_WhenUsersExists_ShouldReturnUsers()
    {
        // Arrange
        List<User> users = [TestData.UserData.ValidUser];

        _readDbContextMock
            .Setup(x => x.Users)
            .ReturnsDbSet(users);

        var query = new GetUsersQuery(new PaginatedParams
        {
            PageIndex = PageIndex
        });

        // Act
        var result = await _sut.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Items.Should().NotBeEmpty();
        result.Value.Items.Should().HaveCount(1);
        result.Value.Items.Should().Contain(x => x.Id == TestData.UserData.ValidUser.Id.Value);
    }

    [Fact]
    public async Task GetUsersHandler_WhenUsersNotExists_ShouldReturnEmptyList()
    {
        // Arrange
        _readDbContextMock
            .Setup(x => x.Users)
            .ReturnsDbSet([]);

        var query = new GetUsersQuery(new PaginatedParams
        {
            PageIndex = PageIndex
        });

        // Act
        var result = await _sut.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Items.Should().BeEmpty();
    }
}