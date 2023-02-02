using FSH.WebApi.Domain.Catalog.Fund;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Application.Catalog.Contributions;

public class GetContributionsSummRequest
{
    private readonly IDapperRepository _repository;
    private readonly IReadRepository<Contribution> _contributionRep;
    public GetContributionsSummRequest(IDapperRepository dapperRepository, IReadRepository<Contribution> contributionRep)
    {
        _repository = dapperRepository;
        _contributionRep = contributionRep;
    }
    public async Task<decimal> GetSumm(CancellationToken cancellationToken)
    {
        return await   _repository.QuerySumValueAsync<Contribution>(p=> true, p=>p.Summa,   cancellationToken);
        
    }
    public async Task<decimal> GetSummBitween(DateTime from, DateTime until, CancellationToken cancellationToken)
    {
        
        return await _repository.QuerySumValueAsync<Contribution>(e => e.Date >= from && e.Date <= until, p => p.Summa, cancellationToken);

    }
    public async Task<IEnumerable<GroupTotal>> GetRuralGovsSummBitween(DateTime from, DateTime until, CancellationToken cancellationToken)
    {
        var ruralSum =await  _repository.QueryRuralGovSumsValue(from, until,cancellationToken);
        
        return (IEnumerable <GroupTotal>)ruralSum;

    }


}
