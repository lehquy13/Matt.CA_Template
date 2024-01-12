using Acme.Application.Contracts.DataTransferObjects.Authentications;
using Acme.Application.Contracts.DataTransferObjects.Users;
using Acme.Domain.Acme.Users;
using Acme.Domain.Acme.Users.Identities;
using Mapster;

namespace Acme.Application.Mapping;

internal class UserMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<IdentityUser, IdentityUserDto>()
            .Map(des => des.Name, src => src.UserName)
            .Map(des => des.Email, src => src.Email)
            .Map(des => des.Role, src => src.IdentityRole.Name)
            .Map(des => des.Id, src => src.Id.Value)
            .Map(des => des, src => src);

        config.NewConfig<User, UserForListDto>()
            .Map(des => des.Name, src => src.FirstName + " " + src.LastName)
            .Map(des => des.Id, src => src.Id.Value)
            .Map(des => des, src => src);

        config.NewConfig<User, UserForBasicDto>()
            .Map(des => des.Name, src => src.FirstName + " " + src.LastName)
            .Map(des => des.Address, src => src.Address.ToString())
            .Map(des => des.Email, src => src.IdentityUser.Email)
            .Map(des => des.Id, src => src.Id.Value)
            .Map(des => des, src => src);
        
        config.NewConfig<User, UserForDetailDto>()
            .Map(des => des.Name, src => src.FirstName + " " + src.LastName)
            .Map(des => des.Address, src => src.Address)
            .Map(des => des.Email, src => src.IdentityUser.Email)
            .Map(des => des.PhoneNumber, src => src.IdentityUser.PhoneNumber)
            .Map(des => des.CreationTime, src => src.CreationTime)
            .Map(des => des.LastModificationTime, src => src.LastModificationTime)
            .Map(des => des.Id, src => src.Id.Value)
            .Map(des => des, src => src);
    }
}