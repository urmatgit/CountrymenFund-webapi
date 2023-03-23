using FSH.WebApi.Domain.Catalog.Fund;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Application.Catalog.FSContributions;
public class CreateFSContributionRequestValidator: CustomValidator<CreateFSContributionRequest>
{
    public CreateFSContributionRequestValidator(IStringLocalizer<CreateFSContributionRequestValidator> T)
    {
        RuleFor(p => p.NativeId)
            .NotEmpty()
            .WithMessage(T["Native is not set"])
            .NotEqual(Guid.Empty)
            .WithMessage(T["Native is not set"]);
        RuleFor(p => p.FinSupportId)
            .NotEmpty()
            .WithMessage(T["FinSupport is not set"])
            .NotEqual(Guid.Empty)
            .WithMessage(T["FinSupport is not set"]);
        RuleFor(p => p.Summa)
            .NotEmpty()
            .WithMessage(T["Summa must be greater than 0.00"])
            .GreaterThan(0)
            .WithMessage(T["Summa must be greater than 0.00"]);
        
        //RuleFor(p => p.Year)
        //    .NotEmpty()
        //    .InclusiveBetween(2000, 2100);
    }
}
