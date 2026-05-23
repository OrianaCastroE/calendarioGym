using System.Reflection;
using DarkKitchen.Domain.Interfaces.Service;

namespace DarkKitchen.Services.Plugins;

public class PluginLoader
{
    public virtual IEnumerable<IProductImporter> LoadFrom(string pluginsFolder)
    {
        if(!Directory.Exists(pluginsFolder))
        {
            return [];
        }

        var importers = new List<IProductImporter>();

        foreach(var dllPath in Directory.GetFiles(pluginsFolder, "*.dll"))
        {
            Assembly assembly;
            try
            {
                assembly = Assembly.LoadFrom(dllPath);
            }
            catch(BadImageFormatException)
            {
                continue;
            }

            Type[] types;
            try
            {
                types = assembly.GetTypes();
            }
            catch(ReflectionTypeLoadException ex)
            {
                types = ex.Types.Where(t => t != null).ToArray()!;
            }

            foreach(var type in types)
            {
                if(!typeof(IProductImporter).IsAssignableFrom(type)
                    || type.IsAbstract
                    || type.IsInterface
                    || type.GetConstructor(Type.EmptyTypes) == null)
                {
                    continue;
                }

                if(Activator.CreateInstance(type) is IProductImporter instance)
                {
                    importers.Add(instance);
                }
            }
        }

        return importers;
    }
}
