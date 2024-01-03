using Matt.SharedKernel.Application.Contracts.Primitives;

namespace Acme.Application.Contracts.DataTransferObjects.Users;

public class UserForListDto : EntityDto<Guid>
{
    public string Name { get; set; } = string.Empty;

    public override string ToString()
    {
        return $"User with Id: {Id}, Name: {Name}";
    }
}