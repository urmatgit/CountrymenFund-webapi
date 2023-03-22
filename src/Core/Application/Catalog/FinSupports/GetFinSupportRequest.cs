using FSH.WebApi.Domain.Catalog.Fund;

namespace FSH.WebApi.Application.Catalog.FinSupports;

public class GetFinSupportRequest : IRequest<FinSupportDto>
{
    public Guid Id { get; set; }

    public GetFinSupportRequest(Guid id) => Id = id;
}

public class FinSupportByIdSpec : Specification<FinSupport, FinSupportDto>, ISingleResultSpecification
{
    public FinSupportByIdSpec(Guid id) =>
        Query.Where(p => p.Id == id);
}

public class GetFinSupportRequestHandler : IRequestHandler<GetFinSupportRequest, FinSupportDto>
{
    private readonly IRepository<FinSupport> _repository;
    private readonly IStringLocalizer _t;

    public GetFinSupportRequestHandler(IRepository<FinSupport> repository, IStringLocalizer<GetFinSupportRequestHandler> localizer) => (_repository, _t) = (repository, localizer);

    public async Task<FinSupportDto> Handle(GetFinSupportRequest request, CancellationToken cancellationToken) =>
        await _repository.GetBySpecAsync(
            (ISpecification<FinSupport, FinSupportDto>)new FinSupportByIdSpec(request.Id), cancellationToken)
        ?? throw new NotFoundException(_t["FinSupport {0} Not Found.", request.Id]);
}