using Acme.Application.Contracts.DataTransferObjects.Authentications;
using Matt.SharedKernel.Application.Mediators.Queries;

namespace Acme.Application.Contracts.Acme.Authentications.Queries;

public record LoginQuery(string Email, string Password) : IQueryRequest<AuthenticationResult>;