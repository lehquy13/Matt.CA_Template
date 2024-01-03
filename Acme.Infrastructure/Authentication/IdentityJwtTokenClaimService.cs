using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Acme.Application.Contracts.DataTransferObjects.Authentications;
using Acme.Application.Contracts.Interfaces.Infrastructures;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Acme.Infrastructure.Authentication;

internal class IdentityJwtTokenClaimService(IOptions<JwtSettings> jwtSettings) : IJwtTokenGenerator
{
    private readonly JwtSettings _jwtSettings = jwtSettings.Value;

    public string GenerateToken(IdentityUserDto identityUserDto)
    {
        var signingCredential = new SigningCredentials(
            new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_jwtSettings.Secret)
            ),
            SecurityAlgorithms.HmacSha256Signature
        );

        var claims = new Claim[]
        {
            new(ClaimTypes.Sid, Guid.NewGuid().ToString()),
            new(ClaimTypes.NameIdentifier, identityUserDto.Id.ToString()),
            new(ClaimTypes.Name, identityUserDto.Name),
            new(ClaimTypes.Email, identityUserDto.Email),
            new(ClaimTypes.Role, identityUserDto.Role),
            new(ClaimTypes.Actor, identityUserDto.TenantId.ToString())
        };

        var securityToken = new JwtSecurityToken(
            _jwtSettings.Issuer,
            _jwtSettings.Audience,
            expires: DateTime.Now.AddMinutes(_jwtSettings.ExpiryMinutes),
            claims: claims,
            signingCredentials: signingCredential
        );

        return new JwtSecurityTokenHandler().WriteToken(securityToken);
    }

    public bool ValidateToken(string token)
    {
        var accessToken = token;

        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));

        var tokenHandler = new JwtSecurityTokenHandler();
        try
        {
            tokenHandler.ValidateToken(accessToken, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = symmetricSecurityKey,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidIssuer = _jwtSettings.Issuer,
                ValidAudience = _jwtSettings.Audience,
                ValidateLifetime = true
                // ClockSkew = TimeSpan.Zero // zero tolerance for the token lifetime expiration time
            }, out var validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;

            if (jwtToken.ValidTo < DateTime.UtcNow)
                // token is expired, redirect to authentication page
                return false;

            // token is still valid, navigate to home page
            return true;
        }
        catch
        {
            return false;
        }
    }
}