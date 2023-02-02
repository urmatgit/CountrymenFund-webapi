using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Domain.Catalog.Fund;
public class GroupTotal : IGroupTotal
{
    public string Name { get; set; }
    public decimal Total { get; set; }
}
