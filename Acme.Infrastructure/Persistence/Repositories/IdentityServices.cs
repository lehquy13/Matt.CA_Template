﻿using System.Text;
using Acme.Domain.Acme;
using Acme.Domain.Acme.Users;
using Acme.Domain.Acme.Users.ValueObjects;
using Acme.Domain.Commons.User;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Authorizations;
using Matt.SharedKernel.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using IEmailSender = Acme.Application.Interfaces.IEmailSender;
using Role = Acme.Domain.Commons.User.Role;


namespace Acme.Infrastructure.Persistence.Repositories;

public class IdentityService(
    SignInManager<IdentityUser> signInManager,
    UserManager<IdentityUser> userManager,
    IEmailSender emailSender,
    ILogger logger
) : DomainServiceBase, IIdentityService
{
    public async Task<IdentityDto?> SignInAsync(
        string email,
        string password)
    {
        var identityUser = await userManager.FindByEmailAsync(email);

        if (identityUser is null)
        {
            return null;
        }

        var userRoles = await userManager.GetRolesAsync(identityUser);

        var result = await signInManager.CheckPasswordSignInAsync(identityUser, password, false);

        if (result.Succeeded
            && identityUser.UserName is not null
            && identityUser.Email is not null)
        {
            return new IdentityDto(
                new Guid(identityUser.Id),
                identityUser.UserName,
                identityUser.Email,
                userRoles);
        }

        return null;
    }

    public async Task<Result<User>> CreateAsync(
        string userName,
        string firstName,
        string lastName,
        Gender gender,
        int birthYear,
        Address address,
        string description,
        string avatar,
        string email,
        string phoneNumber,
        Role role = Role.BaseUser
    )
    {
        var esIdentityUser = await userManager
            .FindByNameAsync(userName);

        if (esIdentityUser is not null)
        {
            return Result.Fail(DomainErrors.Users.UserAlreadyExist);
        }

        esIdentityUser = InitializeUserInstance();

        var result = await CreateUser(userName, email, phoneNumber, esIdentityUser);

        if (!result.Succeeded)
        {
            return Result.Fail(result.Errors.Select(x => x.Description).Aggregate((x, y) => x + " " + y));
        }

        logger.LogInformation("User created a new account with password");

        var userId = new Guid(await userManager.GetUserIdAsync(esIdentityUser));
        var code = await userManager.GenerateEmailConfirmationTokenAsync(esIdentityUser);

        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

        // var callbackUrl = Url.Page(
        //     "/Account/ConfirmEmail",
        //     pageHandler: null,
        //     values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
        //     protocol: Request.Scheme);

#pragma warning disable

        emailSender.Send(email, "Demo email",
            $"This email will use to confirm your email using the code {code}");

#pragma warning restore
        //await emailSender.SendHtmlEmail(email, "Confirm your email",
        //  $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

        var roleAddResult = await userManager.AddToRoleAsync(esIdentityUser, role.ToString().ToUpper());

        if (roleAddResult.Succeeded)
            return User.Create(
                UserId.Create(userId),
                firstName,
                lastName,
                gender,
                birthYear,
                address,
                description,
                avatar,
                email,
                phoneNumber,
                role
            );

        logger.LogError("Fail to add role to user with id {UserId} {Message}", userId,
            roleAddResult.Errors.Select(x => x.Description).Aggregate((x, y) => x + " " + y));

        return Result.Fail(DomainErrors.Users.AddRoleFail);
    }

    private async Task<IdentityResult> CreateUser(
        string userName,
        string email,
        string phoneNumber,
        IdentityUser identityUser)
    {
        identityUser.UserName = userName;
        identityUser.NormalizedUserName = userName.ToUpper();
        identityUser.Email = email;
        identityUser.NormalizedEmail = email.ToUpper();
        identityUser.PhoneNumber = phoneNumber;

        //await emailStore.SetEmailAsync(esIdentityUser, email, cancellationToken);
        //await phoneNumberStore.SetPhoneNumberAsync(esIdentityUser, phoneNumber, cancellationToken);

        var result = await userManager.CreateAsync(identityUser, DefaultPassword);
        return result;
    }

    private const string DefaultPassword = "1q2w3E**";

    public async Task<Result> ChangePasswordAsync(UserId userId, string currentPassword, string newPassword)
    {
        var identityUser = await userManager.FindByIdAsync(userId.Value.ToString());

        if (identityUser is null) return Result.Fail(DomainErrors.Users.NotFound);

        var verifyResult = await userManager.ChangePasswordAsync(identityUser, currentPassword, newPassword);

        if (verifyResult.Succeeded) return Result.Success();

        logger.LogError("Fail to change password for user with id {UserId} {Message}", userId,
            verifyResult.Errors.Select(x => x.Description).Aggregate((x, y) => x + " " + y));

        return Result.Fail(DomainErrors.Users.ChangePasswordFailed);
    }

    public async Task<Result> ResetPasswordAsync(
        string email,
        string newPassword,
        string otpCode)
    {
        var identityUser = await userManager.FindByEmailAsync(email);

        if (identityUser is null) return Result.Fail(DomainErrors.Users.NotFound);

        var result = await userManager.ResetPasswordAsync(identityUser, otpCode, newPassword);

        if (result.Succeeded) return Result.Success();

        var error = result.Errors.First().Description;

        return Result.Fail(error);
    }

    public async Task<Result> RemoveAsync(UserId userId)
    {
        var user = await userManager.FindByIdAsync(userId.ToString());

        if (user == null) return Result.Fail(DomainErrors.Users.NotFound);

        var result = await userManager.DeleteAsync(user);

        return result.Succeeded
            ? Result.Success()
            : Result.Fail(DomainErrors.Users.RemoveFailed);
    }

    public async Task<Result> ForgetPasswordAsync(string email)
    {
        var user = await userManager.FindByEmailAsync(email);

        if (user == null) return Result.Fail(DomainErrors.Users.NotFound);

        if (!await userManager.IsEmailConfirmedAsync(user)) return Result.Fail(DomainErrors.Users.EmailNotConfirmed);

        var code = await userManager.GeneratePasswordResetTokenAsync(user);
        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

        // var callbackUrl = Url.Page(
        //     "/Account/ResetPassword",
        //     pageHandler: null,
        //     values: new { area = "Identity", code },
        //     protocol: Request.Scheme);

        // await emailSender.SendEmail(
        //     email,
        //     "Reset Password",
        //     $"Please reset your password by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

        await emailSender.Send(
            email,
            "Reset Password but just a demo",
            $"Please reset your password by this mail by using this {code}.");

        return Result.Success();
    }

    private static IdentityUser InitializeUserInstance()
    {
        try
        {
            return Activator.CreateInstance<IdentityUser>();
        }
        catch
        {
            throw new InvalidOperationException($"Can't create an instance of '{nameof(IdentityUser)}'. " +
                                                $"Ensure that '{nameof(IdentityUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                                                $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
        }
    }
}