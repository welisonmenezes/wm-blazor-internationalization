using System;
using Microsoft.Extensions.DependencyInjection;

public static class WMBIExtension
{
    public static IServiceCollection AddWMBlazorInternationalization(
        this IServiceCollection services, 
        string defaultLanguage = "en",
        string defaultFileName = "Locale",
        string defaultFilePath = "i18ntext/")
    {
        return services.AddScoped<IWMBI>(p =>
        {
            var WMBI = ActivatorUtilities.CreateInstance<WMBIStrategy>(p);
            WMBI.Configure(defaultLanguage, defaultFileName, defaultFilePath);
            return WMBI;
        });
    }
}