using DarkKitchen.Services.Plugins;

namespace DarkKitchen.Tests.Services.Plugins;

[TestClass]
public class PluginLoaderTest
{
    private PluginLoader? loader;

    [TestInitialize]
    public void Setup()
    {
        loader = new PluginLoader();
    }

    [TestMethod]
    public void LoadFrom_WhenFolderDoesNotExist_ReturnsEmpty()
    {
        var result = loader!.LoadFrom(Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString())).ToList();

        Assert.AreEqual(0, result.Count);
    }

    [TestMethod]
    public void LoadFrom_WhenFolderHasNoDlls_ReturnsEmpty()
    {
        var folder = Directory.CreateTempSubdirectory().FullName;
        try
        {
            var result = loader!.LoadFrom(folder).ToList();

            Assert.AreEqual(0, result.Count);
        }
        finally
        {
            Directory.Delete(folder);
        }
    }
}
