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
    public bool AutoCycle { get; set; }
    public int AutoCycleTime { get; set; } = 5;
    public int Height { get; set; } = 200;

    public List<TextBlockDto> TextBlocs { get; set; }
    public List<SlideDto> Slides { get; set; }
    public MainPageModelDto()
    {
        TextBlocs = new List<TextBlockDto>();
        Slides = new List<SlideDto>();
    }
}
public class TextBlockDto: IDto
{
    public string Caption { get; set; }
    public string Text { get; set; }
    public string? TitleImage { get; set; } = "";
    public List<string> Images { get; set; } = new List<string>();

}


public class SlideDto: IDto
{

    //If  a slide type is an image, then value contain is a path to file
    public string? ImagePath { get; set; } = "";
    public string? Title { get; set; } = "";
    public string? Description { get; set; } = "";
    public SlideTransition Transition { get; set; } = SlideTransition.Slide;
    public FileUploadRequest? Image { get; set; }

}
