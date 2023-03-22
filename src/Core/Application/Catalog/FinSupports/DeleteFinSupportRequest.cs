using FSH.WebApi.Application.Catalog.Products;
using FSH.WebApi.Domain.Catalog.Fund;

namespace FSH.WebApi.Application.Catalog.FinSupports;

public class DeleteFinSupportRequest : IRequest<Guid>
{
    public Guid Id { get; set; }

    public DeleteFinSupportRequest(Guid id) => Id = id;
}

public class DeleteFinSupportRequestHandler : IRequestHandler<DeleteFinSupportRequest, Guid>
{
    // Add Domain Events automatically by using IRepositoryWithEvents
    private readonly IRepositoryWithEvents<FinSupport> _brandRepo;
    private readonly IReadRepository<Product> _productRepo;
    private readonly IStringLocalizer _t;

    public DeleteFinSupportRequestHandler(IRepositoryWithEvents<FinSupport> brandRepo, IReadRepository<Product> productRepo, IStringLocalizer<DeleteFinSupportRequestHandler> localizer) =>
        (_brandRepo, _productRepo, _t) = (brandRepo, productRepo, localizer);

    public async Task<Guid> Handle(DeleteFinSupportRequest request, CancellationToken cancellationToken)
    {
        //if (await _productRepo.AnyAsync(new ProductsByFinSupportSpec(request.Id), cancellationToken))
        //{
        //    throw new ConflictException(_t["FinSupport cannot be deleted as it's being used."]);
        //}

        var brand = await _brandRepo.GetByIdAsync(request.Id, cancellationToken);

        _ = brand ?? throw new NotFoundException(_t["FinSupport {0} Not Found."]);

        await _brandRepo.DeleteAsync(brand, cancellationToken);

        return request.Id;
    }
}