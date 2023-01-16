using FSH.WebApi.Domain.Catalog.Fund;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Application.Catalog.RuralGovs;
public class RuralGovByNameSpec:Specification<RuralGov>, ISingleResultSpecification
{
    public RuralGovByNameSpec(string name)
    => Query.Where(r => r.Name == name);
}
