using Acme.Domain.Acme;
using Acme.Domain.Acme.Users;
using Acme.Domain.Acme.Users.ValueObjects;
using Acme.Persistence.EntityFrameworkCore;
using Matt.SharedKernel.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Acme.Persistence.Repository;

internal class UserRepository : RepositoryImpl<User, IdentityGuid>, IUserRepository
{
    public UserRepository(AppDbContext appDbContext, IAppLogger<UserRepository> logger) : base(appDbContext, logger)
    {
    }

    public async Task<User?> GetFullById(IdentityGuid id)
    {
        try
        {
            var fullUser = await AppDbContext
                .Users
                .FirstOrDefaultAsync(x => x.Id == id);
            return fullUser;
        }
        catch (Exception ex)
        {
            Logger.LogError(ErrorMessage, "GetFullById", ex.Message);
            return null;
        }
    }
}