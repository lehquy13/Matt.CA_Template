using Acme.Domain.Acme.Users.ValueObjects;
using Matt.SharedKernel.Domain.Interfaces.Repositories;

namespace Acme.Domain.Acme.Users;

public interface IUserRepository : IRepository
{
    Task<User?> GetByIdAsync(UserId userId, CancellationToken cancellationToken);
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken);
    void Insert(User user, CancellationToken cancellationToken);
    Task<List<User>> GetPaginatedListAsync(int pageIndex, int pageSize, CancellationToken cancellationToken);
}