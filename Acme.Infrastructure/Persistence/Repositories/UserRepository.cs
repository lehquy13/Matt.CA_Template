using Acme.Domain.Acme.Users;
using Acme.Domain.Acme.Users.ValueObjects;
using Acme.Infrastructure.Persistence.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Acme.Infrastructure.Persistence.Repositories;

public class UserRepository(AppDbContext appDbContext) : IUserRepository
{
    public async Task<User?> GetByIdAsync(UserId userId, CancellationToken cancellationToken)
        => await appDbContext.Set<User>()
            .FirstOrDefaultAsync(x => x.Id == userId, cancellationToken);

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken) =>
        await appDbContext.Set<User>()
            .FirstOrDefaultAsync(x => x.Email == email, cancellationToken);

    public void Insert(User user, CancellationToken cancellationToken) => appDbContext.Set<User>().Add(user);

    public async Task<List<User>> GetPaginatedListAsync(int pageIndex, int pageSize,
        CancellationToken cancellationToken) =>
        await appDbContext
            .Set<User>()
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
}