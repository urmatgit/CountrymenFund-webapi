using FSH.WebApi.Domain.Catalog.Fund;
using FSH.WebApi.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Application.Catalog.FSContributions;
public  class FSContributionDto: FSContributionExportDto
{
    public DefaultIdType Id { get; set; }
    
    public DefaultIdType NativeId { get; set; }
    public DefaultIdType FinSupportId { get; set; }
    public FSContributionDto() :base() 
    {
    
    }
    public FSContributionDto Update(decimal summa, DateTime? date, DefaultIdType nativeId, string? description, DefaultIdType finSupportId)
    {
        if (!Summa.Equals(summa)) Summa = summa;
        if (!FinSupportId.Equals(finSupportId)) FinSupportId = finSupportId;
        if (!Date.Equals(date)) Date = date.Value.ToUniversalTime();
        if (!NativeId.Equals(nativeId)) NativeId = nativeId;
        if (description is not null && Description?.Equals(description) is not true) Description = description;
        return this;
    }
}
