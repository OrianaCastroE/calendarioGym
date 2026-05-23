using DarkKitchen.Domain.Exceptions;
using DarkKitchen.Domain.Interfaces.Service;
using DarkKitchen.Models.ProductDTOs;
using DarkKitchen.Services.Plugins;
using Microsoft.Extensions.Configuration;
using Moq;

namespace DarkKitchen.Tests.Services.Plugins;

[TestClass]
public class ImporterRegistryTest
{
    private Mock<PluginLoader>? loaderMock;
    private Mock<IConfiguration>? configurationMock;

    [TestInitialize]
    public void Setup()
    {
        loaderMock = new Mock<PluginLoader>();
        loaderMock.Setup(l => l.LoadFrom(It.IsAny<string>())).Returns([]);

        configurationMock = new Mock<IConfiguration>();
        configurationMock.Setup(c => c["Plugins:Folder"]).Returns("test-plugins");
    }

    [TestMethod]
    public void GetAll_WhenBuiltInsRegistered_ReturnsAll()
    {
        var json = new FakeImporter("JSON", ".json");
        var xml = new FakeImporter("XML", ".xml");
        var registry = new ImporterRegistry([json, xml], loaderMock!.Object, configurationMock!.Object);

        var all = registry.GetAll().ToList();

        Assert.AreEqual(2, all.Count);
    }

    [TestMethod]
    public void Get_WhenNameMatches_ReturnsImporter()
    {
        var json = new FakeImporter("JSON", ".json");
        var registry = new ImporterRegistry([json], loaderMock!.Object, configurationMock!.Object);

        var result = registry.Get("JSON");

        Assert.AreSame(json, result);
    }

    [TestMethod]
    public void Get_WhenNameIsCaseInsensitive_ReturnsImporter()
    {
        var json = new FakeImporter("JSON", ".json");
        var registry = new ImporterRegistry([json], loaderMock!.Object, configurationMock!.Object);

        var result = registry.Get("json");

        Assert.AreSame(json, result);
    }

    [TestMethod]
    [ExpectedException(typeof(NotFoundException))]
    public void Get_WhenNameUnknown_ThrowsNotFound()
    {
        var registry = new ImporterRegistry([], loaderMock!.Object, configurationMock!.Object);

        registry.Get("Unknown");
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void Constructor_WhenDuplicateBuiltInNames_Throws()
    {
        var a = new FakeImporter("JSON", ".json");
        var b = new FakeImporter("JSON", ".json");

        _ = new ImporterRegistry([a, b], loaderMock!.Object, configurationMock!.Object);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void Constructor_WhenPluginNameCollidesWithBuiltIn_Throws()
    {
        var builtIn = new FakeImporter("JSON", ".json");
        var plugin = new FakeImporter("JSON", ".json");
        loaderMock!.Setup(l => l.LoadFrom(It.IsAny<string>())).Returns([plugin]);

        _ = new ImporterRegistry([builtIn], loaderMock.Object, configurationMock!.Object);
    }

    [TestMethod]
    public void Constructor_WhenPluginsFolderNotConfigured_FallsBackToDefault()
    {
        var emptyConfig = new Mock<IConfiguration>();

        _ = new ImporterRegistry([], loaderMock!.Object, emptyConfig.Object);

        loaderMock.Verify(l => l.LoadFrom("Plugins"), Times.Once);
    }

    [TestMethod]
    public void GetAll_WhenPluginsLoadedFromLoader_IncludesThem()
    {
        var plugin = new FakeImporter("CSV", ".csv");
        loaderMock!.Setup(l => l.LoadFrom(It.IsAny<string>())).Returns([plugin]);
        var builtIn = new FakeImporter("JSON", ".json");

        var registry = new ImporterRegistry([builtIn], loaderMock.Object, configurationMock!.Object);
        var all = registry.GetAll().ToList();

        Assert.AreEqual(2, all.Count);
        Assert.AreSame(plugin, registry.Get("CSV"));
    }

    private sealed class FakeImporter(string name, string extension) : IProductImporter
    {
        public string Name { get; } = name;
        public string Extension { get; } = extension;
        public IEnumerable<ImportedProductDto> Import(Stream source) => [];
    }
}
