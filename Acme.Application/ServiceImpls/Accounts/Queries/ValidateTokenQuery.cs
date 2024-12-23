using Acme.Application.Interfaces;
using Matt.SharedKernel.Results;
using Matt.SharedKernel.Application.Mediators.Queries;

namespace Acme.Application.ServiceImpls.Accounts.Queries;

public record ValidateTokenQuery(string ValidateToken) : IQueryRequest;

public class ValidateTokenQueryHandler(IJwtTokenGenerator jwtTokenGenerator) : QueryHandlerBase<ValidateTokenQuery>
{
    public override Task<Result> Handle(ValidateTokenQuery request, CancellationToken cancellationToken) =>
        Task.FromResult(jwtTokenGenerator.ValidateToken(request.ValidateToken).Any()
            ? Result.Success()
            : Result.Fail("Token is invalid."));
}