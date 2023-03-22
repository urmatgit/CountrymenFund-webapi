using FSH.WebApi.Domain.Catalog.Fund;

namespace FSH.WebApi.Application.Catalog.FinSupports;

public class CreateFinSupportRequest : IRequest<Guid>
{
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public DateTime Begin { get; set; }
    public DateTime? End { get; set; }
    public DefaultIdType NativeId { get; set; }
}

public class CreateFinSupportRequestValidator : CustomValidator<CreateFinSupportRequest>
{
    public CreateFinSupportRequestValidator(IReadRepository<FinSupport> repository, IStringLocalizer<CreateFinSupportRequestValidator> T)
    {
        RuleFor(p => p.Name)
            .NotEmpty()
            .MaximumLength(75)
            .MustAsync(async (name, ct) => await repository.GetBySpecAsync(new FinSupportByNameSpec(name), ct) is null)
                .WithMessage((_, name) => T["FinSupport {0} already Exists.", name]);
        RuleFor(p => p.Begin)
            .NotEmpty()
            .MustAsync(async (begin,ct) =>
            {
                return begin.Date >= DateTime.Now.Date;
            })
            .WithMessage((_, d) => T["Begin don`t must be less of date now."]);
    }
     
}

public class CreateFinSupportRequestHandler : IRequestHandler<CreateFinSupportRequest, Guid>
{
    // Add Domain Events automatically by using IRepositoryWithEvents
    private readonly IRepositoryWithEvents<FinSupport> _repository;

    public CreateFinSupportRequestHandler(IRepositoryWithEvents<FinSupport> repository) => _repository = repository;

    public async Task<Guid> Handle(CreateFinSupportRequest request, CancellationToken cancellationToken)
    {
        var brand = new FinSupport(request.Name, request.Description,request.Begin.ToUniversalTime(),request.End,request.NativeId);

        await _repository.AddAsync(brand, cancellationToken);

        return brand.Id;
    }
}