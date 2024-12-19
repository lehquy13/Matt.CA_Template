using Acme.Application.Interfaces;
using Acme.Domain.Acme.Users;
using Acme.Infrastructure.Persistence.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Acme.Infrastructure.Persistence.Repositories;

public class ReadDbContext(
    AppDbContext appDbContext
) : IReadDbContext
{
    public DbSet<User> Users => appDbContext.Set<User>().AsReadOnly();
}

public static class ReadDbContextExtensions
{
    public static DbSet<T> AsReadOnly<T>(this DbSet<T> dbSet) where T : class => (DbSet<T>)dbSet.AsNoTracking();
}