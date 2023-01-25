using FSH.WebApi.Domain.Catalog.Fund;
using FSH.WebApi.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Application.Catalog.Contributions;
public  class ContributionDto: IDto
{
    public DefaultIdType Id { get; set; }
    public decimal Summa { get; set; }
    public Months Month { get; set; }
    public DateTime Date { get; set; }
    public DefaultIdType NativeId { get; set; }
    public string NativeFIO { get; set; }
    public DefaultIdType YearId { get; set; }
    public int  Year { get; set; }
    public string? Description { get; set; }
    public ContributionDto()
    {
        this.Month =(Months)DateTime.Now.Month;
    }
}
