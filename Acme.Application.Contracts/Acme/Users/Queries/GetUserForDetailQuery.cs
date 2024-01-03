using Acme.Application.Contracts.DataTransferObjects.Users;
using Matt.SharedKernel.Application.Mediators.Queries;

namespace Acme.Application.Contracts.Acme.Users.Queries;

public record GetUserForDetailQuery(Guid Id) : IQueryRequest<UserForDetailDto>;