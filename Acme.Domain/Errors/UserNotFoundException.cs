﻿using Matt.ResultObject;

namespace Acme.Domain.Errors;

public class UserNotFoundException : Exception
{
    public UserNotFoundException(string userName) : base($"No user found with username: {userName}")
    {
    }
}

//Test new User Error using ACP.Results
//Compare this snippet from ACP.Domain/Errors/UserNotFoundException.cs:
public static class UserNotFoundError
{
    public static Error UserNotFound()
    {
        return new Error("UserNotFound",
            "No user found");
    }

    public static Error UserNotFound(string userName)
    {
        return new Error("UserNotFound",
            $"No user found with username: {userName}");
    }
}