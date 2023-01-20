using FSH.WebApi.Domain.Catalog.Fund;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Application.Catalog.Natives;
public class NativesByRuralGovIdSpec: Specification<Native>
{
    public NativesByRuralGovIdSpec(DefaultIdType ruralGovId) => Query.Where(n => n.RuralGovId == ruralGovId);
}
