using FSH.WebApi.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FSH.WebApi.Domain.Common;

public class MainPageModel
{
    public const string NameJson = "HomePage.json";
    public bool ShowArrows { get; set; } = true;
    public bool ShowBullets { get; set; } = true;
    public bool EnableSwapGesture { get; set; } = true;
    public bool AutoCycle { get; set; }
    public int AutoCycleTime { get; set; } = 5;
    public int Height { get; set; } = 200;
    
    public List<TextBlock> TextBlocs { get; set; }
    public List<Slider> Sliders { get; set; }
    public MainPageModel()
    {
        TextBlocs= new List<TextBlock>();
        Sliders = new List<Slider>();
    }
}
public class TextBlock
{
    public Guid Id { get; set; }
    public string Caption { get; set; }
    public string Text { get; set; }
    public string? TitleImage { get; set; } = "";
    public List<BlockImage> Images { get; set; } = new List<BlockImage>();

}

public class BlockImage
{
    public string Name { get; set; }

}

public class Slider
{

    //If  a slide type is an image, then value contain is a path to file
    public string? ImagePath { get; set; } = "";
    public string? Title { get; set; }= "";
    public string? Description { get; set; } = "";
    public SlideTransition Transition { get; set; } = SlideTransition.Slide;

}
