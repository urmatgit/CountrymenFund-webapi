using FSH.WebApi.Domain.Catalog.Fund;
using FSH.WebApi.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Application.Catalog.FSContributions;
public  class FSContributionExportDto : IDto
{
    
    
    public string? NativeFIO { get; set; }
    public string? FinSuportName { get; set; }
    public decimal Summa { get; set; }
    public DateTime? Date { get; set; }= DateTime.Now;
    
    
    public string? Description { get; set; }
    public FSContributionExportDto()
    {
    
    }
}
