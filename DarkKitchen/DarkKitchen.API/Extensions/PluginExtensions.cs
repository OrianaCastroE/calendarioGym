using System.Diagnostics.CodeAnalysis;
using DarkKitchen.Domain.Interfaces.Service;

namespace DarkKitchen.API.Extensions;

[ExcludeFromCodeCoverage]
public static class PluginExtensions
{
    public static WebApplicationBuilder ConfigurePluginsFolder(this WebApplicationBuilder builder)
    {
        var pluginsFolder = builder.Configuration["Plugins:Folder"];
        if(string.IsNullOrEmpty(pluginsFolder))
        {
            pluginsFolder = Path.Combine(builder.Environment.ContentRootPath, "Plugins");
            builder.Configuration["Plugins:Folder"] = pluginsFolder;
        }

        Directory.CreateDirectory(pluginsFolder);
        return builder;
    }

    public static WebApplication LoadPlugins(this WebApplication app)
    {
        _ = app.Services.GetRequiredService<IImporterRegistry>();
        return app;
    }
}
