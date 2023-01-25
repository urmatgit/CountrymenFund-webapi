using FSH.WebApi.Domain.Catalog.Fund;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Application.Catalog.Years;
public class YearbyYearSpec: Specification<Year, YearDto>,ISingleResultSpecification
{
    public YearbyYearSpec(int year) => Query.Where(y => y.year == year);
}
public class YearByIdSpec : Specification<Year, YearDto>, ISingleResultSpecification
{
    public YearByIdSpec(DefaultIdType id) => Query.Where(y => y.Id == id);
}