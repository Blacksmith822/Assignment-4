using System.Reflection;

namespace SupermarketReceipt.Tests;

public class AssemblyMetadataTests : DisposableTest
{
    private readonly Assembly _assembly;

    public AssemblyMetadataTests()
    {
        _assembly = typeof(Product).Assembly;
    }

    [Fact]
    public void SupermarketReceiptAssembly_VerifyNameOfAssembly_ExpectCorrectAssemblyName()
    {
        var assemblyName = _assembly.GetName().Name;
        Assert.Equal("SupermarketReceipt", assemblyName); 
    }

    [Fact]
    public void SupermarketReceiptAssembly_VerifyVersionNumber_ExpectCorrectVersionNumber()
    {
        var version = _assembly.GetName().Version;
        Assert.NotNull(version);
        Assert.Equal(new Version(1, 0, 0, 0), version);
    }

    [Fact]
    public void SupermarketReceiptAssembly_VerifyNumberOfReferencedAssemblies_ExpectCorrectNumberOfReferencedAssemblies()
    {
        var references = _assembly.GetReferencedAssemblies();
        Assert.NotNull(references);
        Assert.Equal(5, references.Length);
    }

    [Fact]
    public void SupermarketReceiptAssembly_VerifySystemDotLinqReferenced_ExpectLinqReferenced()
    {
        var references = _assembly.GetReferencedAssemblies();
        var hasSpecificDependency = references.Any(r => r.Name == "System.Linq");
        Assert.True(hasSpecificDependency);
    }

    [Fact]
    public void SupermarketReceiptAssembly_VerifyFileVersionNumberOfAssembly_ExpectCorrectFileVersion()
    {
        var filePath = _assembly.Location;
        var fileVersionInfo = System.Diagnostics.FileVersionInfo.GetVersionInfo(filePath);
        Assert.Equal("1.0.0.0", fileVersionInfo.FileVersion);
    }

    [Fact]
    public void SupermarketReceiptAssembly_VerifyCultureInformationOfAssembly_ExpectCorrectCultureInfo()
    {
        var cultureInfo = _assembly.GetName().CultureInfo;
        Assert.Equal(System.Globalization.CultureInfo.InvariantCulture, cultureInfo);
    }

    [Fact]
    public void SupermarketReceiptAssembly_VerifyManifestModuleInformation_ExpectCorrectManifestModule()
    {
        var manifestModule = _assembly.ManifestModule;
        Assert.NotNull(manifestModule);
        Assert.Equal("SupermarketReceipt.dll", manifestModule.Name); 
    }

    [Fact]
    public void SupermarketReceiptAssembly_VerifyEntryPointOfAssembly_ExpectNullEntryPoint()
    {
        var entryPoint = _assembly.EntryPoint;
        Assert.Null(entryPoint); 
    }
}