using FSH.WebApi.Application.Common.FileStorage;
using FSH.WebApi.Application.HomePage.Events;
using FSH.WebApi.Shared.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Application.HomePage;
public class UpdateHomePageRequest:MainPageModel, IRequest<MainPageModel>
{

}
public class UpdateHomePageRequestHandler : IRequestHandler<UpdateHomePageRequest, MainPageModel>
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

    public async Task<MainPageModel> Handle(UpdateHomePageRequest request, CancellationToken cancellationToken)
    {

        var result = _serializerService.Serialize<MainPageModel>(request);
        await _file.SaveStringFileAsync(MainPageModel.NameJson, result);
        await _events.PublishAsync(new MainPageUpdateEvent());
        return request;
    }
}


