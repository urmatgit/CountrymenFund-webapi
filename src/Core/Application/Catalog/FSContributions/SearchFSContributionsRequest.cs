using FSH.WebApi.Domain.Catalog.Fund;
using FSH.WebApi.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Application.Catalog.FSContributions;
public class SearchFSContributionsRequest: PaginationFilter,IRequest<PaginationResponse<FSContributionDto>>
{

    public DefaultIdType? NativeId { get; set; }
    public DefaultIdType? FinSupportId { get; set; }
    public DateTime? DateStart { get; set; }
    public DateTime? DateEnd { get; set; }

    public string? FIO { get; set; }

}
public class SearchFSContributionsRequestHandler: IRequestHandler<SearchFSContributionsRequest, PaginationResponse<FSContributionDto>>
{
    private readonly IReadRepository<FSContribution> _repository;
    public SearchFSContributionsRequestHandler(IReadRepository<FSContribution> repository)
    {
        _repository = repository;
    }

    public async Task<PaginationResponse<FSContributionDto>> Handle(SearchFSContributionsRequest request, CancellationToken cancellationToken)
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
        request.FIO = request.Keyword;
        request.Keyword = "";
            var spec = new FSContributionsBySearchRequestSpec(request);
        return await _repository.PaginatedListAsync(spec, request.PageNumber, request.PageSize, cancellationToken);
    }
}
