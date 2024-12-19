using Acme.Domain.Commons.User;

namespace Acme.Domain.Commons.Utilities;

public static class EnumProvider
{
    public static List<string> Genders { get; } = Enum.GetNames(typeof(Gender)).ToList();
    public static List<string> Roles { get; } = Enum.GetNames(typeof(Role)).ToList();
}