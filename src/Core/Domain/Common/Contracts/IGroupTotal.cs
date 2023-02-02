using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Domain.Common.Contracts;
public interface IGroupTotal
{
     string Name { get; set; }
     decimal Total { get; set; }
}
