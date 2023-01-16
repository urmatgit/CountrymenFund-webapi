using FSH.WebApi.Domain.Catalog.Fund;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Application.Catalog.RuralGovs;
public class CreateRuralGovRequest:IRequest<DefaultIdType>
{
    public DefaultIdType Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Coordinate { get; set; }
}
public class CreateRuralGovRequestValidator: CustomValidator<CreateRuralGovRequest>
{
    public CreateRuralGovRequestValidator(IReadRepository<RuralGov> repository, IStringLocalizer<CreateRuralGovRequest> T)
        => RuleFor(p => p.Name)
        .NotEmpty()
        .MaximumLength(100)
        .MustAsync(async (name, ct) => await repository.GetBySpecAsync(new RuralGovByNameSpec(name), ct) is null)
        .WithMessage((_, name) => T["Rural goverment {0} already Exists."]);
}
public class CreateRuralGovRequestHandler: IRequestHandler<CreateRuralGovRequest, DefaultIdType>
{
    private readonly IRepositoryWithEvents<RuralGov> _repository;
    public CreateRuralGovRequestHandler(IRepositoryWithEvents<RuralGov> repository)
    {
        _repository = repository;
    }

    public async Task<DefaultIdType> Handle(CreateRuralGovRequest request, CancellationToken cancellationToken)
    {
        var ruralGov=new RuralGov(request.Name,request.Description);
        ruralGov.Coordinate=request.Coordinate;
        await _repository.AddAsync(ruralGov,cancellationToken);
        return ruralGov.Id;
    }
}
