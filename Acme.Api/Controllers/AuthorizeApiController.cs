using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace Acme.Api.Controllers;

[Authorize]
public class AuthorizeApiController(IMapper mapper, ILogger<AuthorizeApiController> logger, IMediator mediator) 
    : ApiController(mapper, logger, mediator);