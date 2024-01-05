using Acme.Api.Models;
using Acme.Application.Contracts.Acme.Users.Commands;
using Acme.Application.Contracts.Acme.Users.Queries;
using Acme.Application.Contracts.DataTransferObjects.Users;
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
    public async Task<IActionResult> EditUser(Guid id, [FromBody] UserModel userModel)
    {
        var result = await Mediator.Send(
            new UpsertUserCommand(
                new UserForUpsertDto(
                    id,
                    userModel.Name,
                    userModel.City,
                    userModel.Country
                )
            ));

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