using FSH.WebApi.Application.Catalog.Brands;
using FSH.WebApi.Domain.Catalog.Fund;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Application.Catalog.Years;
public class UpdateYearRequest:YearDto, IRequest<DefaultIdType>
{

}
public class UpdateYearRequestHandler : IRequestHandler<UpdateYearRequest, DefaultIdType>
{
    private readonly IRepositoryWithEvents<Year> _repository;
    private readonly IStringLocalizer _t;

    public UpdateYearRequestHandler(IRepositoryWithEvents<Year> repository, IStringLocalizer<UpdateBrandRequestHandler> localizer) =>
        (_repository, _t) = (repository, localizer);
    public async Task<DefaultIdType> Handle(UpdateYearRequest request, CancellationToken cancellationToken)
    {
        var year=await _repository.GetByIdAsync(request.Id,cancellationToken);
        _=year ?? throw new NotFoundException(_t["Year {0} Not Found.",request.Id]);
        year.Update(request.year, request.Description);
        await _repository.UpdateAsync(year, cancellationToken);
        return request.Id;
    }
}

