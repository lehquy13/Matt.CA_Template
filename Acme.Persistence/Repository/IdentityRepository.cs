using Acme.Domain.Acme.Users.Identities;
using Acme.Domain.Acme.Users.ValueObjects;
using Acme.Persistence.EntityFrameworkCore;
using Matt.SharedKernel.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Acme.Persistence.Repository;

internal class IdentityRepository :
    RepositoryImpl<IdentityUser, IdentityGuid>,
    IIdentityRepository
{
    public IdentityRepository(AppDbContext appDbContext, IAppLogger<IdentityRepository> logger) : base(appDbContext,
        logger)
    {
    }


    public async Task<IdentityUser?> GetByIdAsync(IdentityGuid id)
    {
        try
        {
            return await AppDbContext.IdentityUsers
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.Id.Equals(id));
        }
        catch (Exception ex)
        {
            Logger.LogError(ErrorMessage, "GetByIdAsync", ex.Message);
            return null;
        }
    }

    public async Task<IdentityUser?> FindByNameAsync(string normalizedUserName,
        CancellationToken cancellationToken = default)
    {
        try
        {
            return await AppDbContext.IdentityUsers
                .Include(x => x.User)
                .Include(x => x.IdentityRole)
                .FirstOrDefaultAsync(x => x.UserName != null && x.UserName.ToUpper() == normalizedUserName,
                    cancellationToken);
        }
        catch (Exception ex)
        {
            Logger.LogError(ErrorMessage, "FindByNameAsync", ex.Message);
            return null;
        }
    }

    public async Task<IdentityUser?> FindByEmailAsync(string normalizedEmail,
        CancellationToken cancellationToken = default)
    {
        try
        {
            return await AppDbContext.IdentityUsers
                .Include(x => x.User)
                .Include(x => x.IdentityRole)
                .FirstOrDefaultAsync(x => x.Email != null && x.Email.ToUpper() == normalizedEmail, cancellationToken);
        }
        catch (Exception ex)
        {
            Logger.LogError(ErrorMessage, "FindByEmailAsync", ex.Message);
            return null;
        }
    }

    public async Task<IdentityRole?> GetRolesAsync(IdentityGuid userId, CancellationToken cancellationToken = default)
    {
        try
        {
            var identityUser = await AppDbContext.IdentityUsers
                .Include(x => x.IdentityRole)
                .FirstOrDefaultAsync(x => x.Id.Equals(userId), cancellationToken);

            if (identityUser is null)
            {
                throw new Exception("User not found.");
            }

            return identityUser.IdentityRole;
        }
        catch (Exception ex)
        {
            Logger.LogError(ErrorMessage, "GetRolesAsync", ex.Message);
            return null;
        }
    }

    public Task<IdentityUser?> CheckExistingAccount(string email, string userName)
    {
        try
        {
            return AppDbContext.IdentityUsers
                .Include(x => x.User)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Email == email || x.UserName == userName);
        }
        catch (Exception ex)
        {
            Logger.LogError(ErrorMessage, "CheckExistingAccount", ex.Message);
            throw;
        }
    }
}