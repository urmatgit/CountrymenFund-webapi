using FSH.WebApi.Application.Common.FileStorage;
using FSH.WebApi.Application.HomePage.Events;

using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Application.HomePage;
public class UpdateHomePageRequest: MainPageModelDto, IRequest<MainPageModelDto>
{

}
public class UpdateHomePageRequestHandler : IRequestHandler<UpdateHomePageRequest, MainPageModelDto>
{
    private readonly IFileStorageService _file;
    private readonly ISerializerService _serializerService;
    private readonly IEventPublisher _events;
    public UpdateHomePageRequestHandler(IFileStorageService file, ISerializerService serializerService, IEventPublisher events)
    {
        _file = file;
        _serializerService = serializerService;
        _events = events;
    }

    public async Task<MainPageModelDto> Handle(UpdateHomePageRequest request, CancellationToken cancellationToken)
    {
        
        foreach(var slider in request.Slides)
        {
            if (slider.Image is not null) {
                
                slider.ImagePath = await _file.UploadAsync<MainPageModel>(slider.Image, FileType.Image, cancellationToken);
            }
        }
        var mainpageModel = request.Adapt<MainPageModel>();
        var result = _serializerService.Serialize<MainPageModel>(mainpageModel);
        await _file.SaveStringFileAsync(MainPageModel.NameJson, result);
        await _events.PublishAsync(new MainPageUpdateEvent());
        return request;
    }
}


