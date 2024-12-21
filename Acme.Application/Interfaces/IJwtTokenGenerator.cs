using System.Security.Claims;
using Matt.SharedKernel.Application.Authorizations;

namespace Acme.Application.Interfaces;

public interface IJwtTokenGenerator
{
    string GenerateToken(IdentityDto identityDto);
    IEnumerable<Claim> ValidateToken(string token);
}