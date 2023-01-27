using FSH.WebApi.Domain.Catalog.Fund;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Application.Catalog.Contributions;

public class GetContributionsSummRequest
{
    private readonly IDapperRepository _repository;
    
    public GetContributionsSummRequest(IDapperRepository dapperRepository)
    {
        _repository= dapperRepository;
        
    }
    public async Task<decimal> GetSumm(CancellationToken cancellationToken)
    {
        return await   _repository.QuerySumValueAsync<Contribution>(p=> true, p=>p.Summa,   cancellationToken);
        
    }
    public async Task<decimal> GetSummBitween(DateTime from, DateTime until, CancellationToken cancellationToken)
    {
        return await _repository.QuerySumValueAsync<Contribution>(e => e.Date >= from && e.Date <= until, p => p.Summa, cancellationToken);

    }

}
