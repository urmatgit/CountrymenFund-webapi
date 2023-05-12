using FSH.WebApi.Application.Common.Interfaces;
using FSH.WebApi.Domain.Catalog.Fund;
namespace FSH.WebApi.Application.NewsPosts;

public class CreateNewsPostRequest : NewsPostDto, IRequest<DefaultIdType>
{

}

public class CreateNewsPostRequestHandler : IRequestHandler<CreateNewsPostRequest, DefaultIdType>
{
    // Add Domain Events automatically by using IRepositoryWithEvents
    private readonly IRepositoryWithEvents<NewsPost> _repository;
    private readonly IFileStorageService _file;
    private readonly ICurrentUser _currentUser;
    public CreateNewsPostRequestHandler(IRepositoryWithEvents<NewsPost> repository, IFileStorageService fileStorageService,ICurrentUser currentUser) => (_repository, _file,_currentUser) = (repository, fileStorageService,currentUser);

    public async Task<DefaultIdType> Handle(CreateNewsPostRequest request, CancellationToken cancellationToken)
    {
        var entity = new NewsPost(request.Title,
                                  request.Author,
                                  request.Body);

                    entity.Author = $"{_currentUser.Name}";
        

        var newspost = await _repository.AddAsync(entity, cancellationToken);

        //List<string> imageList = new List<string>();
        //    foreach (var image in request.Images)
        //    {
        //        if (image.Image is not null)
        //        {
        //        imageList.Add(await _file.UploadAsync<NewsPost>(image.Image, FileType.Image, cancellationToken, entity.Id.ToString()));
        //        }
        //    }

        //newspost.Images = string.Join(';', imageList);
        //await _repository.UpdateAsync(entity, cancellationToken);
        return entity.Id;
    }
}

