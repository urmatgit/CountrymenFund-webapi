using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Application.Catalog.Totals;
public class TotalByRuralGovDto:IDto
{
    public int Year { get; set; }
    public int Month { get; set; }
    public string RuralGovName { get; set; }
    public double Summa { get; set; }
}
public class TotalByNative : TotalByRuralGovDto
{
     public string FIO { get; set; }
