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
    public bool AutoCycle { get; set; }
    public int AutoCycleTime { get; set; } = 5;
    public int Height { get; set; } = 200;
    
    public List<TextBlock> TextBlocs { get; set; }
    public List<Slide> Slides { get; set; }
    public MainPageModel()
    {
        TextBlocs= new List<TextBlock>();
        Slides = new List<Slide>();
    }
}
public class TextBlock
{
    public string Caption { get; set; }
    public string Text { get; set; }
    public string? TitleImage { get; set; } = "";
    public List<string> Images { get; set; } = new List<string>();
        
}


public class Slide
{

    //If  a slide type is an image, then value contain is a path to file
    public string? ImagePath { get; set; } = "";
    public string? Title { get; set; }= "";
    public string? Description { get; set; } = "";
    public SlideTransition Transition { get; set; } = SlideTransition.Slide;

}
