using FSH.WebApi.Shared.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Application.HomePage;
public class GetHomePageRequest: IRequest<MainPageModel>
{
}

public class GetHomePageRequestHandler : IRequestHandler<GetHomePageRequest, MainPageModel>
{
    private readonly IFileStorageService _file;
    private readonly ISerializerService _serializerService;
    public GetHomePageRequestHandler(IFileStorageService file, ISerializerService serializerService)
    {
        _file = file;
        _serializerService = serializerService;
    }

    public async Task<MainPageModel> Handle(GetHomePageRequest request, CancellationToken cancellationToken)
    {
        var mainpage =await  _file.GetStringFileAsync(MainPageModel.NameJson);
        if (!string.IsNullOrEmpty(mainpage))
        {
            var mainpageModel=_serializerService.Deserialize<MainPageModel>(mainpage);
            return mainpageModel;
        }
        return null;
    }
}