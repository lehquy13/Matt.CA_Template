using Matt.ResultObject;

namespace Acme.Application.Contracts.DataTransferObjects.Users;

public static class UserErrorMessages
{
    public static readonly Error UserNotFound = new("UserNotFound", "User not found");
    public static readonly Error UpsertFail = new("UpsertFail", "Upsert fail with exception");
    public static readonly Error DeleteFailWithException = new("DeleteFailWithException", "Delete fail with exception");
}