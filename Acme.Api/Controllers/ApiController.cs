using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Acme.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ApiController(IMapper mapper, ILogger<ApiController> logger, IMediator mediator) : ControllerBase
{
    protected readonly IMediator Mediator = mediator;
    protected readonly IMapper Mapper = mapper;
    protected readonly ILogger<ApiController> Logger = logger;
}
