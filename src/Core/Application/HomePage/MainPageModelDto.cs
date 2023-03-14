using FSH.WebApi.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Application.HomePage;
public class MainPageModelDto : IDto
{
    public const string NameJson = "HomePage.json";
    public bool ShowArrows { get; set; } = true;
    public bool ShowBullets { get; set; } = true;
    public bool EnableSwapGesture { get; set; } = true;
    public bool AutoCycle { get; set; }
    public int AutoCycleTime { get; set; } = 5;
    public int Height { get; set; } = 200;

    public List<TextBlockDto> TextBlocs { get; set; }
    public List<SliderDto> Sliders { get; set; }
    public MainPageModelDto()
    {
        TextBlocs = new List<TextBlockDto>();
        Sliders = new List<SliderDto>();
    }
}
public class TextBlockDto: IDto
{
    public Guid Id { get; set; }
    public string Caption { get; set; }
    public string Text { get; set; }
    public string? TitleImage { get; set; } = "";
    public List<BlockImageDto> Images { get; set; } = new List<BlockImageDto>();

}
public class BlockImageDto
{
    public string Name { get; set; }
    public FileUploadRequest? Image { get; set; }
    public string? ImageInBytes { get; set; }
    public string? ImageExtension { get; set; }

}

public class SliderDto: IDto
{

    //If  a slide type is an image, then value contain is a path to file
    public string? ImagePath { get; set; } = "";
    public string? Title { get; set; } = "";
    public string? Description { get; set; } = "";
    public SlideTransition Transition { get; set; } = SlideTransition.Slide;
    public FileUploadRequest? Image { get; set; }

}
