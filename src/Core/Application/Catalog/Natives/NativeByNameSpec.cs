using FSH.WebApi.Domain.Catalog.Fund;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Application.Catalog.Natives;
public class NativeByNameSpec: Specification<Native>,ISingleResultSpecification
{
    public NativeByNameSpec(string name) => Query.Where(n => n.Name == name);
}
