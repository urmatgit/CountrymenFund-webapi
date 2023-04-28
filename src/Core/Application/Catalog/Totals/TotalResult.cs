using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Application.Catalog.Totals;
public class TotalResult<T>

{
    public T Data { get; set; }
    public int TotalCount {get;set;}
}
