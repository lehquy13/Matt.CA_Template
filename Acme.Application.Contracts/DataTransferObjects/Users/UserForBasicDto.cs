using Matt.SharedKernel.Application.Contracts.Primitives;

namespace Acme.Application.Contracts.DataTransferObjects.Users;

public class UserForBasicDto : EntityDto<Guid>
{
    public string Name { get; set; } = string.Empty;

    public string Address { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public override string ToString()
    {
        return $"User with Id: {Id}" +
               $"\nName: {Name}" +
               $"\nEmail: {Email}" +
               $"\nAddress: {Address}";
    }
}