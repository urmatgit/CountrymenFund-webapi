using FSH.WebApi.Domain.Catalog.Fund;

namespace FSH.WebApi.Application.Catalog.FinSupports;

public class FinSupportByNameSpec : Specification<FinSupport>, ISingleResultSpecification
{
    public FinSupportByNameSpec(string name) =>
        Query.Where(b => b.Name == name);
}