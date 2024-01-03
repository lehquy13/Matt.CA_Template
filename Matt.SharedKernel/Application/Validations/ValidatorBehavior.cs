using FluentValidation;
using MediatR;

namespace Matt.SharedKernel.Application.Validations;

public class ValidationBehavior<TRequest, TResponse>(IValidator<TRequest>? validator = null) :
    IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>

{
    public async Task<TResponse> Handle(TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (validator == null) return await next();
        // before the handler
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        // after the handler
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.ToList();
            throw new Exception(errors.FirstOrDefault()?.ErrorMessage);
        }

        return await next();
    }
}