
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;

namespace FSH.WebApi.Application.HomePage;
internal class HomePageBuilder
{
    public static string Serialize(MainPageModel model)
    {
         return JsonSerializer.Serialize(model);

    }
    private static JsonSerializerOptions DefaultSettings => SerializerSettings();

    private static JsonSerializerOptions SerializerSettings(bool indented = true)
    {
        var options = new JsonSerializerOptions
        {
            IgnoreNullValues = true,
            WriteIndented = indented,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        options.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));

        return options;
    }
    public static MainPageModel FromJson(string json) => JsonSerializer.Deserialize<MainPageModel>(json, DefaultSettings);
}
