using FSH.WebApi.Domain.Catalog.Fund;
using FSH.WebApi.Domain.Common.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Application.NewsPosts;

public class DeleteNewsPostRequest : IRequest<DefaultIdType>
{
    public DefaultIdType Id { get; set; }

    public DeleteNewsPostRequest(DefaultIdType id) => Id = id;
}

public class DeleteNewsPostRequestHandler : IRequestHandler<DeleteNewsPostRequest, DefaultIdType>
{
    // Add Domain Events automatically by using IRepositoryWithEvents
    private readonly IRepositoryWithEvents<NewsPost> _repository;
    private readonly IStringLocalizer _t;

    public DeleteNewsPostRequestHandler(IRepositoryWithEvents<NewsPost> repository, IStringLocalizer<DeleteNewsPostRequestHandler> localizer) =>
        (_repository, _t) = (repository, localizer);

    public async Task<DefaultIdType> Handle(DeleteNewsPostRequest request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);

        _ = entity ?? throw new NotFoundException(_t["Entity {0} Not Found."]);

        await _repository.DeleteAsync(entity, cancellationToken);

        return request.Id;
    }
}

