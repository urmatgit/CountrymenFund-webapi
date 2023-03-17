using FSH.WebApi.Domain.Catalog.Fund;
using FSH.WebApi.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Application.Catalog.Contributions;
public  class ContributionDto: ContributionExportDto
{
    public DefaultIdType Id { get; set; }
    
    public DefaultIdType NativeId { get; set; }
    
    public DefaultIdType RuralGovId { get; set; }
    

    public int Rate { get; set; }
    public DefaultIdType YearId { get; set; }
    
    public ContributionDto() :base() 
    {
    //    this.Month =(Months)DateTime.Now.Month;
    }
}
