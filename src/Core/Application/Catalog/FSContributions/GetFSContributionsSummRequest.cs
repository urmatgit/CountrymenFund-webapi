using FSH.WebApi.Domain.Catalog.Fund;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Application.Catalog.FSContributions;

public class GetFSContributionsSummRequest
{
    private readonly IDapperRepository _repository;
    private readonly IReadRepository<FSContribution> _FSContributionRep;
    public GetFSContributionsSummRequest(IDapperRepository dapperRepository, IReadRepository<FSContribution> FSContributionRep)
    {
        _repository = dapperRepository;
        _FSContributionRep = FSContributionRep;
    }
    public async Task<decimal> GetSumm(CancellationToken cancellationToken)
    {
        return await   _repository.QuerySumValueAsync<FSContribution>(p=> true, p=>p.Summa,   cancellationToken);
        
    }
    public async Task<decimal> GetSummBitween(DateTime from, DateTime until, CancellationToken cancellationToken)
    {
        
        return await _repository.QuerySumValueAsync<FSContribution>(e => e.Date >= from && e.Date <= until, p => p.Summa, cancellationToken);

    }
    public async Task<IEnumerable<GroupTotal>> GetFinSupportSummBitween(DateTime from, DateTime until, CancellationToken cancellationToken)
    {
        var ruralSum =await  _repository.QueryFinSupportSumsValue(from, until,cancellationToken);
        
        return (IEnumerable <GroupTotal>)ruralSum;

    }


}
