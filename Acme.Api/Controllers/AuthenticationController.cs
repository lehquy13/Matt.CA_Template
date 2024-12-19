using Acme.Application.ServiceImpls.Accounts.Commands;
using Acme.Application.ServiceImpls.Accounts.Queries;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Acme.Api.Controllers;

public class AuthenticationController(
    IMapper mapper,
    ILogger<AuthenticationController> logger,
    IMediator mediator
) : ApiController(mapper, logger, mediator)
{
    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login(LoginQuery loginRequest)
    {
        var result = await Mediator.Send(loginRequest);

        return Ok(result);
    }

    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register(RegisterCommand registerRequest)
    {
        var result = await Mediator.Send(registerRequest);

        return Ok(result);
    }

    [HttpPost]
    [Route("forgot-password")]
    public async Task<IActionResult> ForgotPassword(string email)
    {
        var result = await Mediator.Send(new ForgetPasswordCommand(email));

        return Ok(result);
    }

    [HttpPost]
    [Route("reset-password")]
    public async Task<IActionResult> ResetPassword(ResetPasswordCommand resetPasswordRequest)
    {
        var result = await Mediator.Send(resetPasswordRequest);

        return Ok(result);
    }

    [Authorize]
    [HttpPost]
    [Route("change-password")]
    public async Task<IActionResult> ChangePassword(ChangePasswordCommand changePasswordCommand)
    {
        var result = await Mediator.Send(changePasswordCommand);

        return Ok(result);
    }
}