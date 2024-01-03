using MapsterMapper;
using Matt.ResultObject;
using Matt.SharedKernel.Domain.Interfaces;
using MediatR;

namespace Matt.SharedKernel.Application.Mediators.Queries;

public abstract class QueryHandlerBase<TQuery, TResult>(
        IUnitOfWork unitOfWork,
        IAppLogger<RequestHandlerBase> logger,
        IMapper mapper)
    : RequestHandlerBase(unitOfWork, logger), IRequestHandler<TQuery, Result<TResult>>
    where TQuery : IQueryRequest<TResult>
    where TResult : class
{
    protected IMapper Mapper { get; } = mapper;

    public abstract Task<Result<TResult>> Handle(TQuery request, CancellationToken cancellationToken);
}