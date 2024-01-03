using Matt.SharedKernel.Domain.Interfaces;

namespace Matt.SharedKernel.Application.Mediators;

public abstract class RequestHandlerBase
{
    protected readonly IUnitOfWork UnitOfWork;
    protected readonly IAppLogger<RequestHandlerBase> Logger;

    protected RequestHandlerBase(IUnitOfWork unitOfWork, IAppLogger<RequestHandlerBase> logger)
    {
        UnitOfWork = unitOfWork;
        Logger = logger;
    }
}