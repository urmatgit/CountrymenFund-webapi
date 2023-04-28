using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Application.HomePage;
public class NewsPostDto:IDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public string Body { get; set; }
    //public string Images { get; set; } 
    public List<BlockImageDto> Images { get; set; } = new List<BlockImageDto>();
}
