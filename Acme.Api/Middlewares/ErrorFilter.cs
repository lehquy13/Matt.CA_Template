using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Acme.Api.Middlewares;

public class ErrorHandlingFilterAttribute : ExceptionFilterAttribute
{
    public override void OnException(ExceptionContext context)
    {
        ProblemDetails problemDetails = new()
        {
            Title = "An error occurred while processing your request.",
            Status = (int)HttpStatusCode.InternalServerError,
        };

        context.Result = new ObjectResult(problemDetails);

        context.ExceptionHandled = true;
    }
}