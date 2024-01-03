using System.Security.Claims;
using Acme.Application.Contracts.Interfaces.Infrastructures;
using Matt.SharedKernel.Application.Contracts.Interfaces.Infrastructures;
using Microsoft.AspNetCore.Http;

namespace Acme.Infrastructure.Authentication;

public class CurrentTenantService(
        IHttpContextAccessor httpContextAccessor
    )
    : ICurrentTenantService
{
    public string GetTenantId()
    {
        var userId = httpContextAccessor
            .HttpContext?.User
            .FindFirst(ClaimTypes.Actor)?.Value ?? "";
        
        return userId;
    }
}