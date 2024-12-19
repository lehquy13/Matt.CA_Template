namespace Acme.Application.Contracts.Users;

public record UserDetailDto(Guid Id, string FullName, string Email, string Gender, int BirthYear);