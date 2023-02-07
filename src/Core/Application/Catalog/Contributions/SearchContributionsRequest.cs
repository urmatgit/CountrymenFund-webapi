using FSH.WebApi.Domain.Catalog.Fund;
using FSH.WebApi.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Application.Catalog.Contributions;
public class SearchContributionsRequest: PaginationFilter,IRequest<PaginationResponse<ContributionDto>>
{
    public DefaultIdType? YearId { get; set; }
    public DefaultIdType? NativeId { get; set; }
    public DefaultIdType? RuralGovId { get; set; }
    public Months? Month { get; set; }
    public DateTime? DateStart { get; set; }
    public DateTime? DateEnd { get; set; }

    public string? FIO { get; set; }

}
public class SearchContributionsRequestHandler: IRequestHandler<SearchContributionsRequest, PaginationResponse<ContributionDto>>
{
    private readonly IReadRepository<Contribution> _repository;
    public SearchContributionsRequestHandler(IReadRepository<Contribution> repository)
    {
        _repository = repository;
    }

    public async Task<PaginationResponse<ContributionDto>> Handle(SearchContributionsRequest request, CancellationToken cancellationToken)
    {
         if (request.HasOrderBy())
        {
            for (int i=0;i<request.OrderBy.Length;i++)
            {
                var orderby = request.OrderBy[i];
                switch(orderby)
                {
                    case string s when s.StartsWith("Year"):
                        orderby= orderby.Replace("Year", "Year.year"); break;
                    case string s when s.StartsWith("NativeFIO"):
                        orderby = orderby.Replace("NativeFIO", "Native.Name"); break;
                    case string s when s.StartsWith("Rate"):
                        orderby = orderby.Replace("Rate", "Native.Rate"); break;
                    case string s when s.StartsWith("RuralGovName"):
                        orderby = orderby.Replace("RuralGovName", "Native.RuralGov.Name"); break;


                }
                request.OrderBy[i] = orderby;

            }
        }
        // if (!string.IsNullOrEmpty(request.Keyword) && request.AdvancedSearch==null)
        //{
        //    Search search = new Search();
        //    search.Fields.Add("Native.Name");
        //    search.Fields.Add("Native.Rate");
        //    search.Fields.Add("Year.year");
        //    search.Fields.Add("Native.RuralGov.Name");
        //    search.Keyword = request.Keyword;
        //    request.AdvancedSearch = search;

            

        //}

            var spec = new ContributionsBySearchRequestSpec(request);
        return await _repository.PaginatedListAsync(spec, request.PageNumber, request.PageSize, cancellationToken);
    }
}
