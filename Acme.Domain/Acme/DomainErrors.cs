using Matt.SharedKernel.Results;

namespace Acme.Domain.Acme;

public static class DomainErrors
{
    public static class Users
    {
        public static readonly Error InvalidPhoneNumber = new("InvalidPhoneNumber", "Invalid phone number format");
        public static readonly Error InvalidBirthYear = new("InvalidBirthYear", "Birth year is invalid");
        public static readonly Error FirstNameIsRequired = new("FirstNameIsRequired", "First name is required");
        public static readonly Error LastNameIsRequired = new("LastNameIsRequired", "Last name is required");
        public static readonly Error EmailIsRequired = new("EmailIsRequired", "Email is required");
        public static readonly Error UserAlreadyExist = new("UserAlreadyExist", "User already exists");
        public static readonly Error AddRoleFail = new("AddRoleFail", "Add role fail");
        public static readonly Error NotFound = new("UserNotFound", "User not found");
        public static readonly Error ChangePasswordFailed = new("ChangePasswordFailed", "Change password failed");
        public static readonly Error RemoveFailed = new("RemoveFailed", "Remove failed");
        public static readonly Error EmailNotConfirmed = new("EmailNotConfirmed", "Email not confirmed");
    }
}