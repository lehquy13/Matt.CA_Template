using Mapster;

namespace Acme.Application.Mapping;

internal class UserMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Acme.Domain.Acme.Users.User, Contracts.Users.UserDetailDto>();
    }
}