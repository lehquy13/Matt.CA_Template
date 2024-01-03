using Acme.Domain.Shared.User;

namespace Acme.Domain.Shared.Utilities;

public static class EnumProvider
{
    public static List<string> Genders { get; } = Enum.GetNames(typeof(Gender)).ToList();
}