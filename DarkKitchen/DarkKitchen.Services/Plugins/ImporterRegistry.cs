using DarkKitchen.Domain.Exceptions;
using DarkKitchen.Domain.Interfaces.Service;
using Microsoft.Extensions.Configuration;

namespace DarkKitchen.Services.Plugins;

public class ImporterRegistry : IImporterRegistry
{
    private readonly Dictionary<string, IProductImporter> _importers
        = new(StringComparer.OrdinalIgnoreCase);

    public ImporterRegistry(IEnumerable<IProductImporter> builtIns, PluginLoader loader, IConfiguration configuration)
    {
        foreach(var importer in builtIns)
        {
            Register(importer);
        }

        var pluginsFolder = configuration["Plugins:Folder"] ?? "Plugins";
        foreach(var importer in loader.LoadFrom(pluginsFolder))
        {
            Register(importer);
        }
    }

    public IEnumerable<IProductImporter> GetAll() => _importers.Values;

    public IProductImporter Get(string name)
    {
        if(!_importers.TryGetValue(name, out var importer))
        {
            throw new NotFoundException($"Importer '{name}' not found.");
        }

        return importer;
    }

    private void Register(IProductImporter importer)
    {
        if(!_importers.TryAdd(importer.Name, importer))
        {
            throw new InvalidOperationException($"Duplicate importer name: '{importer.Name}'.");
        }
    }
}
