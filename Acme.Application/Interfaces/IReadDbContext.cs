using Acme.Domain.Acme.Users;
using Microsoft.EntityFrameworkCore;

namespace Acme.Application.Interfaces;

public interface IReadDbContext
{
    DbSet<User> Users { get; }
}