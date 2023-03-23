using FSH.WebApi.Application.Catalog.Contributions;
using FSH.WebApi.Application.Catalog.FinSupports;
using FSH.WebApi.Application.Catalog.FSContributions;
using FSH.WebApi.Application.Catalog.Natives;
using FSH.WebApi.Application.HomePage;
using FSH.WebApi.Domain.Catalog.Fund;
using FSH.WebApi.Domain.Common;
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
            .Map(d => d.Village, s => s.Native!.Village)
            .Map(d=>d.RuralGovName,s=>s.Native!.RuralGov!.Name);

        TypeAdapterConfig<FSContribution, FSContributionDto>.NewConfig()
            .Map(d => d.NativeFIO, s => $"{s.Native!.Name} {s.Native!.Surname} {s.Native!.MiddleName}")
            .Map(d => d.FinSuportName, s => s.FinSupport!.Name);

        TypeAdapterConfig<FSContribution, FSContributionExportDto>.NewConfig()
            .Map(d => d.NativeFIO, s => $"{s.Native!.Name} {s.Native!.Surname} {s.Native!.MiddleName}")
            .Map(d => d.FinSuportName, s => s.FinSupport!.Name);


        TypeAdapterConfig<Contribution, ContributionExportDto>.NewConfig()
            .Map(d => d.NativeFIO, s => $"{s.Native!.Name} {s.Native!.Surname} {s.Native!.MiddleName}")
            .Map(d => d.Year, s => s.Year!.year)
            .Map(d => d.Village, s => s.Native!.Village)
            .Map(d => d.RuralGovName, s => s.Native!.RuralGov!.Name);

        TypeAdapterConfig<ContributionDto, Contribution>.NewConfig()
            .Map(d => d.Date, s => s.Date!.Value.ToUniversalTime());
        TypeAdapterConfig<Native, NativeExportDto>.NewConfig()
            .Map(d => d.RuralGovName, s => s.RuralGov.Name)
            .Map(d=>d.BirthDate,s=>s.BirthDate.Value.Date);

        TypeAdapterConfig<FinSupport, FinSupportDto>.NewConfig()
            .Map(d => d.FIOManager, s => $"{s.Native.Name} {s.Native.Surname} {s.Native.MiddleName}");
        
        //TypeAdapterConfig<BlockImageDto, string>.NewConfig()
        //     .Map(d => d, s => s.Name);
        //TypeAdapterConfig<string, BlockImageDto>.NewConfig()
        //     .Map(d => d, s => new BlockImageDto() { Name=s} );
    }
}