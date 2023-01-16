using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Domain.Catalog.Fund;
public class RuralGov : AuditableEntity, IAggregateRoot
{
    public string Name { get; private set; }
    public string? Description { get; private set; }
    public string? Coordinate { get; set; }
    public Guid? DistrictId { get; private set; }
    public RuralGov(string name, string? description)
    {
        Name = name;
        Description = description;
    }

    public RuralGov Update(string? name, string? description,string? coordinate)
    {
        if (name is not null && Name?.Equals(name) is not true) Name = name;
        if (description is not null && Description?.Equals(description) is not true) Description = description;
        if (coordinate is not null && Coordinate?.Equals(coordinate) is not true) Coordinate = coordinate;
        return this;
    }
}
