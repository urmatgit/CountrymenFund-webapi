﻿using FSH.WebApi.Domain.Catalog.Fund;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Application.Catalog.Natives;
public class UpdateNativeRequestValidator: CustomValidator<UpdateNativeRequest>
{
    public UpdateNativeRequestValidator(IReadRepository<Native> nativeRep,IReadRepository<RuralGov> ruralGovRep,IStringLocalizer<UpdateNativeRequestValidator> T)
    {
        RuleFor(p => p.Name)
            .NotEmpty()
            .MaximumLength(100);
        RuleFor(p => new { p.Name, p.Surname, p.BirthDate, p.RuralGovId,p.Image })
             .MustAsync(async (x, ct) => await nativeRep.GetBySpecAsync(new NativeCheckExistSpec(x.Name, x.Surname, x.BirthDate!.Value.ToUniversalTime(), x.RuralGovId,x.Image!=null), ct) is null)
             .WithMessage((_, x) => T["Native {0} already Exists.", $"{x.Name} {x.Surname} ({x.BirthDate}, {ruralGovRep.GetByIdAsync(x.RuralGovId).Result?.Name})"]);
        //RuleFor(p => p.Rate)
        //    .InclusiveBetween (1, 5)
        //    .WithMessage((_) => T["The rating should be from 1 to 5"])
        //    .GreaterThanOrEqualTo(1);
        //RuleFor(p => p.Image)
        //    .InjectValidator();
        RuleFor(p => p.RuralGovId)
            .MustAsync(async (id, ct) => await ruralGovRep.GetByIdAsync(id, ct) is not null)
            .WithMessage((_, id) => T["Rural goverment {0} Not Found.", id]);
    }
}
