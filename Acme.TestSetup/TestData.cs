using Acme.Application;
using Acme.Domain;
using Acme.Domain.Acme.Users;
using Acme.Domain.Acme.Users.ValueObjects;
using Acme.Domain.Commons.User;
using Mapster;
using MapsterMapper;

namespace Acme.TestSetup;

public static class TestData
{
    public static class UserData
    {
        public const string FirstName = "John";
        public const string LastName = "Doe";
        public const Gender UserGender = Gender.Female;
        private const Role UserRole = Role.BaseUser;
        public const string Mail = "johnd@mail.com";
        private const string PhoneNumber = "0123456789";
        public const int BirthYear = 1991;
        private const string City = "Hanoi";
        private const string District = "Ba Dinh";
        private const string Street = "123 Hoang Hoa Tham Street";
        public static readonly UserId UserId = UserId.Create();

        public static readonly User ValidUser =
            User.Create(
                UserId,
                FirstName,
                LastName,
                UserGender,
                BirthYear,
                Address.Create(City, District, Street).Value,
                string.Empty,
                null,
                Mail,
                PhoneNumber,
                UserRole).Value;
    }
}

public static class MapsterProfile
{
    public static IMapper Get => ScanMapsterProfile();

    private static Mapper ScanMapsterProfile()
    {
        var config = TypeAdapterConfig.GlobalSettings;

        config.Scan(typeof(DomainDependencyInjection).Assembly);
        config.Scan(typeof(DependencyInjection).Assembly);

        return new Mapper(config);
    }
}