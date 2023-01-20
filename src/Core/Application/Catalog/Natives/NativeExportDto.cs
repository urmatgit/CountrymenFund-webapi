using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Application.Catalog.Natives;
public class NativeExportDto: IDto
{
    public string Name { get;   set; }
    public string Surname { get;   set; }
    public string? MiddleName { get;   set; }
    public DateTime? BirthDate { get;   set; }

    public string Description { get;   set; }
    //Rating of person
    public byte Rate { get;   set; }
    public string RuralGovName { get; set; }

}
