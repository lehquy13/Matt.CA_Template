namespace Matt.SharedKernel.Application.Contracts.Interfaces.Infrastructures;

public interface ICurrentUserService
{
    string? CurrentUserId { get; }
    bool IsAuthenticated { get; }
    string? CurrentUserEmail { get; }
    string? CurrentUserFullName { get; }
}