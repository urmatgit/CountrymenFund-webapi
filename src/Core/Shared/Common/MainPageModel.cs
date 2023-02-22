using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Shared.Common;
public class MainPageModel
{
    public CarouselModel CarouselModel { get; set; }
    public List<TextBlock> TextBlocs { get; set; }

}
public class TextBlock
{
    public string Caption { get; set; }
    public string Text { get; set; }
}
public class CarouselModel
{
    public bool AutoCycle { get; set; }
    public TimeSpan AutoCycleTime { get; set; } = TimeSpan.FromSeconds(5);
    public int Height { get;set; }
    public int Transition { get; set; }
   public List<Slide> Slides { get; set; }
}

public enum SlideType {
    Text,
    Image
}
public class Slide
{
    public SlideType SlideType { get; set; }
    //If  a slide type is an image, then value contain is a path to file
    public string Value { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }

}
