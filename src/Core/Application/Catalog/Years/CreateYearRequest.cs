using FSH.WebApi.Domain.Catalog.Fund;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Application.Catalog.Years;
public class CreateYearRequest: YearDto, IRequest<DefaultIdType>
{
    public new DefaultIdType Id { get; private set; }

}
public class CreateYearRequestValidator:CustomValidator<CreateYearRequest>
{
    public CreateYearRequestValidator(IReadRepository<Year> readRepository, IStringLocalizer<CreateYearRequestValidator> T) =>
        RuleFor(p => p.year)
        .NotEmpty()
        .InclusiveBetween(2000, 2100)
        .MustAsync(async (year, ct) => await readRepository.GetBySpecAsync(new YearbyYearSpec(year), ct) is null)
        .WithMessage((_, year) => T["Year {0} already exists.", year]);

}
public class CreateYearRequestHangler : IRequestHandler<CreateYearRequest, DefaultIdType>
{
    private readonly IRepositoryWithEvents<Year> _repository;
    private readonly IStringLocalizer _l;
    public CreateYearRequestHangler(IRepositoryWithEvents<Year> repository, IStringLocalizer<CreateYearRequestHangler> l)
    {
        _repository = repository;
        _l = l;
    }

    public async Task<DefaultIdType> Handle(CreateYearRequest request, CancellationToken cancellationToken)
    {
        var year = new Year(request.year, request.Description);
        await _repository.AddAsync(year, cancellationToken);
        return year.Id;
    }
}
