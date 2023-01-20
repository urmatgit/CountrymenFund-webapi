using FSH.WebApi.Domain.Catalog.Fund;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Application.Catalog.Natives;
public class NativeByIdWithRuralGovSpec: Specification<Native,NativeDto>,ISingleResultSpecification
{
    public NativeByIdWithRuralGovSpec(DefaultIdType id) => Query
        .Where(n => n.Id == id)
        .Include(r => r.RuralGov);
}
