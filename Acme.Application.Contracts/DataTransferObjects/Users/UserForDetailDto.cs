using Matt.SharedKernel.Application.Contracts.Primitives;

namespace Acme.Application.Contracts.DataTransferObjects.Users;

/// <summary>
/// May update FullAuditedEntityDto later
/// </summary>
public class UserForDetailDto : EntityDto<Guid>
{
    public string Name { get; set; } = string.Empty;

    public string Address { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string PhoneNumber { get; set; } = string.Empty;

    public DateTime CreationTime { get; set; }

    public DateTime? LastModificationTime { get; set; }

    public override string ToString()
    {
        return $"User with Id: {Id}" +
               $"\nName: {Name}" +
               $"\nEmail: {Email}" +
               $"\nPhoneNumber: {PhoneNumber}" +
               $"\nAddress: {Address}";
    }
}