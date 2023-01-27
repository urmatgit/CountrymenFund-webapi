using FSH.WebApi.Application.Catalog.Contributions;
using FSH.WebApi.Domain.Catalog.Fund;
using Mapster;

namespace FSH.WebApi.Infrastructure.Mapping;

public class MapsterSettings
{
    public static void Configure()
    {
        // here we will define the type conversion / Custom-mapping
        // More details at https://github.com/MapsterMapper/Mapster/wiki/Custom-mapping

        // This one is actually not necessary as it's mapped by convention
        // TypeAdapterConfig<Product, ProductDto>.NewConfig().Map(dest => dest.BrandName, src => src.Brand.Name);
        TypeAdapterConfig<Contribution, ContributionDto>.NewConfig()
            .Map(d => d.NativeFIO, s => $"{s.Native!.Name} {s.Native!.Surname} {s.Native!.MiddleName}")
            .Map(d => d.Rate, s => s.Native!.Rate)
            .Map(d => d.Year, s => s.Year!.year)
            .Map(d => d.Village, s => s.Native!.Village);
        TypeAdapterConfig<ContributionDto, Contribution>.NewConfig()
            .Map(d => d.Date, s => s.Date!.Value.ToUniversalTime());
    }
}