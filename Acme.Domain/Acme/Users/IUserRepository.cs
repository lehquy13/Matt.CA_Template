using Acme.Domain.Acme.Users.ValueObjects;
using Matt.SharedKernel.Domain.Interfaces.Repositories;

namespace Acme.Domain.Acme.Users;

public interface IUserRepository : IRepository<User, IdentityGuid>
{
}