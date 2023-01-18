using FSH.WebApi.Application.Catalog.RuralGovs;
using FSH.WebApi.Domain.Catalog.Fund;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Application.Catalog.Natives;
public class NativeDto: IDto
{
    public string Name { get; private set; }
    public string Surname { get; private set; }
    public string? MiddleName { get; private set; }
    public DateTime? BirthDate { get; private set; }
    public string? ImagePath { get; private set; }
    public string Description { get; private set; }
    //Rating of person
    public byte Rate { get; private set; }
    
    public  RuralGovDto RuralGov { get; private set; }
}
