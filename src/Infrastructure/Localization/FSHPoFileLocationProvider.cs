using FSH.WebApi.Infrastructure.Common.Extensions;
using Hangfire.Logging;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OrchardCore.Localization;

namespace FSH.WebApi.Infrastructure.Localization;

/// <summary>
/// Provides PO files for FSH Localization.
/// </summary>
public class FSHPoFileLocationProvider : ILocalizationFileLocationProvider
{
    private readonly IFileProvider _fileProvider;
    private readonly string _resourcesContainer;
    private readonly ILogger<FSHPoFileLocationProvider> _logger;   
    public FSHPoFileLocationProvider(IHostEnvironment hostingEnvironment, IOptions<LocalizationOptions> localizationOptions, ILogger<FSHPoFileLocationProvider> log)
    {   
        _fileProvider = hostingEnvironment.ContentRootFileProvider;
        _resourcesContainer = localizationOptions.Value.ResourcesPath;
        _logger = log;
    }

    public IEnumerable<IFileInfo> GetLocations(string cultureName)
    {
        // Loads all *.po files from the culture folder under the Resource Path.
        // for example, src\Host\Localization\en-US\FSH.Exceptions.po
        _logger.LogInformation($"Current  culture is  {cultureName} {PathExtensions.Combine(_resourcesContainer, cultureName)}");
        var dirContent = _fileProvider.GetDirectoryContents(PathExtensions.Combine(_resourcesContainer, cultureName));
        _logger.LogInformation($"GetDirectoryContents count: {dirContent.Count()}");
        foreach (var file in _fileProvider.GetDirectoryContents(PathExtensions.Combine(_resourcesContainer, cultureName)))
        {
            _logger.LogInformation($"Culture file {file.Name} {file.PhysicalPath} ");
            yield return file;
        }
    }
}
