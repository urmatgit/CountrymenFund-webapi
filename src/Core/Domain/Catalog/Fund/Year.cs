using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FSH.WebApi.Domain.Catalog.Fund;
public class Year: AuditableEntity, IAggregateRoot
{
    public int year { get; private set; }
    public string? Description { get; private set; }
    public Year(int year, string? description)
    {
        this.year = year;
        Description = description;
    }
    public Year Update(int year, string? description)
    {
        if (year !=0 && year!=this.year) this.year = year;
        if (description is not null && Description?.Equals(description) is not true) Description = description;
        return this;
    }
}
