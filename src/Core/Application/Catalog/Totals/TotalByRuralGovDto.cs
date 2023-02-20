using FSH.WebApi.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Application.Catalog.Totals;
public class TotalDto: IDto
    {
    public int Year { get; set; }
    public string RuralGovName { get; set; }
    public string? Style { get; set; }
    public decimal AllSumm { get; set; }
}
public class TotalByRuralGovDto:TotalDto
{
    public Months Month { get; set; }
    public decimal Summa { get; set; }

}
public class TotalByNative : TotalWithMonths
{
    public string FIO { get; set; }
    public string Village { get; set; }
}
public  class TotalWithMonths: TotalDto
{
    public decimal? January { get; set; }
    public decimal? February { get; set; }
    public decimal? March { get; set; }
    public decimal? April { get; set; }
    public decimal? May { get; set; }
    public decimal? June { get; set; }
    public decimal? July { get; set; }
    public decimal? August { get; set; }
    public decimal? September { get; set; }
    public decimal? October { get; set; }
    public decimal? November { get; set; }
    public decimal? December { get; set; }
}
