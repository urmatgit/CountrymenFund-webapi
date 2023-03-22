using FSH.WebApi.Domain.Catalog.Fund;

namespace FSH.WebApi.Application.Catalog.FinSupports;

public class UpdateFinSupportRequest : IRequest<Guid>
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public DateTime? Begin { get; set; }
    public DateTime? End { get; set; }
    public DefaultIdType NativeId { get; set; }
}

public class UpdateFinSupportRequestValidator : CustomValidator<UpdateFinSupportRequest>
{
    public UpdateFinSupportRequestValidator(IRepository<FinSupport> repository, IStringLocalizer<UpdateFinSupportRequestValidator> T) =>
        RuleFor(p => p.Name)
            .NotEmpty()
            .MaximumLength(75)
            .MustAsync(async (brand, name, ct) =>
                    await repository.GetBySpecAsync(new FinSupportByNameSpec(name), ct)
                        is not FinSupport existingFinSupport || existingFinSupport.Id == brand.Id)
                .WithMessage((_, name) => T["FinSupport {0} already Exists.", name]);
}

public class UpdateFinSupportRequestHandler : IRequestHandler<UpdateFinSupportRequest, Guid>
{
    // Add Domain Events automatically by using IRepositoryWithEvents
    private readonly IRepositoryWithEvents<FinSupport> _repository;
    private readonly IStringLocalizer _t;

    public UpdateFinSupportRequestHandler(IRepositoryWithEvents<FinSupport> repository, IStringLocalizer<UpdateFinSupportRequestHandler> localizer) =>
        (_repository, _t) = (repository, localizer);

    public async Task<Guid> Handle(UpdateFinSupportRequest request, CancellationToken cancellationToken)
    {
        var brand = await _repository.GetByIdAsync(request.Id, cancellationToken);

        _ = brand
        ?? throw new NotFoundException(_t["FinSupport {0} Not Found.", request.Id]);

        brand.Update(request.Name, request.Description,request.Begin.Value,request.End,request.NativeId);

        await _repository.UpdateAsync(brand, cancellationToken);

        return request.Id;
    }
}