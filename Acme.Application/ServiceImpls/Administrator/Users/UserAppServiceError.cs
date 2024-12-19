using Matt.ResultObject;

namespace Acme.Application.ServiceImpls.Administrator.Users;

public static class UserAppServiceError
{
    public static Error UserNotFound => new("UserNotFound", "User not found.");
}