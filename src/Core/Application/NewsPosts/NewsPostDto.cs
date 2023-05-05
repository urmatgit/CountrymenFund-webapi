using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FSH.WebApi.Application.HomePage;

namespace FSH.WebApi.Application.NewsPosts;
public class NewsPostDto : IDto
{
    public DefaultIdType Id { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public string Body { get; set; }
    public string Images { get; set; }
    public List<BlockImageDto> ImagesDto { get; set; } = new List<BlockImageDto>();
}
