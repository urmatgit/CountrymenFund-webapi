using FSH.WebApi.Domain.Catalog.Fund;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Application.Catalog.Contributions;
public class CreateContributionRequestValidator: CustomValidator<CreateContributionRequest>
{
    public CreateContributionRequestValidator(IStringLocalizer<CreateContributionRequestValidator> T)
    {
        RuleFor(p => p.Summa)
            .NotEmpty()
            .GreaterThan(0)
            .WithMessage(T["Summa must be greater than 0.00"]);
        RuleFor(p => p.Month)
            .IsInEnum();
        //RuleFor(p => p.Year)
        //    .NotEmpty()
        //    .InclusiveBetween(2000, 2100);
    }
}
