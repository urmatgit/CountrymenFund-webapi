using FSH.WebApi.Application.Common.FileStorage;
using FSH.WebApi.Application.Common.Interfaces;
using FSH.WebApi.Infrastructure.Persistence.Initialization;
using FSH.WebApi.Shared.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Infrastructure.Catalog;
public class HomePageSeed : ICustomSeeder
{
    private readonly ISerializerService _serializerService;
    private readonly IFileStorageService _fileStorageService;
    public HomePageSeed(ISerializerService serializerService,IFileStorageService fileStorage   )
    {
        _serializerService = serializerService;
        _fileStorageService = fileStorage;
    }

    public async Task InitializeAsync(CancellationToken cancellationToken)
    {
        string? path =Path.Combine(_fileStorageService.GetRootFilesPath(),"Files", MainPageModel.NameJson);
        if (!File.Exists(path))
        {
            MainPageModel mainPageModel = new MainPageModel();
            var slider = new Slide() {  Title = "Three", ImagePath = "Files/full-stack-hero-logo.png" };
            var sliderimagePath = Path.Combine(_fileStorageService.GetRootFilesPath(), "Sliders");
             if (!Directory.Exists(sliderimagePath)){
                Directory.CreateDirectory(sliderimagePath);
            }

            mainPageModel.CarouselModel = new CarouselModel()
            {
                AutoCycle = true,
                AutoCycleTime =TimeSpan.FromSeconds(5),
                Height = 200,
                
                Slides = new List<Slide> {
                    new Slide() {  Title="One"},
                    new Slide() {  Title="Two"},
                    slider
                }

            };
            mainPageModel.TextBlocs.Add(new TextBlock() { Caption = "Text 1", Text = "This is text one." });
            mainPageModel.TextBlocs.Add(new TextBlock() { Caption = "Text 2", Text = "This is text two." });
          var result=   _serializerService.Serialize<MainPageModel>(mainPageModel);
             await _fileStorageService.SaveStringFileAsync(MainPageModel.NameJson, result);
        }
       // return Task.CompletedTask;
    }
}
