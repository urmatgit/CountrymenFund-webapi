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
         
}
