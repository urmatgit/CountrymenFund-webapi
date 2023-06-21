using FSH.WebApi.Application.Catalog.Totals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Application.Catalog.Contributions;
public class ImportContributionDto: TotalByNative
{
    public string Name { get; set; }
    public string Surname { get; set; }
}
