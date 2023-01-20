using FSH.WebApi.Domain.Catalog.Fund;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Application.Catalog.Natives;
public class NativeCheckExistSpec: Specification<Native,NativeDto>, ISingleResultSpecification
{
    public NativeCheckExistSpec(string name,string surname,DateTime? birchDate,DefaultIdType ruralId)
    {
        Query.Where(p => p.Name.Equals(name)
            && p.Surname.Equals(surname)
            && p.BirthDate.HasValue && birchDate.HasValue && p.BirthDate == birchDate
            && p.RuralGovId == ruralId);
            
    }
}
