using Matt.AutoDI;
using Matt.SharedKernel.Domain.Interfaces;

namespace Acme.Domain.DomainServices;

public abstract class DomainServiceBase(IAppLogger<IdentityDomainServices> logger) : IDomainService
{
    protected readonly IAppLogger<IdentityDomainServices> Logger = logger;
}