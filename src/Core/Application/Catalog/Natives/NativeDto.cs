using FSH.WebApi.Application.Catalog.RuralGovs;
using FSH.WebApi.Domain.Catalog.Fund;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Application.Catalog.Natives;
public class NativeDto : IDto
{
    public DefaultIdType Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string? MiddleName { get; set; }
    public DateTime? BirthDate { get; set; }
    public string? ImagePath { get; set; }
    public string? Description { get; set; }
    //Rating of person
    
    public int Rate { get; set; }
    public DefaultIdType RuralGovId { get; set; }
    public string RuralGovName { get; set; }
}
