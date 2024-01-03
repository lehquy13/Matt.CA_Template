using Matt.ResultObject;
using Matt.SharedKernel.Domain.Interfaces;
using MediatR;

namespace Matt.SharedKernel.Application.Mediators.Commands;

public abstract class CommandHandlerBase<TCommand, TResult> : RequestHandlerBase,
    IRequestHandler<TCommand, Result<TResult>>
    where TCommand : ICommandRequest<TResult>
    where TResult : class
{
    public abstract Task<Result<TResult>> Handle(TCommand command, CancellationToken cancellationToken);

    protected CommandHandlerBase(IUnitOfWork unitOfWork, IAppLogger<RequestHandlerBase> logger) : base(unitOfWork,
        logger)
    {
    }
}

public abstract class CommandHandlerBase<TCommand> : RequestHandlerBase, IRequestHandler<TCommand, Result>
    where TCommand : ICommandRequest
{
    public abstract Task<Result> Handle(TCommand command, CancellationToken cancellationToken);

    protected CommandHandlerBase(IUnitOfWork unitOfWork, IAppLogger<RequestHandlerBase> logger) : base(unitOfWork,
        logger)
    {
    }
}