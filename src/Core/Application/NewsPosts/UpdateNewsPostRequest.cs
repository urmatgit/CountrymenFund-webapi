using FSH.WebApi.Domain.Catalog.Fund;
using FSH.WebApi.Domain.Common.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Application.NewsPosts;

public class UpdateNewsPostRequest : NewsPostDto, IRequest<DefaultIdType>
{


}

public class UpdateNewsPostRequestHandler : IRequestHandler<UpdateNewsPostRequest, DefaultIdType>
{
    // Add Domain Events automatically by using IRepositoryWithEvents
    private readonly IRepositoryWithEvents<NewsPost> _repository;
    private readonly IStringLocalizer _t;
    private readonly IFileStorageService _file;
    private readonly ICurrentUser _currentUser;
    public UpdateNewsPostRequestHandler(IRepositoryWithEvents<NewsPost> repository, IStringLocalizer<UpdateNewsPostRequestHandler> localizer, IFileStorageService file,ICurrentUser currentUser) =>
        (_repository, _t, _file,_currentUser) = (repository, localizer, file,currentUser);

    public async Task<DefaultIdType> Handle(UpdateNewsPostRequest request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);

        _ = entity
        ?? throw new NotFoundException(_t["Entity {0} Not Found.", request.Id]);
        if (string.IsNullOrEmpty(entity.Author))
        {
            entity.Author = $"{_currentUser.Name}";
        }
        entity.Update(request.Title, request.Author, request.Body);

        //List<string> imageList = new List<string>();
        //foreach (var image in request.Images)
        //{
        //    if (image.Image is not null)
        //    {
        //        imageList.Add(await _file.UploadAsync<NewsPost>(image.Image, FileType.Image, cancellationToken, request.Id.ToString()));
        //    }
        //}

        //entity.Images = string.Join(';', imageList);
        await _repository.UpdateAsync(entity, cancellationToken);

        return request.Id;
    }
}
