namespace Acme.Application.Contracts.DataTransferObjects.Authentications;

/// <summary>
/// A result for authentication after login or register
/// </summary>
/// <param name="IdentityUserDto"></param>
/// <param name="Token"></param>
public record AuthenticationResult(IdentityUserDto IdentityUserDto, string Token);