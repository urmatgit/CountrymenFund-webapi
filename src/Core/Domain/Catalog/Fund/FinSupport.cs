using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Domain.Catalog.Fund;
public class FinSupport : AuditableEntity, IAggregateRoot
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public DateTime Begin { get; set; }
    public DateTime? End { get; set; }
    public DefaultIdType NativeId { get; set; }
    /// <summary>
    /// Responsible person, Manager
    /// </summary>
    public virtual Native Native { get; set; }
    public FinSupport Update(string name,string description,DateTime begin,DateTime? end,DefaultIdType nativeId)
    {
        Name= name;
        Description= description;
        if (Begin!=begin.ToUniversalTime())
            Begin=  begin.ToUniversalTime();
        End = end.HasValue ? end.Value.ToUniversalTime() : end;
        NativeId = nativeId;
        return this;

    }
    public FinSupport(string name, string description, DateTime begin, DateTime? end, DefaultIdType nativeId)
    {
        Name = name;
        Description = description;
        Begin = begin;
        End = end.HasValue? end.Value.ToUniversalTime(): end;
        NativeId = nativeId;
    }

}
