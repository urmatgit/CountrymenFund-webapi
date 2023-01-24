using FSH.WebApi.Application.Catalog.Contributions;
using FSH.WebApi.Domain.Catalog.Fund;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Application.Catalog.Years;
public class DeleteYearRequest: IRequest<DefaultIdType>
{
    public DefaultIdType Id { get; set; }
    public DeleteYearRequest(DefaultIdType id)=> Id = id;
    
}
public class DeleteYearRequestHandler : IRequestHandler<DeleteYearRequest, DefaultIdType>
{
    private readonly IRepositoryWithEvents<Year> _repository;
    private readonly IReadRepository<Contribution> _ContributionRep;
    private readonly IStringLocalizer _l;
    public DeleteYearRequestHandler(IRepositoryWithEvents<Year> repository, IReadRepository<Contribution> contributionRep, IStringLocalizer<DeleteYearRequestHandler> l)
    {
        _repository = repository;
        _ContributionRep = contributionRep;
        _l = l;
    }

    public async Task<DefaultIdType> Handle(DeleteYearRequest request, CancellationToken cancellationToken)
    {
        if (await _ContributionRep.AnyAsync(new ContributionsByYearSpec(request.Id), cancellationToken))
        {
            throw new ConflictException(_l["Year cannot be deleted as it's being used."]);
        }
        var year =await _repository.GetByIdAsync(request.Id,cancellationToken);
        _ = year ?? throw new NotFoundException(_l["Year {0}"]);
        await _repository.DeleteAsync(year, cancellationToken);
        return request.Id;
    }
}
