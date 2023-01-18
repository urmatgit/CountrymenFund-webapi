using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Domain.Catalog.Fund;
public class Native : AuditableEntity, IAggregateRoot
{
    public string  Name { get; private set; }
    public string Surname { get; private set; }
    public string? MiddleName { get; private set; }
    public DateTime? BirthDate { get; private set; }
    public string? ImagePath { get; private set; }
    public string Description { get; private set; }
    //Rating of person
    public byte Rate { get; private set; }
    public DefaultIdType RuralGovId { private get; set; }
    public virtual RuralGov RuralGov { get; private set; }
    public Native()
    {

    }
    public Native(string name,string surname,string? middlename,DateTime? birthdate,string description,DefaultIdType ruralGovId )
    {
        Name= name;
        Surname= surname;
        middlename = middlename;
        BirthDate= birthdate;
        Description= description;
        RuralGovId= ruralGovId;
    }

    public Native Update(string? name, string? surname, string? middlename, DateTime? birthdate, string? description, DefaultIdType? ruralGovId)
    {
        if (name is not null && Name?.Equals(name) is not true) Name = name;
        if (surname is not null && Surname?.Equals(surname) is not true) Surname = surname;
        if (middlename is not null && MiddleName?.Equals(middlename) is not true) MiddleName= middlename;
        if (birthdate.HasValue && BirthDate?.Equals(birthdate) is not true) BirthDate = birthdate.Value;
        if (description is not null && Description?.Equals(description) is not true) Description= description;
        if (ruralGovId.HasValue && ruralGovId.Value!=DefaultIdType.Empty && !RuralGovId.Equals(ruralGovId.Value)) RuralGovId= ruralGovId.Value;

        return this;
    }
    public Native ClearImagePath()
    {
        ImagePath = string.Empty;
        return this;
    }
}
