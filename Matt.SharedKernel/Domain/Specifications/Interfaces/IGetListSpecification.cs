using Matt.Paginated;

namespace Matt.SharedKernel.Domain.Specifications.Interfaces;

public interface IGetListSpecification<TLEntity> : ISpecification<TLEntity>;
public interface IPaginatedGetListSpecification<TLEntity> : ISpecification<TLEntity>, IPaginated;