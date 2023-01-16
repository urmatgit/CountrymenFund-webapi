using FSH.WebApi.Domain.Catalog.Fund;

using FSH.WebApi.Domain.Common.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Application.Catalog.RuralGovs;
public class UpdateRuralGovRequest: IRequest<DefaultIdType>
{
    public DefaultIdType Id { get; set; }
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public string? Coordinate { get; set; }
}
public class UpdateRuralRequestValidator: CustomValidator<UpdateRuralGovRequest>
{
    public UpdateRuralRequestValidator(IRepository<RuralGov> repository, IStringLocalizer<UpdateRuralRequestValidator> T)
    => RuleFor(p => p.Name)
        .NotEmpty()
        .MaximumLength(100)
        .MustAsync(async (ruralGov, name, ct) =>
        await repository.GetBySpecAsync(new RuralGovByNameSpec(name), ct)
         is not RuralGov existingRuralGov || existingRuralGov.Id == ruralGov.Id)
        .WithMessage((_, name) => T["Rural goverment {0} already exists", name]
        );
}
public class UpdateRuralGovRequestHanlde : IRequestHandler<UpdateRuralGovRequest, DefaultIdType>
{
    private readonly IRepositoryWithEvents<RuralGov> _repository;
    private readonly IStringLocalizer l;
    public UpdateRuralGovRequestHanlde(IRepositoryWithEvents<RuralGov> repository, IStringLocalizer<UpdateRuralGovRequestHanlde> l)
    {
        _repository = repository;
        this.l = l;
    }
    public async Task<DefaultIdType> Handle(UpdateRuralGovRequest request, CancellationToken cancellationToken)
    {
        var ruralGov = await _repository.GetByIdAsync(request.Id, cancellationToken);
        _ = ruralGov ?? throw new NotFoundException(this.l["Rural goverment {0} Not Found.", request.Id]);
        ruralGov.Update(request.Name, request.Description,request.Coordinate);
        await _repository.UpdateAsync(ruralGov, cancellationToken);
        return request.Id;
    }
}