// ReSharper disable NotAccessedPositionalProperty.Global

namespace Acme.Application.Contracts.Authentications;

public record AuthenticationResult(UserLoginDto User, string Token);