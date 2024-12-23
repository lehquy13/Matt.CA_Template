using Acme.Application.ServiceImpls.Administrator.Users.Commands;
using Acme.Application.ServiceImpls.Administrator.Users.Queries;
using MapsterMapper;
using Matt.SharedKernel.Paginations;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Acme.Api.Controllers;

public class UserController(
    IMapper mapper,
    ILogger<UserController> logger,
    IMediator mediator
) : AuthorizeApiController(mapper, logger, mediator)
{
    [HttpGet]
    [Route("")]
    public async Task<IActionResult> GetAllUsers([FromQuery] PaginatedParams userParams)
    {
        var users = await Mediator.Send(new GetUsersQuery(userParams));

        return Ok(users);
    }

    [HttpGet]
    [Route("{id:guid}")]
    public async Task<IActionResult> GetUser([FromRoute] Guid id)
    {
        var user = await Mediator.Send(new GetUserByIdQuery(id));

        return Ok(user);
    }

    [HttpPut]
    [Route("detail/{id:guid}/edit")]
    public async Task<IActionResult> EditUser(Guid id, [FromBody] CreateUserCommand editUserCommand)
    {
        var result = await Mediator.Send(editUserCommand);

        return Ok(result);
    }

    [HttpDelete]
    [Route("detail/{id:guid}/delete")]
    public async Task<IActionResult> DeleteUser([FromRoute] Guid id)
    {
        var result = await Mediator.Send(new DeleteUserCommand(id));

        return Ok(result);
    }
}