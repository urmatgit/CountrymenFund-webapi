using FSH.WebApi.Domain.Catalog.Fund;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Application.Catalog.Natives;
public   class CreateNativeRequestValidator: CustomValidator<CreateNativeRequest>
{
    public CreateNativeRequestValidator(IReadRepository<Native> nativeRepo,IReadRepository<RuralGov> ruralGovRepo,IStringLocalizer<CreateNativeRequestValidator> T )
    {
        RuleFor(p => p.Name)
            .NotEmpty()
            .MaximumLength(100);
        RuleFor(p => new { p.Name, p.Surname, p.BirthDate, p.RuralGovId})
             .MustAsync(async (x, ct) => await nativeRepo.GetBySpecAsync(new NativeCheckExistSpec(x.Name, x.Surname, x.BirthDate!.Value.ToUniversalTime(), x.RuralGovId), ct) is null)
             .WithMessage((_, x) => T["Native {0} already Exists.",$"{x.Name} {x.Surname } ({x.BirthDate}, {ruralGovRepo.GetByIdAsync(x.RuralGovId).Result?.Name})"]);

        RuleFor(p => p.Rate)
            .InclusiveBetween(1,10)
            .GreaterThanOrEqualTo(1);
        
        RuleFor(p => p.Image)
            .InjectValidator();
        RuleFor(p => p.RuralGovId)
            .MustAsync(async (id, ct) => await ruralGovRepo.GetByIdAsync(id, ct) is not null)
            .WithMessage((_, id) => T["Rural goverment {0} Not Found.", id]);
    }
}
