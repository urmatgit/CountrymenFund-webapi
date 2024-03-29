﻿using System;
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
    public string? Village { get; private set; }
    public string? ImagePath { get; private set; }
    public string? Description { get; private set; }
    //Rating of person
    public int Rate { get; private set; }
    public DefaultIdType RuralGovId {  get; private set; }
    public virtual RuralGov RuralGov { get; private set; }
    public Native()
    {

    }
    public Native(string name,string surname,string? middlename,DateTime? birthdate,string village, string description,int rate, DefaultIdType ruralGovId,string imagepath )
    {
        Name= name;
        Surname= surname;
        middlename = middlename;
        BirthDate= birthdate;
        Description= description;
        RuralGovId= ruralGovId;
        Rate= rate;
        Village= village;
        ImagePath= imagepath;
    }

    public Native Update(string? name, string? surname, string? middlename, DateTime? birthdate,string? village, string? description,int rate,  DefaultIdType? ruralGovId,string imagepath)
    {
        if (name is not null && Name?.Equals(name) is not true) Name = name;
        if (surname is not null && Surname?.Equals(surname) is not true) Surname = surname;
        if (middlename is not null && MiddleName?.Equals(middlename) is not true) MiddleName= middlename;
        if (village is not null && Village?.Equals(village) is not true) Village = village;
        if (birthdate.HasValue && BirthDate?.Equals(birthdate) is not true) BirthDate = birthdate.Value;
        if (description is not null && Description?.Equals(description) is not true) Description= description;
        if (rate !=Rate) Rate= rate;
        if (ruralGovId.HasValue && ruralGovId.Value!=DefaultIdType.Empty && !RuralGovId.Equals(ruralGovId.Value)) RuralGovId= ruralGovId.Value;
        if (imagepath!=ImagePath) ImagePath= imagepath;
        return this;
    }
    public Native ClearImagePath()
    {
        ImagePath = string.Empty;
        return this;
    }
}
