using FSH.WebApi.Domain.Catalog.Fund;
using FSH.WebApi.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Application.Catalog.Contributions;
public  class ContributionExportDto : IDto
{
    public int Year { get; set; }
    public string? RuralGovName { get; set; }
    public string? NativeFIO { get; set; }
    public Months Month { get; set; } = (Months)DateTime.Now.Month;
    public decimal Summa { get; set; }
    public DateTime? Date { get; set; }= DateTime.Now;
    
    public string? Village { get; set; }
    public string? Description { get; set; }
    public ContributionExportDto()
    {
        this.Month =(Months)DateTime.Now.Month;
    }
}
