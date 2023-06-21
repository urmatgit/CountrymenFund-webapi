using FSH.WebApi.Domain.Catalog.Fund;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Application.Catalog.Natives;
public class NativeCheckExistSpec: Specification<Native>, ISingleResultSpecification
{
    public NativeCheckExistSpec(string name,string surname,DateTime? birchDate,DefaultIdType ruralId,bool? hasImage)
    {
        Query.Where(p => p.Name.Equals(name)
            && p.Surname.Equals(surname))
            .Where(p => p.BirthDate == birchDate, birchDate.HasValue)
            .Where(p => p.RuralGovId == ruralId);
            
    }
}
