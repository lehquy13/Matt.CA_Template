using Matt.SharedKernel.Application.Contracts.Primitives;

namespace Acme.Application.Contracts.DataTransferObjects.Authentications;

public class IdentityUserDto : EntityDto<Guid>
{
    //User information
    public string Name { get; set; } = string.Empty;

    //Account References
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public Guid TenantId { get; set; } = Guid.Empty;
}