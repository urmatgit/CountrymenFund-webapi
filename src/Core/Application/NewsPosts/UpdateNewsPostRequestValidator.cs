using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Application.NewsPosts;
public class UpdateNewsPostRequestValidator: CustomValidator<UpdateNewsPostRequest>
{
    public UpdateNewsPostRequestValidator()
{
    RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
    RuleFor(x => x.Body).NotEmpty();
        

    }
}
