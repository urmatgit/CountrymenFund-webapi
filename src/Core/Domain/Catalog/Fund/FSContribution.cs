using FSH.WebApi.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Domain.Catalog.Fund;
public class FSContribution : AuditableEntity, IAggregateRoot
{
    public decimal Summa { get; set; }
    public DateTime Date { get; set; }
    public DefaultIdType NativeId { get; set; }
    public virtual Native Native { get; set; }
    public string? Description { get; set; }
    public DefaultIdType FinSupportId { get; set; }
    public FinSupport FinSupport { get; set; }
    public FSContribution Update(decimal summa, DateTime? date, DefaultIdType nativeId, string? description)
    {
        if (!Summa.Equals(summa)) Summa = summa;

        if (!Date.Equals(date)) Date = date.Value.ToUniversalTime();
        if (!NativeId.Equals(nativeId)) NativeId = nativeId;
        if (description is not null && Description?.Equals(description) is not true) Description = description;
        return this;
    }
}