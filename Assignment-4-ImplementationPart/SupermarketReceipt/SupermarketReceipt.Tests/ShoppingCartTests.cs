using System.Reflection;

namespace SupermarketReceipt.Tests;

public class ShoppingCartTests : DisposableTest
{
    [Fact]
    public void ShoppingCart_VerifyNameOfClass_ExpectClassNameIsShoppingCart()
    {
        Assert.Equal("ShoppingCart", nameof(ShoppingCart)); 
    }
    
    [Fact]
    public void ShoppingCart_VerifyNumberOfPublicProperties_ExpectOnePublicProperties()
    {
        var type = typeof(ShoppingCart); 
        var publicProperties = type.GetProperties();
        var propertyCount = publicProperties.Length;
        Assert.Equal(1, propertyCount);
    }
    
    [Fact]
    public void ShoppingCart_VerifyNumberOfPrivateFields_ExpectTwoPrivateFields()
    {
        Assert.Equal
        (
            2,
            typeof(ShoppingCart)
                .GetFields(BindingFlags.NonPublic | BindingFlags.Instance)
                .Length
        );
    }
    
    [Fact]
    public void NumberOfItems_VerifyPropertyExists_ExpectPropertyExists()
    {
        var property = typeof(ShoppingCart).GetProperty("NumberOfItems", BindingFlags.Public | BindingFlags.Instance);
        Assert.NotNull(property);
        Assert.Equal("NumberOfItems", property.Name); 
    }
    
    [Fact]
    public void AddItemByProductId_VerifySpecificNameAndParameters_ExpectExistsWithOneStringParameter()
    {
        var method = typeof(ShoppingCart).GetMethod("AddItemByProductId", BindingFlags.Public | BindingFlags.Instance);
        Assert.NotNull(method);
        Assert.Equal("AddItemByProductId", method.Name);
        Assert.Single(method.GetParameters());
        Assert.Equal(typeof(string), method.GetParameters()[0].ParameterType);
    }
    
    [Fact]
    public void RemoveItemByProductId_VerifySpecificNameAndParameters_ExpectExistsWithOneStringParameter()
    {
        var method = typeof(ShoppingCart).GetMethod("RemoveItemByProductId", BindingFlags.Public | BindingFlags.Instance);
        Assert.NotNull(method);
        Assert.Equal("RemoveItemByProductId", method.Name);
        Assert.Single(method.GetParameters());
        Assert.Equal(typeof(string), method.GetParameters()[0].ParameterType);
    }
    
    [Fact]
    public void Receipt_VerifySpecificNameAndParameters_ExpectExistsWithNoParameters()
    {
        var method = typeof(ShoppingCart).GetMethod("Receipt", BindingFlags.Public | BindingFlags.Instance);
        Assert.NotNull(method);
        Assert.Equal("Receipt", method.Name);
        Assert.Empty(method.GetParameters());
    }
    
    [Fact]
    public void Constructor_VerifyParameter_ExpectTakesOneCatalogParameter()
    {
        var constructors = typeof(ShoppingCart).GetConstructors(BindingFlags.Public | BindingFlags.Instance);
        var constructor = constructors.FirstOrDefault(c => c.GetParameters().Length == 1);
        Assert.NotNull(constructor);
        Assert.Single(constructors);
        Assert.Equal(typeof(ICatalog), constructor.GetParameters().First().ParameterType);
    }
    
    [Fact]
    public void NumberOfItems_CountItemsInEmptyCart_ExpectNumberOfItemsIsZero()
    {
        var catalog = new Catalog();
        var shoppingCart = new ShoppingCart(catalog);
        Assert.Equal(0, shoppingCart.NumberOfItems);
    }
    
    [Fact]
    public void NumberOfItems_CountItemsWhenCartHasOneItem_ExpectNumberOfItemsIsOne()
    {
        const string productId = "TEST";
        var catalog = new MockCatalog();
        var shoppingCart = new ShoppingCart(catalog);
        shoppingCart.AddItemByProductId(productId);
        Assert.Equal(1, shoppingCart.NumberOfItems);
        Assert.Equal(1 ,catalog.NumberOfInvocationsOfContains);
        Assert.Equal(1 ,catalog.NumberOfInvocationsOfProduct);
    }
    
    [Fact]
    public void NumberOfItems_CountItemsWhenCartHasTwoItems_ExpectNumberOfItemsIsTwo()
    {
        const string productId = "TEST";
        var catalog = new MockCatalog();
        var shoppingCart = new ShoppingCart(catalog);
        shoppingCart.AddItemByProductId(productId);
        shoppingCart.AddItemByProductId(productId);
        Assert.Equal(2, shoppingCart.NumberOfItems);
        Assert.Equal(2 ,catalog.NumberOfInvocationsOfContains);
        Assert.Equal(2 ,catalog.NumberOfInvocationsOfProduct);
    }
    
    [Fact]
    public void NumberOfItems_CountItemsWhenCartHasThreeItems_ExpectNumberOfItemsIsThree()
    {
        const string productId = "TEST";
        var catalog = new MockCatalog();
        var shoppingCart = new ShoppingCart(catalog);
        shoppingCart.AddItemByProductId(productId);
        shoppingCart.AddItemByProductId(productId);
        shoppingCart.AddItemByProductId(productId);
        Assert.Equal(3, shoppingCart.NumberOfItems);
        Assert.Equal(3 ,catalog.NumberOfInvocationsOfContains);
        Assert.Equal(3 ,catalog.NumberOfInvocationsOfProduct);
    }
    
    [Fact]
    public void NumberOfItems_CountItemsWhenCartHasThreeItemsAndOneIsRemoved_ExpectNumberOfItemsIsTwo()
    {
        const string productId = "TEST";
        var catalog = new MockCatalog();
        var shoppingCart = new ShoppingCart(catalog);
        shoppingCart.AddItemByProductId(productId);
        shoppingCart.AddItemByProductId(productId);
        shoppingCart.AddItemByProductId(productId);
        shoppingCart.RemoveItemByProductId(productId);
        Assert.Equal(2, shoppingCart.NumberOfItems);
        Assert.Equal(3 ,catalog.NumberOfInvocationsOfContains);
        Assert.Equal(3 ,catalog.NumberOfInvocationsOfProduct);
    }
    
    [Fact]
    public void AddItemByProductId_CannotAddProductNotInCatalog_ExpectException()
    {
        const string productId = "TEST";
        var catalog = new MockCatalog(false);
        var shoppingCart = new ShoppingCart(catalog);
        var exception = Assert.Throws<ApplicationException>(() => shoppingCart.AddItemByProductId(productId));
        Assert.Equal("Cannot add an item to the cart that is not in the catalog.", exception.Message);
        Assert.Equal(1 ,catalog.NumberOfInvocationsOfContains);
    }
    
    [Fact]
    public void RemoveItemByProductId_CannotRemoveAProductNotInTheCart_ExpectException()
    {
        const string productId = "TEST";
        var catalog = new MockCatalog();
        var shoppingCart = new ShoppingCart(catalog);
        var exception = Assert.Throws<ApplicationException>(() => shoppingCart.RemoveItemByProductId(productId));
        Assert.Equal("Cannot remove an item that is not in the cart.", exception.Message);
    }
    
    [Fact]
    public void Receipt_CannotGenerateReceiptForEmptyShoppingCart_ExpectException()
    {
        var catalog = new MockCatalog();
        var shoppingCart = new ShoppingCart(catalog);
        var exception = Assert.Throws<ApplicationException>(() => shoppingCart.Receipt());
        Assert.Equal("Cannot produce receipt for an empty cart.", exception.Message);
    }
    
    [Fact]
    public void Receipt_GenerateReceiptForShoppingCart_ExpectReceipt()
    {
        var catalog = new MockCatalog();
        var shoppingCart = new ShoppingCart(catalog);
        shoppingCart.AddItemByProductId("TEST");
        shoppingCart.Receipt();
    }

    private class MockCatalog : ICatalog
    {
        private readonly bool _containsProductFlag;
        public int NumberOfProducts { get; } = 0;
        public int NumberOfInvocationsOfContains { get; private set; } = 0;
        public int NumberOfInvocationsOfProduct { get; private set; } = 0;

        public MockCatalog(bool containsProductFlag = true)
        {
            _containsProductFlag = containsProductFlag;
        }
        public bool Contains
        (
            string productId
        )
        {
            NumberOfInvocationsOfContains++;
            return _containsProductFlag;
        }

        public Product Product
        (
            string productId
        )
        {
            NumberOfInvocationsOfProduct++;
            return new Product(productId, 1);
        }

        public void AddProduct
        (
            Product product
        )
        {
        }

        public void RemoveProduct
        (
            string productId
        )
        {
        }

        public void ApplySpecialDeal
        (
            SpecialDeal deal,
            string productId
        )
        {
        }

        public void Clear()
        {
        }
    }
}