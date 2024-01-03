using Acme.Application.Contracts.DataTransferObjects.Authentications;

namespace Acme.Application.Contracts.Interfaces.Infrastructures;

public interface IJwtTokenGenerator
{
    string GenerateToken(IdentityUserDto identityUserDto);
    bool ValidateToken(string token); //TODO: change the parameter to be a dto and this method will use for refresh token
}