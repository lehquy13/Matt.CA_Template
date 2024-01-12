using Acme.Domain.Acme.Users.Identities;
using Acme.Domain.Acme.Users.ValueObjects;
using Acme.Domain.DomainServices.Errors;
using Acme.Domain.DomainServices.Interfaces;
using Matt.ResultObject;
using Matt.SharedKernel.Domain;
using Matt.SharedKernel.Domain.Interfaces;
using Matt.SharedKernel.Domain.Interfaces.Repositories;

namespace Acme.Domain.DomainServices;

public class IdentityDomainServices(
        IAppLogger<IdentityDomainServices> logger,
        IIdentityRepository identityUserRepository,
        IReadOnlyRepository<IdentityRole, Guid> identityRoleRepository)
    : DomainServiceBase(logger), IIdentityDomainServices
{
    public async Task<IdentityUser?> SignInAsync(string email, string password)
    {
        await Task.CompletedTask;

        var identityUser = await identityUserRepository
            .FindByEmailAsync(email);

        if (identityUser is null || identityUser.ValidatePassword(password) is false) return null;

        return identityUser;
    }

    public async Task<Result<IdentityUser>> CreateAsync(
        string userName,
        string email,
        string password,
        string phoneNumber)
    {
        var roles = await identityRoleRepository
            .GetListAsync();

        if (roles.Count <= 0)
        {
            return Result.Fail(DomainServiceErrors.RoleNotFoundDomainError);
        }
        
        var identityUser = await identityUserRepository
            .CheckExistingAccount(email, userName);

        if (identityUser is not null)
        {
            return Result.Fail(DomainServiceErrors.UserAlreadyExistDomainError);
        }
        
        identityUser = IdentityUser.Create(
            userName,
            email,
            password,
            phoneNumber,
            roles.First().Id
        );
        
        await identityUserRepository.InsertAsync(identityUser);

        return identityUser;
    }

    public async Task<Result> ChangePasswordAsync(IdentityGuid identityId, string currentPassword, string newPassword)
    {
        var identityUser = await identityUserRepository.FindAsync(identityId);

        if (identityUser is null)
        {
            return Result.Fail(DomainServiceErrors.UserNotFound);
        }

        var verifyResult = identityUser.ValidatePassword(currentPassword);

        if (!verifyResult)
        {
            return Result.Fail(DomainServiceErrors.InvalidPassword);
        }

        identityUser.HandlePassword(newPassword);

        return Result.Success();
    }

    public async Task<Result> ResetPasswordAsync(string email, string newPassword, string otpCode)
    {
        var identityUser = await identityUserRepository.FindByEmailAsync(email);

        if (identityUser is null)
        {
            return DomainServiceErrors.UserNotFound;
        }

        //Check does otp have same value and still valid
        if (identityUser.OtpCode.Value != otpCode)
        {
            return DomainServiceErrors.InvalidOtp;
        }

        if (identityUser.OtpCode.IsExpired())
        {
            return DomainServiceErrors.OtpExpired;
        }

        identityUser.HandlePassword(newPassword);
        identityUser.OtpCode.Reset();

        return Result.Success();
    }
}