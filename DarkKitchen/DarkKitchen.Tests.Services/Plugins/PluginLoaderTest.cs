using DarkKitchen.Services.Plugins;

namespace DarkKitchen.Tests.Services.Plugins;

[TestClass]
public class PluginLoaderTest
{
    private PluginLoader? loader;
    private string? tempFolder;

    [TestInitialize]
    public void Setup()
    {
        loader = new PluginLoader();
        tempFolder = Directory.CreateTempSubdirectory().FullName;
    }

    [TestCleanup]
    public void Cleanup()
    {
        if(Directory.Exists(tempFolder!))
        {
            Directory.Delete(tempFolder!, recursive: true);
        }
    }

    [TestMethod]
    public void LoadFrom_WhenFolderDoesNotExist_ReturnsEmpty()
    {
        var missing = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());

        var result = loader!.LoadFrom(missing).ToList();

        Assert.AreEqual(0, result.Count);
    }

    [TestMethod]
    public void LoadFrom_WhenFolderHasNoDlls_ReturnsEmpty()
    {
        var result = loader!.LoadFrom(tempFolder!).ToList();

        Assert.AreEqual(0, result.Count);
    }

    [TestMethod]
    public void LoadFrom_WhenFolderHasValidPluginDll_ReturnsImporterInstance()
    {
        var sourceDll = typeof(TestPluginImporter).Assembly.Location;
        File.Copy(sourceDll, Path.Combine(tempFolder!, Path.GetFileName(sourceDll)));

        var result = loader!.LoadFrom(tempFolder!).ToList();

        Assert.IsTrue(result.Any(i => i is TestPluginImporter));
    }

    [TestMethod]
    public void LoadFrom_WhenDllIsMalformed_SkipsItAndReturnsEmpty()
    {
        File.WriteAllText(Path.Combine(tempFolder!, "bogus.dll"), "not a real assembly");

        var result = loader!.LoadFrom(tempFolder!).ToList();

        Assert.AreEqual(0, result.Count);
    }
}
