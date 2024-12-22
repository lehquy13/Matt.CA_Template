using Acme.Application.Contracts.Authentications;
using Acme.Application.Interfaces;
using Acme.Domain.Acme.Users;
using Acme.Domain.Acme.Users.ValueObjects;
using FluentValidation;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Mediators.Queries;

namespace Acme.Application.ServiceImpls.Accounts.Queries;

public record LoginQuery(
    string Email,
    string Password
) : IQueryRequest<AuthenticationResult>;

public class LoginQueryValidator : AbstractValidator<LoginQuery>
{
    public LoginQueryValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .WithMessage("Please enter a valid email address.");

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Password must be at least 8 characters long.");
    }
}

public class LoginQueryHandler(
    IIdentityService identityService,
    IUserRepository userRepository,
    IJwtTokenGenerator jwtTokenGenerator
) : QueryHandlerBase<LoginQuery, AuthenticationResult>
{
    public override async Task<Result<AuthenticationResult>> Handle(
        LoginQuery getAllUserQuery,
        CancellationToken cancellationToken)
    {
        var identityDto = await identityService.SignInAsync(
            getAllUserQuery.Email, getAllUserQuery.Password);

        if (identityDto is null)
        {
            return Result.Fail(AuthenticationErrorConstants.LoginFailError);
        }

        var customer = await userRepository.GetByIdAsync(
            UserId.Create(identityDto.Id),
            cancellationToken);

        if (customer is null)
        {
            return Result.Fail(AuthenticationErrorConstants.UserNotFound);
        }

        var loginToken = jwtTokenGenerator.GenerateToken(identityDto);

        var userLoginDto = new UserLoginDto
        {
            Id = customer.Id.Value,
            Email = customer.Email,
            FullName = $"{customer.FirstName} {customer.LastName}",
            Roles = identityDto.Roles,
            Avatar = customer.Avatar
        };

        return new AuthenticationResult(userLoginDto, loginToken);
    }
}