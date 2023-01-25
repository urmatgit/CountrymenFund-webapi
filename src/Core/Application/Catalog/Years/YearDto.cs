using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Application.Catalog.Years;
public class YearDto: IDto
{
    public DefaultIdType Id { get; set; }
    public int year { get; set; }
    public string? Description { get; set; }
}
