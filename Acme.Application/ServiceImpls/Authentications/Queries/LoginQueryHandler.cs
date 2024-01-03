using Acme.Application.Contracts.Acme.Authentications.Queries;
using Acme.Application.Contracts.DataTransferObjects.Authentications;
using Acme.Application.Contracts.Interfaces.Infrastructures;
using Acme.Domain.DomainServices.Interfaces;
using MapsterMapper;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Mediators.Queries;
using Matt.SharedKernel.Domain.Interfaces;

namespace Acme.Application.ServiceImpls.Authentications.Queries;

internal class LoginQueryHandler(
        IIdentityDomainServices authenticationService,
        IJwtTokenGenerator jwtTokenGenerator,
        IAppLogger<LoginQueryHandler> logger,
        IUnitOfWork unitOfWork,
        IMapper mapper
    )
    : QueryHandlerBase<LoginQuery, AuthenticationResult>(unitOfWork, logger, mapper)
{
    public override async Task<Result<AuthenticationResult>> Handle(LoginQuery query,
        CancellationToken cancellationToken)
    {
        var identityUser = await
            authenticationService.SignInAsync(query.Email, query.Password);

        if (identityUser is null)
        {
            return Result.Fail(AuthenticationErrorMessages.LoginFailError);
        }

        var userLoginDto = new IdentityUserDto
        {
            Id = identityUser.Id.Value,
            Email = identityUser.Email ?? string.Empty,
            Name = identityUser.UserName ?? string.Empty,
            Role = identityUser.IdentityRole.Name,
            TenantId = identityUser.Id.Value
        };

        var token = jwtTokenGenerator.GenerateToken(userLoginDto);

        var result = new AuthenticationResult(userLoginDto, token);

        return result;
    }
}