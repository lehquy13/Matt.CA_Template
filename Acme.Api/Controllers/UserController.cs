using Acme.Application.Contracts.Acme.Users.Commands;
using Acme.Application.Contracts.Acme.Users.Queries;
using MapsterMapper;
using Matt.Paginated;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Acme.Api.Controllers;

public class UserController(
    IMapper mapper,
    ILogger<UserController> logger,
    IMediator mediator)
    : AuthorizeApiController(mapper, logger, mediator)
{
    [HttpGet]
    [Route("")]
    public async Task<IActionResult> GetAllUsers([FromQuery] PaginatedParams userParams)
    {
        var users = await Mediator.Send(new GetUsersQuery(userParams));

        return Ok(users);
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> GetUser([FromRoute] Guid id)
    {
        var user = await Mediator.Send(new GetUserBasicQuery(id));

        return Ok(user);
    }

    [HttpGet]
    [Route("detail/{id}")]
    public async Task<IActionResult> GetDetail([FromRoute] Guid id)
    {
        var user = await Mediator.Send(new GetUserBasicQuery(id));

        return Ok(user);
    }

    [HttpPut]
    [Route("detail/{id}/edit")]
    public async Task<IActionResult> EditUser([FromBody] UpsertUserCommand editUserCommand)
    {
        var result = await Mediator.Send(editUserCommand);

        return Ok(result);
    }

    [HttpDelete]
    [Route("detail/{id}/delete")]
    public async Task<IActionResult> DeleteUser([FromRoute] Guid id)
    {
        var result = await Mediator.Send(new DeleteUserCommand(id));

        return Ok(result);
    }
}