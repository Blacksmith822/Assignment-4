using System.Reflection;

namespace SupermarketReceipt.Tests;

public class CatalogTests : DisposableTest
{
    [Fact]
    public void Catalog_VerifyNameOfClass_ExpectClassNameIsCatalog()
    {
        Assert.Equal("Catalog", nameof(Catalog)); 
    }
    
    [Fact]
    public void Catalog_VerifyImplementsICatalog_ExpectItDoesImplementICatalog()
    {
        var classType = typeof(Catalog);
        var interfaceType = typeof(ICatalog);
        var implementsInterface =  interfaceType.IsAssignableFrom(classType);
        Assert.True(implementsInterface, $"{classType.Name} should implement {interfaceType.Name}.");
    }
    
    [Fact]
    public void LoadCatalog_VerifyIsAPrivateMethod_ExpectItIsPrivateAndInstanceMethod()
    {
        var classType = typeof(Catalog);
        const string expectedMethodName = "LoadCatalog";
        var privateMethod = classType.GetMethod(expectedMethodName, BindingFlags.NonPublic | BindingFlags.Instance);
        Assert.NotNull(privateMethod);
        Assert.Equal(expectedMethodName, privateMethod.Name);
    }
    
    [Fact]
    public void SaveCatalog_VerifyIsAPrivateMethod_ExpectItIsPrivateAndInstanceMethod()
    {
        var classType = typeof(Catalog);
        const string expectedMethodName = "SaveCatalog";
        var privateMethod = classType.GetMethod(expectedMethodName, BindingFlags.NonPublic | BindingFlags.Instance);
        Assert.NotNull(privateMethod);
        Assert.Equal(expectedMethodName, privateMethod.Name);
    }
    
    [Fact]
    public void GetJsonSettings_VerifyIsAPrivateMethod_ExpectItIsPrivateAndStaticMethod()
    {
        var classType = typeof(Catalog);
        const string expectedMethodName = "GetJsonSettings";
        var privateMethod = classType.GetMethod(expectedMethodName, BindingFlags.NonPublic | BindingFlags.Static);
        Assert.NotNull(privateMethod);
        Assert.Equal(expectedMethodName, privateMethod.Name);
    }

    [Fact]
    public void _filePath_VerifyIsAPrivateField_ExpectItIsPrivate()
    {
        var fieldInfo = typeof(Catalog)
            .GetField
            (
                "_filePath", 
                BindingFlags.NonPublic | BindingFlags.Instance
            );
        
        Assert.NotNull(fieldInfo);
        Assert.Equal("_filePath", fieldInfo.Name);
        Assert.True(fieldInfo.IsPrivate);
    }

    [Fact]
    public void _productCatalog_VerifyIsAPrivateField_ExpectItIsPrivate()
    {
        var fieldInfo = typeof(Catalog)
            .GetField
            (
                "_productCatalog", 
                BindingFlags.NonPublic | BindingFlags.Instance
            );
        
        Assert.NotNull(fieldInfo);
        Assert.Equal("_productCatalog", fieldInfo.Name);
        Assert.True(fieldInfo.IsPrivate);
    }
    
    [Fact]
    public void Catalog_VerifyNumberOfPublicMethods_ExpectElevenPublicMethods()
    {
        var classType = typeof(Catalog);
        const int expectedPublicMethodCount = 13;
        var publicMethods = classType.GetMethods(BindingFlags.Public | BindingFlags.Instance);
        Assert.Equal(expectedPublicMethodCount, publicMethods.Length);
    }

    [Fact]
    public void Contains_WhenAProductHasBeenAddedToTheCatalog_ExpectCatalogContainsTheProduct()
    {
        const string productId = "TEST";
        var product = new Product(productId, 1);
        var catalog = new Catalog();
        catalog.Clear();
        catalog.AddProduct(product);
        Assert.True(catalog.Contains(productId));
        
    }

    [Fact]
    public void Contains_WhenAProductHasNotBeenAddedToTheCatalog_ExpectCatalogDoesNotContainTheProduct()
    {
        const string productId = "TEST";
        var catalog = new Catalog();
        catalog.Clear();
        Assert.False(catalog.Contains(productId));
    }

    [Fact]
    public void Constructor_NewCatalogExecutesLoadCatalog_ExpectLoadMethodInvoked()
    {
        var catalog = new Catalog();
        Assert.True(catalog.LoadCatalogExecuted);
    }

    [Fact]
    public void Constructor_NewCatalogDoesNotExecuteSaveCatalogExecuted_ExpectSaveMethodNotInvoked()
    {
        var catalog = new Catalog();
        Assert.False(catalog.SaveCatalogExecuted);
    }

    [Fact]
    public void Clear_CatalogExecutesSaveCatalog_ExpectSaveMethodNotInvoked()
    {
        var catalog = new Catalog();
        catalog.Clear();
        Assert.True(catalog.SaveCatalogExecuted);
    }

    [Fact]
    public void Clear_ACatalogWithOneItemIsCleared_ExpectNumberOfProductsIsZero()
    {
        var catalog = new Catalog();
        catalog.Clear();
        catalog.AddProduct(new Product("TEST", 1));
        Assert.Equal(1, catalog.NumberOfProducts);
        catalog.Clear();
        Assert.Equal(0, catalog.NumberOfProducts);
    }

    [Fact]
    public void Product_RequestProductNotInCatalog_ExpectAnException()
    {
        const string productId = "TEST";
        var catalog = new Catalog();
        catalog.Clear();
        var exception = Assert.Throws<ApplicationException>(() => catalog.Product(productId));
        Assert.Equal($"Product with ID {productId} not found in the catalog.", exception.Message);
    }

    [Fact]
    public void Product_RequestProductInCatalog_ExpectProduct()
    {
        const string productId = "TEST";
        var product = new Product(productId, 1);
        var catalog = new Catalog();
        catalog.Clear();
        catalog.AddProduct(product);
        var productFromCatalog = catalog.Product(productId);
        Assert.Equal(product.ProductId, productFromCatalog.ProductId);
    }

    [Fact]
    public void AddProduct_AddAProductAlreadyInTheCatalog_ExpectAnException()
    {
        const string productId = "TEST";
        var product = new Product(productId, 1);
        var catalog = new Catalog();
        catalog.Clear();
        catalog.AddProduct(product);
        var exception = Assert.Throws<ApplicationException>(() => catalog.AddProduct(product));
        Assert.Equal("Cannot add a product that is already in the catalog.", exception.Message);
    }

    [Fact]
    public void AddProduct_AddAProductToTheCatalog_ExpectProductAdded()
    {
        const string productId = "TEST";
        var product = new Product(productId, 1);
        var catalog = new Catalog();
        catalog.Clear();
        catalog.AddProduct(product);
        Assert.Equal(1, catalog.NumberOfProducts);
        Assert.Equal(product.ProductId, catalog.Product(productId).ProductId);
    }

    [Fact]
    public void RemoveProduct_RemoveAProductNotInTheCatalog_ExpectAnException()
    {
        const string productId = "TEST";
        var catalog = new Catalog();
        catalog.Clear();
        var exception = Assert.Throws<ApplicationException>(() => catalog.RemoveProduct(productId));
        Assert.Equal("Cannot remove a product that is not in the catalog.", exception.Message);
    }

    [Fact]
    public void RemoveProduct_RemoveAProductThatIsInTheCatalog_ExpectProductRemoved()
    {
        const string productId = "TEST";
        var product = new Product(productId, 1);
        var catalog = new Catalog();
        catalog.Clear();
        catalog.AddProduct(product);
        catalog.RemoveProduct(productId);
        Assert.Equal(0, catalog.NumberOfProducts);
    }

    [Fact]
    public void ApplySpecialDeal_ApplySpecialDealToAProductThatIsInTheCatalog_ExpectSpecialDealApplied()
    {
        const string productId = "TEST";
        var product = new Product(productId, 1);
        var catalog = new Catalog();
        catalog.Clear();
        catalog.AddProduct(product);
        catalog.ApplySpecialDeal(new PercentDiscountSpecialDeal(10), productId);
        var productFromCatalog = catalog.Product(productId);
        Assert.IsType<PercentDiscountSpecialDeal>(productFromCatalog.Deal);
        Assert.Equal(10, productFromCatalog.Deal.Discount(1,1));
    }

    [Fact]
    public void Clear_RemoveAllTheProductsInTheCatalogWithMultipleProducts_ExpectAllProductsRemoved()
    {
        const string productId1 = "TEST1";
        var product1 = new Product(productId1, 1);
        const string productId2 = "TEST2";
        var product2 = new Product(productId2, 1);
        var catalog = new Catalog();
        catalog.Clear();
        catalog.AddProduct(product1);
        catalog.AddProduct(product2);
        catalog.Clear();
        Assert.Equal(0, catalog.NumberOfProducts);
    }

    [Fact]
    public void LoadCatalog_CreateCatalogJsonFileWhenItDoesNotExist_ExpectFileCreated()
    {
        var catalog = new Catalog();
        catalog.Clear();
        Assert.False(File.Exists("catalog.json"));
        catalog = new Catalog();
        Assert.NotNull(catalog);
        Assert.True(File.Exists("catalog.json"));
    }
    
    [Fact]
    public void Price_DefinedInReceiptClassFile_ExpectDefinitionInSourceCodeFile()
    {
        const string filePath = "../../../../SupermarketReceipt/ICatalog.cs";
        Assert.True(File.Exists(filePath), $"The file '{filePath}' does not exist.");

        var sourceCode = File.ReadAllText(filePath);

        const string expectedSource = "namespace SupermarketReceipt;\r\n\r\npublic interface ICatalog\r\n{\r\n    int NumberOfProducts { get; }\r\n    bool Contains(string productId);\r\n    Product Product(string productId);\r\n    void AddProduct(Product product);\r\n    void RemoveProduct(string productId);\r\n\r\n    void ApplySpecialDeal\r\n    (\r\n        SpecialDeal deal,\r\n        string productId\r\n    );\r\n\r\n    void Clear();\r\n}";
        
        Assert.Equal(expectedSource, sourceCode);
    }
}