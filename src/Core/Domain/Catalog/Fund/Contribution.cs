using FSH.WebApi.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Domain.Catalog.Fund;
public class Contribution: AuditableEntity,IAggregateRoot
{
    public decimal Summa { get; set; }
    public Months Month { get; set; }
    public DateTime Date { get; set; }
    public DefaultIdType NativeId { get; set; }
    public virtual Native Native { get; set; }
    public DefaultIdType YearId { get; set; }
    public virtual Year Year { get; set; }
    public string? Description { get; set; } 
    public Contribution Update(decimal summa,Months month, DateTime date,DefaultIdType nativeId,DefaultIdType yearId,string? description)
    {
        if (!Summa.Equals(summa)) Summa= summa;
        if (!Month.Equals(month)) Month= month;
        if (!Date.Equals(date)) Date = date;
        if( !NativeId.Equals(nativeId)) NativeId= nativeId;
        if (!YearId.Equals(yearId)) YearId= yearId;
        if (description is not null && Description?.Equals(description) is not true) Description = description;
        return this;
    }
         
}
